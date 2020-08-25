using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecureApiJWT.JWT
{
    public class JWTAuthManager : IJWTAuthManager
    {
        private readonly string Key;
        public JWTAuthManager(string Key)
        {
            this.Key = Key;
        }
        private readonly IDictionary<string, string> users = new Dictionary<string, string> 
        {
            {"user1","pass1" },
            {"user2","pass2" }
        };
        public string Auth(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password)) 
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Key);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
