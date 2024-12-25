using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogoApi.Security
{
    public class JwtService
    {

        private JwtUtil _jwtUtil { get; set; }

        public JwtService(JwtUtil jwtUtil) 
        {
            _jwtUtil = jwtUtil;
        }

        public string GetToken(string username)
        {
            return _jwtUtil.GenerateToken(username);
        }

    }
}
