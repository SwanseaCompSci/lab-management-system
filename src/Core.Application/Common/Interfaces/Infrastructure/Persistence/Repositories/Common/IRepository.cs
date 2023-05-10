using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common
{
    /// <summary>
    /// Defines a common contract for all repositories.
    /// </summary>
    /// <remarks>
    /// To learn more about the Repository pattern, visit <see href="https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff649690(v=pandp.10)"/>.
    /// </remarks>
    /// <typeparam name="T">The specific type of <see cref="BaseEntity"/> that the repository works with.</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets item that matches a given specification.
        /// </summary>
        /// <param name="specification">The specification against which the item should be matched.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The matched item.</returns>
        Task<T?> GetItemAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken);

        /// <summary>
        /// Gets items that match a given specification.
        /// </summary>
        /// <param name="specification">The specification against which the items should be matched.</param>
        /// <returns>The matched items.</returns>
        IEnumerable<T> GetItems(ISpecification<T> specification);

        /// <summary>
        /// Gets the count of items that match a given specification.
        /// </summary>
        /// <param name="specification">The specification against which the items should be matched.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The count of the matched items.</returns>
        Task<int> GetItemsCountAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes all items from the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted items.</returns>
        Task<IEnumerable<T>> DeleteAllAsync(CancellationToken cancellationToken);
    }
}
