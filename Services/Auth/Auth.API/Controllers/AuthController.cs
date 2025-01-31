using Auth.API.Infrastructure.Repositories;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Auth.API.DTOs;
using Google.Apis.Auth;
using Auth.API.Models;
using Auth.API.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using Auth.API.Services;

namespace Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthRepo repo, IConfiguration configuration, ITokenService tokenService, ILogger<AuthController> logger, IAuthService authService) : ControllerBase
    {
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleAuthDto request)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = configuration["Auth:Google:ClientId"],
                        ClientSecret = configuration["Auth:Google:ClientSecret"]
                    },
                    Scopes = new[]
                    {
                        "openid",
                        "profile",
                        "email"
                    }
                });
                var redirectUri = configuration["Auth:Google:RedirectUri"];
                var tokenResponse = await flow.ExchangeCodeForTokenAsync(
                        userId: "user",
                        code: request.Code,
                        redirectUri: configuration["Auth:Google:RedirectUri"],
                        CancellationToken.None
                    );

                var validPayload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken);
                logger.LogInformation(JsonConvert.SerializeObject(validPayload));
                var user = await repo.FindUserByEmail(validPayload.Email);
                if(user is null)
                {
                    user = new AuthUser
                    {
                        Email = validPayload.Email,
                        Name = validPayload.Name
                    };
                    var result = await authService.CreateUserAsync(user, validPayload.GivenName, validPayload.FamilyName);
                    if (result.IsFailure) return BadRequest(result.Error);
                }

                var accessToken = tokenService.GenerateAccessToken(user.Id);
                var refreshToken = await tokenService.GenerateRefreshToken(user.Id);
                user.RefreshToken = refreshToken;
                await repo.SaveAsync();

                return Ok(new
                {
                    CurrentUser = new 
                    {
                        Id = user.Id,
                        UserName = user.Name,
                        Email = user.Email,
                    },
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error during Google authentication");
                return BadRequest("Invalid Google Token");
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await repo.DeleteRefreshToken(Guid.Parse(userId));
            await repo.SaveAsync();
            return NoContent();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null) return Unauthorized();
            var result = await authService.FindByToken(request.RefreshToken);
            if(result.IsFailure) {
                return BadRequest(result.Error);
            }
            var accessToken = tokenService.GenerateAccessToken(Guid.Parse(userId));
            var refreshToken = await tokenService.GenerateRefreshToken(Guid.Parse(userId));
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }
    }
}
