using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services
{
    public interface IIdentityProvider
    {
        Task AddToRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
        Task RemoveFromRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
        Task<Guid> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken = default);
        Task<Guid?> GetUserRoleIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserModel>> SearchUsersAsync(string searchExpression, CancellationToken cancellationToken = default);
    }
}
