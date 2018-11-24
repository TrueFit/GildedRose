//using GildedRose.Membership.Models;
//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Linq;
//using System.IdentityModel.Tokens.Jwt;
//using GildedRose.Core.Models;

//namespace GildedRose.Api.Middleware
//{
//    public class UserContextMiddleware
//    {
//        private readonly RequestDelegate next;

//        public UserContextMiddleware(RequestDelegate next)
//        {
//            this.next = next;
//        }

//        public Task Invoke(HttpContext context, UserContext userContext)
//        {
//            var isUserPresent = context.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier);
//            if (!isUserPresent)
//            {
//                return this.next(context);
//            }

//            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

//            userContext.Id = int.Parse(userId);

//            // Call the next delegate/middleware in the pipeline
//            return this.next(context);
//        }
//    }
//}
