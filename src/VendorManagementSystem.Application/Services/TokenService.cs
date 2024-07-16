using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettingsDto _jwtSettings;
        public TokenService(IOptions<JwtSettingsDto> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public TokenDto JwtToken(User user, string type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Key!);
            Claim[] claims;
            DateTime? expires;
            if(string.Equals(type, "resetpassword", StringComparison.OrdinalIgnoreCase) || string.Equals(type,"newuser", StringComparison.OrdinalIgnoreCase))
            {
                claims = new Claim[]
                {
                    new(ClaimTypes.Email, user.Email),
                };
                expires = DateTime.UtcNow.AddDays(1);
            }
            else
            {
                claims =
                [
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, user.Role),
                    new(ClaimTypes.Name, user.UserName.Replace("$", string.Empty)),
                ];

                if (string.Equals(type, "login", StringComparison.OrdinalIgnoreCase))
                {
                    expires = DateTime.UtcNow.AddHours(8);
                }
                else if (string.Equals(type.ToLower(), "rememberme", StringComparison.OrdinalIgnoreCase))
                { 
                    expires = DateTime.UtcNow.AddDays(14);
                }
                else
                {
                    expires= DateTime.UtcNow.AddHours(1);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDto { Token = tokenHandler.WriteToken(token) };
        }
        public string ExtractUserDetials(string jwtToken, string type)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwtToken);

            var claims = token.Claims;
            if (string.Equals(type.ToLower(), "id", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("going to return id");
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid");
                Console.WriteLine(userClaim);
                return userClaim?.Value ?? string.Empty;
            }
            if (string.Equals(type.ToLower(), "email", StringComparison.OrdinalIgnoreCase))
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");
                return userClaim?.Value ?? string.Empty;
            }
            if (string.Equals(type.ToLower(), "username", StringComparison.OrdinalIgnoreCase))
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name");
                return userClaim?.Value ?? string.Empty;
            }
            if (string.Equals(type.ToLower(), "role", StringComparison.OrdinalIgnoreCase))
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
                return userClaim?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        public bool ValidateToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var ValidateParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                tokenHandler.ValidateToken(jwtToken, ValidateParameters, out var ValidateToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
