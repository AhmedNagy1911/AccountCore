using AccountCore.Application.Common.Results;
using AccountCore.Application.Contracts.Roles;

namespace AccountCore.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> GetAsync(string id);
}
