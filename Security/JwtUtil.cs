using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CatalogoApi.Security
{
    public class JwtUtil
    {
        private readonly IConfiguration _configuration; // O IConfiguration é a interface que permite acessar as configurações do arquivo appsettings.json.
        private readonly string _secret_key;

        public JwtUtil(IConfiguration configuration)
        {
            _configuration = configuration;
            _secret_key = _configuration["JWT:SecretKey"];
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret_key));
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = GetSymmetricSecurityKey();
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateToken(List<Claim> claims)
        {
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JWT:ValidIssuer"],  
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:TokenValidityInMinutes"])),
                claims: claims,
                signingCredentials: GetSigningCredentials()
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        /*
        public string GenerateRefreshToken(string username)
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
        */

        /*
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                       out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                             !jwtSecurityToken.Header.Alg.Equals(
                             SecurityAlgorithms.HmacSha256,
                             StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        */

    }
}
