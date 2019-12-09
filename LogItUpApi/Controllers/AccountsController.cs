using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LogItUpApi.Entities;
using LogItUpApi.Models;
using LogItUpApi.Repositories;
using LogItUpApi.Shared.EmailSender;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LogItUpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : MainController
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountsController(
            UserManager<ApplicationUser> userManager,
            ILogger<CategoryTypesController> logger,
            IDataAccess dataAccess,
            IConfiguration configuration,
            IMapper mapper,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager) : base(userManager, logger, dataAccess, configuration, mapper)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserInfoDTO model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");

                var roles = await _userManager.GetRolesAsync(user);

                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                SendConfirmAccountEmail(user, code);

                return BuildToken(model, roles);
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfoDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(model.Email);

                if (!usuario.EmailConfirmed)
                    return Unauthorized("Must confirm email address");

                var roles = await _userManager.GetRolesAsync(usuario);

                return BuildToken(model, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                SendForgotPasswordEmail(user, resetToken);
            }

            return Ok();

        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ResetPasswordAsync(user, model.ResetPasswordToken, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }


        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        { 
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }

        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                await _userManager.ConfirmEmailAsync(user, model.Token);
                return Ok();
            }
            else
            {
                return BadRequest("Username or token invalid");
            }

        }

        [HttpPost("SendConfirmEmailLink")]
        public async Task<ActionResult> SendConfirmEmailLink([FromBody] SendConfirmEmailLinkDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                SendConfirmAccountEmail(user, code);
                return Ok();
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("AddUserRol")]
        public async Task<ActionResult> AddUserRol(EditUserRolDTO editUserRolDTO)
        {
            var usuario = await _userManager.FindByIdAsync(editUserRolDTO.UserId);
            //await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editUserRolDTO.RoleName));
            await _userManager.AddToRoleAsync(usuario, editUserRolDTO.RoleName); //JWT
            return Ok();
        }

        [HttpPost("RemoveUserRol")]
        public async Task<ActionResult> RemoveUserRol(EditUserRolDTO editUserRolDTO)
        {
            var usuario = await _userManager.FindByIdAsync(editUserRolDTO.UserId);
            //await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editUserRolDTO.RoleName));
            await _userManager.RemoveFromRoleAsync(usuario, editUserRolDTO.RoleName); //JWT
            return Ok();
        }

        private UserTokenDTO BuildToken(UserInfoDTO userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
        //new Claim("miValor", "Lo que yo quiera"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserTokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private void SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            try
            {
                EmailConfig emailConfig = new EmailConfig()
                {
                    SenderAddress = _configuration["EmailConfig:SenderAddress"],
                    SenderPassword = _configuration["EmailConfig:SenderPassword"],
                    SmtpPort = Convert.ToInt32(_configuration["EmailConfig:SmtpPort"]),
                    SmtpServer = _configuration["EmailConfig:SmtpServer"]
                };

                EmailInfo emailInfo = new EmailInfo()
                {
                    SenderAddress = emailConfig.SenderAddress,
                    ReceiverAddress = user.Email,
                    Subject = "Recuperar contraseña",
                    Body = token
                };

                _emailSender.SendEmail(emailConfig, emailInfo);
            }
            catch (Exception)
            {

            }

        }

        private void SendConfirmAccountEmail(ApplicationUser user, string token)
        {
            try
            {
                EmailConfig emailConfig = new EmailConfig()
                {
                    SenderAddress = _configuration["EmailConfig:SenderAddress"],
                    SenderPassword = _configuration["EmailConfig:SenderPassword"],
                    SmtpPort = Convert.ToInt32(_configuration["EmailConfig:SmtpPort"]),
                    SmtpServer = _configuration["EmailConfig:SmtpServer"]
                };

                var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { UserId = user.Id, Token = token });

                callbackUrl = _configuration["Parametrization:SiteUrl"] + callbackUrl;

                EmailInfo emailInfo = new EmailInfo()
                {
                    SenderAddress = emailConfig.SenderAddress,
                    ReceiverAddress = user.Email,
                    Subject = "Confirmar cuenta",
                    Body = "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>"
                };

                _emailSender.SendEmail(emailConfig, emailInfo);
            }
            catch (Exception)
            {

            }


        }
    }
}