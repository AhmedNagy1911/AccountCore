namespace AccountCore.Application.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);