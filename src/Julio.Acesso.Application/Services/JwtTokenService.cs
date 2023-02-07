using Julio.Acesso.App.Models;
using Julio.Acesso.App.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Julio.Acesso.Application
{
    public class JwtTokenService : ITokenService
    {
        public const string SECRET = "p05e7_lk24uWyE+PDTqX5lkADXUo0y&9Mma@BFq$W";

        public string Gerar(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            byte[]? key = Encoding.ASCII.GetBytes(SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login),
                    new Claim(ClaimTypes.Role, usuario.Grupo)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken? token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public static string JwtAuthenticationScheme => JwtBearerDefaults.AuthenticationScheme;
    }
}