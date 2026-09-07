namespace AccountCore.Application.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);

