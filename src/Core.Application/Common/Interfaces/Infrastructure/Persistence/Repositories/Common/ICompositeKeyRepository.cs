using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common
{
    /// <summary>
    /// Defines a common contract for repositories of entities with a composite key.
    /// </summary>
    /// <typeparam name="T">The specific type of <see cref="BaseEntity"/> that the repository works with.</typeparam>
    public interface ICompositeKeyRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added item.</returns>
        /// <exception cref="DuplicateEntityException">Composite primary key already exists in the database.</exception>
        Task<T> AddItemAsync(T item, CancellationToken cancellationToken);
    }
}
