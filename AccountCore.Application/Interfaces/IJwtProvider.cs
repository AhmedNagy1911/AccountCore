using AccountCore.Domain.Entities;

namespace AccountCore.Application.Interfaces;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    string? ValidateToken(string token);
}
