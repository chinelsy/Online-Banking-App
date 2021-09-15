using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebUI.domain.Middlewares
{

   
        public static class ClaimsPrincipleExtensions
        {
            public static string GetUsername(this ClaimsPrincipal user)
            {
                return user.FindFirst(ClaimTypes.Name)?.Value;
            }

            public static string GetUserId(this ClaimsPrincipal user)
            {
                return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
            {
                return user.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
            }
        }
    
}