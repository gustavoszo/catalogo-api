using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
using CatalogoApi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager; 
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, JwtService jwtService, IMapper mapper, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username);
            if (userExists != null) return BadRequest(new { Message = $"Username '{userRegisterDto.Username}' já está cadastrado" });

            var result = await _userManager.CreateAsync(_mapper.Map<User>(userRegisterDto), userRegisterDto.Password);

            if (result.Errors.Any())
            {
                return BadRequest(new { Message = result.Errors.Select(e => e.Description) });
            }
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var token = _jwtService.GetToken(loginDto.Username);
                return Ok ( new { AccessToken = token } );
            }
            return Unauthorized(new { Message = "Credenciais inválidas" } );
        }

        /*
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {

            string? accessToken = tokenModel.AccessToken
                                  ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                                   ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return BadRequest("AcessToken/RefreshToken inválido(s)");
            }

            string username = principal.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(username);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            // Obtenha o usuário autenticado
            var authenticatedUser = await _userManager.GetUserAsync(User); // User do ControllerBase

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            // Verifique se o usuário autenticado é o mesmo que está tentando revogar o token
            bool isSelfRevoking = authenticatedUser.UserName == username;

            // Verifique se o usuário autenticado é um Super Admin
            bool isSuperAdmin = await _userManager.IsInRoleAsync(authenticatedUser, "SuperAdmin");

            // Permita a revogação apenas se o usuário estiver revogando seu próprio token ou se for um Super Admin
            if (!isSelfRevoking && !isSuperAdmin)
            {
                return Forbid();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("Invalid user name");
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
        */

    }
}
