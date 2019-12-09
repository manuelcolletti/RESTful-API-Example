using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LogItUpApi.Entities;
using LogItUpApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogItUpApi.Controllers
{
    public class MainController : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<CategoryTypesController> _logger;
        protected readonly IDataAccess _dataAccess;
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
               
        public MainController(
            UserManager<ApplicationUser> userManager,
            ILogger<CategoryTypesController> logger,
            IDataAccess dataAccess,
            IConfiguration configuration,
            IMapper mapper)
        {

            _userManager = userManager;
            _logger = logger;
            _dataAccess = dataAccess;
            _configuration = configuration;
            _mapper = mapper;
        }
        
        [NonAction]
        public Task<ApplicationUser> GetUser()
        {
            string userEmail = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            return _userManager.FindByEmailAsync(userEmail);
        }
    }
}