using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="UserLabSchedule"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface IUserLabScheduleRepository : ICompositeKeyRepository<UserLabSchedule>
    {
        /// <summary>
        /// Adds items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added items.</returns>
        /// <exception cref="DuplicateEntityException">Composite primary key already exists in the database.</exception>
        Task<IEnumerable<UserLabSchedule>> AddRangeAsync(IEnumerable<UserLabSchedule> items, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes items.
        /// </summary>
        /// <param name="items">The items to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted items.</returns>
        Task<IEnumerable<UserLabSchedule>> DeleteRangeAsync(IEnumerable<UserLabSchedule> items, CancellationToken cancellationToken);
    }
}
