namespace AccountCore.Application.Contracts.Authentication;

public record RegisterRequest(
   string Email,
   string Password,
   string PhoneNumber,
   string UserName,
   string FirstName,
   string LastName
);