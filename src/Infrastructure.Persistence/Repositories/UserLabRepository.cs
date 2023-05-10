using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Repositories
{
    // TODO: Add docs comments

    public class UserLabRepository : IUserLabRepository
    {
        public UserLabRepository(IApplicationDbContext dbContext,
                                 ILogger<IUserLabRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IUserLabRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<UserLab> AddItemAsync(UserLab item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(UserLab)));

            if (await DbContext.UserLabs.AnyAsync(x => x.UserId == item.UserId && x.LabId == item.LabId, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(UserLab), $"{item.UserId}, {item.LabId}");
            }

            var result = await DbContext.UserLabs.AddAsync(item, cancellationToken);

            item.DomainEvents.Add(new UserAddedToLabDomainEvent(userId: item.UserId,
                                                                labId: item.LabId));

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(UserLab), $"{result.Entity.UserId}, {result.Entity.LabId}"));

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<UserLab?> DeleteItemAsync(Guid userId, Guid labId, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(UserLab), $"{userId}, {labId}"));

            var userLab = await GetItemAsync(userId, labId, cancellationToken);
            if (userLab is null)
            {
                return null;
            }

            var result = DbContext.UserLabs.Remove(userLab).Entity;

            result.DomainEvents.Add(new UserRemovedFromLabDomainEvent(userId: result.UserId,
                                                                      labId: result.LabId));

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(UserLab), $"{userId}, {labId}"));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLab>> DeleteRangeAsync(IEnumerable<UserLab> items, CancellationToken cancellationToken)
        {
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var item in items)
                {
                    Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(UserLab), $"{item.UserId}, {item.LabId}"));
                }
            }

            DbContext.UserLabs.RemoveRange(items);

            foreach (var item in items)
            {
                item.DomainEvents.Add(new UserRemovedFromLabDomainEvent(userId: item.UserId,
                                                                        labId: item.LabId));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            if (Logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var item in items)
                {
                    Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(UserLab), $"{item.UserId}, {item.LabId}"));
                }
            }

            return items;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLab>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(UserLab)));

            var userLabs = DbContext.UserLabs.ToList();
            DbContext.UserLabs.RemoveRange(userLabs);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(UserLab)));

            return userLabs;
        }

        /// <inheritdoc/>
        public Task<UserLab?> GetItemAsync(Guid userId, Guid labId, CancellationToken cancellationToken)
        {
            return DbContext.UserLabs.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.LabId.Equals(labId), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<UserLab?> GetItemAsync(ISingleResultSpecification<UserLab> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserLabs.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<UserLab> GetItems(ISpecification<UserLab> specification)
        {
            return DbContext.UserLabs.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<UserLab> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserLabs.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
