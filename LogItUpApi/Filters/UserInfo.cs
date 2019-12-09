using System;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace LogItUpApi.Filters
{
    public interface IUserInfo
    {
        int RoleId { get; }
        int UserId { get; }
        bool IsAuthenticated { get; }
    }

    public class WebUserInfo : IUserInfo
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool IsAuthenticated { get; set; }

        public WebUserInfo(HttpContext httpContext)
        {
            try
            {
                var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
                IsAuthenticated = httpContext.User.Identity.IsAuthenticated;
                if (claimsIdentity != null)
                {
                    RoleId = Int32.Parse(claimsIdentity.FindFirst("RoleId").Value);
                    UserId = 0;// Int32.Parse(claimsIdentity.GetUserId());
                }
            }
            catch (Exception)
            {
                IsAuthenticated = false;
                UserId = -1;
                RoleId = -1;

                // log exception
            }

        }
    }
}
