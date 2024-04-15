using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Api.Controllers
{
    public class Auth
    {
        public static int? GetUserId(HttpContext httpContext)
        {
            var userIdString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null)
            {
                return null;
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return null;
            }

            return userId;
        }
    }
}