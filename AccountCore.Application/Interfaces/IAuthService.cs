using AccountCore.Application.Common.Results;
using AccountCore.Application.Contracts.Authentication;


namespace AccountCore.Application.Interfaces;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
