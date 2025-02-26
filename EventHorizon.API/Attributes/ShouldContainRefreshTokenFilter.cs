using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventHorizon.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShouldContainRefreshTokenAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Cookies
                .TryGetValue("refreshToken", out var refreshToken);

            if (refreshToken == null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
