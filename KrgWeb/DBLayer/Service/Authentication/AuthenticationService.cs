using DBLayer.Models;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;

namespace DBLayer.Service.Authentication
{
    public interface IAuthenticationService
    {
        public string GenerateToken(InvUser user);
        public Task<VMAuthResponse> ValidateUser(VMAuthReq user);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IConfiguration _config;
        public AuthenticationService(IConfiguration config, NsinvoiceBillingContext billingContext)
        {
            _config = config;
            _context = billingContext;
        }
        public async Task<VMAuthResponse> ValidateUser(VMAuthReq reqUser)
        {
            var response = new VMAuthResponse();
            var user = await _context.InvUsers.Where(I => I.Username.Trim() == reqUser.username.Trim()).FirstOrDefaultAsync();
            response.isAuthenticated = false;
            if (user != null)
            {
                if (HashHelper.ComputeSha512Hash(reqUser.password) == user.Password)
                {
                    response.isAuthenticated = true;
                    response.token = GenerateToken(user);
                }
            }
            return response;
        }
        public string GenerateToken(InvUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserCode.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("type", user.UserType.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
