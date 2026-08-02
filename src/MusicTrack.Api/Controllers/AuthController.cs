using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicTrack.Api.Auth;
using MusicTrack.Api.Models;

namespace MusicTrack.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly DemoUserSettings _demoUser;

    public AuthController(ITokenService tokenService, IOptions<DemoUserSettings> demoUser)
    {
        _tokenService = tokenService;
        _demoUser = demoUser.Value;
    }

    [HttpPost("token")]
    public ActionResult<LoginResponse> Token([FromBody] LoginRequest request)
    {
        if (!IsValidCredentials(request.Username, request.Password))
            return Unauthorized(new { Errors = new[] { "Invalid username or password" } });

        var (token, expiresAtUtc) = _tokenService.GenerateToken(request.Username);

        return Ok(new LoginResponse { AccessToken = token, ExpiresAtUtc = expiresAtUtc });
    }

    private bool IsValidCredentials(string username, string password)
    {
        if (!string.Equals(username, _demoUser.Username, StringComparison.OrdinalIgnoreCase))
            return false;

        var providedHash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var expectedHash = Convert.FromHexString(_demoUser.PasswordHash);

        return CryptographicOperations.FixedTimeEquals(providedHash, expectedHash);
    }
}
