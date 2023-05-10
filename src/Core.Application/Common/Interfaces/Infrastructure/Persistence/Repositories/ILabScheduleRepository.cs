using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Defines a contract for <see cref="LabSchedule"/> repository.
    /// </summary>
    /// <inheritdoc/>
    public interface ILabScheduleRepository : ISingleKeyRepository<LabSchedule>
    {
        /// <summary>
        /// Adds items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added items.</returns>
        public Task<IEnumerable<LabSchedule>> AddRangeAsync(IEnumerable<LabSchedule> items, CancellationToken cancellationToken);

        /// <summary>
        /// Updates items.
        /// </summary>
        /// <param name="items">The items to update.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The updated items.</returns>
        /// <exception cref="EntityNotFoundException">Entity with a primary key not found.</exception>
        public Task<IEnumerable<LabSchedule>> UpdateRangeAsync(IEnumerable<LabSchedule> items, CancellationToken cancellationToken);
    }
}
