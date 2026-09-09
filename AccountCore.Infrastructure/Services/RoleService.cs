using AccountCore.Application.Common.Errors;
using AccountCore.Application.Common.Results;
using AccountCore.Application.Contracts.Roles;
using AccountCore.Application.Interfaces;
using AccountCore.Domain.Entities;
using AccountCore.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountCore.Infrastructure.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(
        bool includeDisabled = false,
        CancellationToken cancellationToken = default) =>
        await _roleManager.Roles
            .Where(x => !x.IsDefault && (!x.IsDeleted || includeDisabled))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Result<RoleDetailResponse>> GetAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

        var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted);

        return Result.Success(response);
    }

}
