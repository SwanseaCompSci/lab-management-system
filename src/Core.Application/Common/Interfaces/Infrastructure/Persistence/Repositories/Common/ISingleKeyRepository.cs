using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common
{
    /// <summary>
    /// Defines a common contract for repositories of entities with a single key.
    /// </summary>
    /// <typeparam name="T">The specific type of <see cref="BaseEntity"/> that the repository works with.</typeparam>
    public interface ISingleKeyRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added item.</returns>
        Task<T> AddItemAsync(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Gets item by Id.
        /// </summary>
        /// <param name="id">An Id of the item.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The item with the Id.</returns>
        Task<T?> GetItemAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="id">The Id of the item to update.</param>
        /// <param name="item">The item with new values.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated item.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        Task<T> UpdateItemAsync(Guid id, T item, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="id">The Id of the item to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted item. <see langword="null"/> if no item was deleted.</returns>
        Task<T?> DeleteItemAsync(Guid id, CancellationToken cancellationToken);
    }
}
