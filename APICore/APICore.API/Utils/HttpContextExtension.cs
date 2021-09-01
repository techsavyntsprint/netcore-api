using Microsoft.AspNetCore.Http;

namespace APICore.API.Utils
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// Get current user id from claim principal
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static int CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor?.HttpContext?.User?.GetUserIdFromToken();

            return id ?? 0;
        }
    }
}
