//using GildedRose.Core.Models;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Linq;
//using System.Security.Claims;

//namespace GildedRose.Api.Filters
//{
//    public class UserContextActionFilter : IActionFilter
//    {
//        private UserContext userContext;

//        public UserContextActionFilter(UserContext userContext)
//        {
//            this.userContext = userContext;
//        }

//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            var isUserPresent = context.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier);
//            if (isUserPresent)
//            {
//                var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
//                this.userContext.Id = int.Parse(userId);
//            }
//        }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//        }
//    }
//}
