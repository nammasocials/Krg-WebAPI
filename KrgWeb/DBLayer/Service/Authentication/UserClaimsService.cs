using DBLayer.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service.Authentication
{
    public interface IUserClaimsService
    {
        public InvUser GetUserClaims();
    }
    public class UserClaimsService : IUserClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public InvUser GetUserClaims()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            return new InvUser
            {
                UserCode = Guid.Parse(user.FindFirst("UserCode")?.Value),
                Username = user.FindFirst("UserName")?.Value,
                UserType = Convert.ToInt16(user.FindFirst("Usertype")?.Value)
            };
        }
    }
}
