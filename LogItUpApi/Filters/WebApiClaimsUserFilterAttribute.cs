using Microsoft.AspNetCore.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace LogItUpApi.Filters
{
    public class WebApiClaimsUserFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // access to the HttpContextBase instance can be done using the Properties collection MS_HttpContext
            var context = (HttpContext)actionContext.Request.Properties["MS_HttpContext"];
            var user = new WebUserInfo(context);
            actionContext.ActionArguments["claimsUser"] = user; // key name here must match the parameter name in the methods you want to populate with this instance
            base.OnActionExecuting(actionContext);
        }
    }
}
