using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="TimeAvailability"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface ITimeAvailabilityRepository : ISingleKeyRepository<TimeAvailability>
    {
        /// <summary>
        /// Adds items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added items.</returns>
        public Task<IEnumerable<TimeAvailability>> AddRangeAsync(IEnumerable<TimeAvailability> items, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes items related to user.
        /// </summary>
        /// <param name="userId">An Id of the related <see cref="User"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The deleted items.</returns>
        public Task<IEnumerable<TimeAvailability>> DeleteRangeByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates items.
        /// </summary>
        /// <param name="items">The items to update.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated items.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        public Task<IEnumerable<TimeAvailability>> UpdateRangeAsync(IEnumerable<TimeAvailability> items, CancellationToken cancellationToken);
    }
}
