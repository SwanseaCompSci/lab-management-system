using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="UserLab"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface IUserLabRepository : ICompositeKeyRepository<UserLab>
    {
        /// <summary>
        /// Gets item by a composite key.
        /// </summary>
        /// <param name="userId">An Id of the user.</param>
        /// <param name="labId">An Id of the lab.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The item with the composite key.</returns>
        Task<UserLab?> GetItemAsync(Guid userId, Guid labId, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="userId">An Id of the user.</param>
        /// <param name="labId">An Id of the lab.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted item. <see langword="null"/> if no item was deleted.</returns>
        Task<UserLab?> DeleteItemAsync(Guid userId, Guid labId, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes items.
        /// </summary>
        /// <param name="items">The items to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted items.</returns>
        Task<IEnumerable<UserLab>> DeleteRangeAsync(IEnumerable<UserLab> items, CancellationToken cancellationToken);
    }
}
