using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="ModulePreference"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface IModulePreferenceRepository : ICompositeKeyRepository<ModulePreference>
    {
        /// <summary>
        /// Gets item by a composite key.
        /// </summary>
        /// <param name="userId">An Id of the user.</param>
        /// <param name="moduleId">An Id of the module.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The item with the composite key.</returns>
        Task<ModulePreference?> GetItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken);

        /// <summary>
        /// Adds items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added items.</returns>
        /// <exception cref="DuplicateEntityException">Composite primary key already exists in the database.</exception>
        public Task<IEnumerable<ModulePreference>> AddRangeAsync(IEnumerable<ModulePreference> items, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="moduleId">The Id of the module.</param>
        /// <param name="item">The item with new values.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated item.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        Task<ModulePreference> UpdateItemAsync(Guid userId, Guid moduleId, ModulePreference item, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes items related to user.
        /// </summary>
        /// <param name="userId">An Id of the related <see cref="User"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted items.</returns>
        public Task<IEnumerable<ModulePreference>> DeleteRangeByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
