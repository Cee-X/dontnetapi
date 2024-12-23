using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace NZWalks.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, string[] roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: Credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}