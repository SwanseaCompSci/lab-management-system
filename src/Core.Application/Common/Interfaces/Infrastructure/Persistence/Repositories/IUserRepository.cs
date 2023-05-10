using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="User"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface IUserRepository : ISingleKeyRepository<User>
    {
        /// <summary>
        /// Updates items.
        /// </summary>
        /// <param name="items">The items to update.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated items.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        Task<IEnumerable<User>> UpdateRangeAsync(IEnumerable<User> items, CancellationToken cancellationToken);
    }
}
