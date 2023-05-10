using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabScheduleEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Repositories
{
    // TODO: Add docs comments

    public sealed class LabScheduleRepository : ILabScheduleRepository
    {
        public LabScheduleRepository(IApplicationDbContext dbContext,
                                     ILogger<ILabScheduleRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<ILabScheduleRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<LabSchedule> AddItemAsync(LabSchedule item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(LabSchedule)));

            var entity = (await DbContext.LabSchedules.AddAsync(item, cancellationToken)).Entity;

            entity.DomainEvents.Add(new LabScheduleCreatedDomainEvent(labSchedule: entity));

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(LabSchedule), entity.Id.ToString()));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LabSchedule>> AddRangeAsync(IEnumerable<LabSchedule> items, CancellationToken cancellationToken)
        {
            LinkedList<LabSchedule> output = new();

            Logger.LogDebug(RepositoryLogMessages.GetAddingEntitiesLogMessage(nameof(LabSchedule)));

            foreach (var item in items)
            {
                var entity = (await DbContext.LabSchedules.AddAsync(item, cancellationToken)).Entity;
                entity.DomainEvents.Add(new LabScheduleCreatedDomainEvent(labSchedule: entity));
                output.AddLast(entity);

                Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(LabSchedule), entity.Id.ToString()));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return output;
        }

        /// <inheritdoc/>
        public async Task<LabSchedule> UpdateItemAsync(Guid id, LabSchedule item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(LabSchedule), id.ToString()));

            var labSchedule = await GetItemAsync(id, cancellationToken);
            if (labSchedule is null)
            {
                throw new EntityNotFoundException(nameof(LabSchedule), id);
            }

            labSchedule.Start = item.Start;
            labSchedule.End = item.End;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(LabSchedule), id.ToString()));

            return labSchedule;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LabSchedule>> UpdateRangeAsync(IEnumerable<LabSchedule> items, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntitiesLogMessage(nameof(LabSchedule)));

            var output = new LinkedList<LabSchedule>();

            foreach (var item in items)
            {
                var labSchedule = await GetItemAsync(id: item.Id, cancellationToken: cancellationToken);
                if (labSchedule is null)
                {
                    throw new EntityNotFoundException(nameof(LabSchedule), item.Id);
                }

                labSchedule.Start = item.Start;
                labSchedule.End = item.End;

                Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(LabSchedule), labSchedule.Id.ToString()));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return output;
        }

        /// <inheritdoc/>
        public async Task<LabSchedule?> DeleteItemAsync(Guid id, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(LabSchedule), id.ToString()));

            var labSchedule = await GetItemAsync(id, cancellationToken);
            if (labSchedule is null)
            {
                return null;
            }

            var result = DbContext.LabSchedules.Remove(labSchedule).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(LabSchedule), id.ToString()));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LabSchedule>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(LabSchedule)));

            var labSchedules = DbContext.LabSchedules.ToList();
            DbContext.LabSchedules.RemoveRange(labSchedules);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(LabSchedule)));

            return labSchedules;
        }

        /// <inheritdoc/>
        public Task<LabSchedule?> GetItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbContext.LabSchedules.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<LabSchedule?> GetItemAsync(ISingleResultSpecification<LabSchedule> specification, CancellationToken cancellationToken)
        {
            return DbContext.LabSchedules.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<LabSchedule> GetItems(ISpecification<LabSchedule> specification)
        {
            return DbContext.LabSchedules.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<LabSchedule> specification, CancellationToken cancellationToken)
        {
            return DbContext.LabSchedules.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
