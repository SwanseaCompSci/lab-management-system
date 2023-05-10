using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="UserModule"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface IUserModuleRepository : ICompositeKeyRepository<UserModule>
    {
        /// <summary>
        /// Gets item by a composite key.
        /// </summary>
        /// <param name="userId">An Id of the user.</param>
        /// <param name="moduleId">An Id of the module.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The item with the composite key.</returns>
        Task<UserModule?> GetItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="moduleId">The Id of the module.</param>
        /// <param name="item">The item with new values.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated item.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        Task<UserModule> UpdateItemAsync(Guid userId, Guid moduleId, UserModule item, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="moduleId">The Id of the module.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted item. <see langword="null"/> if no item was deleted.</returns>
        Task<UserModule?> DeleteItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken);
    }
}
