namespace MusicTrack.Api.Auth;

public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(string username);
}
