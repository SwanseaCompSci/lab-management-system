using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Repositories
{
    // TODO: Add docs comments

    public sealed class TimeAvailabilityRepository : ITimeAvailabilityRepository
    {
        public TimeAvailabilityRepository(IApplicationDbContext dbContext,
                                          ILogger<ITimeAvailabilityRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<ITimeAvailabilityRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<TimeAvailability> AddItemAsync(TimeAvailability item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(TimeAvailability)));

            var result = await DbContext.TimeAvailabilities.AddAsync(item, cancellationToken);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(TimeAvailability), result.Entity.Id.ToString()));

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TimeAvailability>> AddRangeAsync(IEnumerable<TimeAvailability> items, CancellationToken cancellationToken)
        {
            LinkedList<TimeAvailability> output = new();

            Logger.LogDebug(RepositoryLogMessages.GetAddingEntitiesLogMessage(nameof(TimeAvailability)));

            foreach (var item in items)
            {
                var entity = (await DbContext.TimeAvailabilities.AddAsync(item, cancellationToken)).Entity;
                output.AddLast(entity);

                Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(TimeAvailability), entity.Id.ToString()));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return output;
        }

        /// <inheritdoc/>
        public async Task<TimeAvailability> UpdateItemAsync(Guid id, TimeAvailability item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(TimeAvailability), id.ToString()));

            var timeAvailability = await GetItemAsync(id, cancellationToken);
            if (timeAvailability is null)
            {
                throw new EntityNotFoundException(nameof(TimeAvailability), id.ToString());
            }

            timeAvailability.IsAllocated = item.IsAllocated;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(TimeAvailability), id.ToString()));

            return timeAvailability;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TimeAvailability>> UpdateRangeAsync(IEnumerable<TimeAvailability> items, CancellationToken cancellationToken)
        {
            Logger.LogInformation(RepositoryLogMessages.GetUpdatingEntitiesLogMessage(nameof(TimeAvailability)));

            var output = new LinkedList<TimeAvailability>();

            foreach (var item in items)
            {
                var timeAvailability = await GetItemAsync(item.Id, cancellationToken);
                if (timeAvailability is null)
                {
                    throw new EntityNotFoundException(nameof(TimeAvailability), item.Id.ToString());
                }

                timeAvailability.IsAllocated = item.IsAllocated;

                output.AddLast(timeAvailability);

                Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(TimeAvailability), timeAvailability.Id.ToString()));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return output;
        }

        /// <inheritdoc/>
        public async Task<TimeAvailability?> DeleteItemAsync(Guid id, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(TimeAvailability), id.ToString()));

            var timeAvailability = await GetItemAsync(id, cancellationToken);
            if (timeAvailability is null)
            {
                return null;
            }

            var result = DbContext.TimeAvailabilities.Remove(timeAvailability).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(TimeAvailability), id.ToString()));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TimeAvailability>> DeleteRangeByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntitiesLogMessage(nameof(TimeAvailability), $"UserId={userId}"));

            var entities = DbContext.TimeAvailabilities.Where(x => x.UserId.Equals(userId)).ToList();
            DbContext.TimeAvailabilities.RemoveRange(entities);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedEntitiesLogMessage(nameof(TimeAvailability), $"UserId={userId}"));

            return entities;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TimeAvailability>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(TimeAvailability)));

            var timeAvailabilities = DbContext.TimeAvailabilities.ToList();
            DbContext.TimeAvailabilities.RemoveRange(timeAvailabilities);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(TimeAvailability)));

            return timeAvailabilities;
        }

        /// <inheritdoc/>
        public Task<TimeAvailability?> GetItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbContext.TimeAvailabilities.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TimeAvailability?> GetItemAsync(ISingleResultSpecification<TimeAvailability> specification, CancellationToken cancellationToken)
        {
            return DbContext.TimeAvailabilities.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<TimeAvailability> GetItems(ISpecification<TimeAvailability> specification)
        {
            return DbContext.TimeAvailabilities.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<TimeAvailability> specification, CancellationToken cancellationToken)
        {
            return DbContext.TimeAvailabilities.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
