namespace AccountCore.Application.Contracts.Authentication;

public record ResendConfirmationEmailRequest(
    string Email
);