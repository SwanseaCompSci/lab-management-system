using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Repositories
{
    // TODO: Add docs comments

    public sealed class LabRepository : ILabRepository
    {
        public LabRepository(IApplicationDbContext dbContext,
                             ILogger<ILabRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<ILabRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<Lab> AddItemAsync(Lab item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(Lab)));

            var result = await DbContext.Labs.AddAsync(item, cancellationToken);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(Lab), result.Entity.Id.ToString()));

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<Lab> UpdateItemAsync(Guid id, Lab item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(Lab), id.ToString()));

            var lab = await GetItemAsync(id, cancellationToken);
            if (lab is null)
            {
                throw new EntityNotFoundException(nameof(Lab), id);
            }

            var oldLab = (Lab)lab.Clone();
            var newLab = lab;

            lab.Name = item.Name;
            lab.Day = item.Day;
            lab.StartTime = item.StartTime;
            lab.EndTime = item.EndTime;
            lab.MinNumberOfStaff = item.MinNumberOfStaff;
            lab.MaxNumberOfStaff = item.MaxNumberOfStaff;

            lab.DomainEvents.Add(new LabUpdatedDomainEvent(oldLab: oldLab,
                                                           newLab: newLab));

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(Lab), id.ToString()));

            return lab;
        }

        /// <inheritdoc/>
        public async Task<Lab?> DeleteItemAsync(Guid id, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(Lab), id.ToString()));

            var lab = await GetItemAsync(id, cancellationToken);
            if (lab is null)
            {
                return null;
            }

            var result = DbContext.Labs.Remove(lab).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(Lab), id.ToString()));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lab>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(Lab)));

            var labs = DbContext.Labs.ToList();
            DbContext.Labs.RemoveRange(labs);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(Lab)));

            return labs;
        }

        /// <inheritdoc/>
        public Task<Lab?> GetItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbContext.Labs.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Lab?> GetItemAsync(ISingleResultSpecification<Lab> specification, CancellationToken cancellationToken)
        {
            return DbContext.Labs.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<Lab> GetItems(ISpecification<Lab> specification)
        {
            return DbContext.Labs.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<Lab> specification, CancellationToken cancellationToken)
        {
            return DbContext.Labs.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
