using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserModuleEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Repositories
{
    // TODO: Add docs comments

    public sealed class UserModuleRepository : IUserModuleRepository
    {
        public UserModuleRepository(IApplicationDbContext dbContext,
                                    ILogger<IUserModuleRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IUserModuleRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<UserModule> AddItemAsync(UserModule item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(UserModule)));

            if (await DbContext.UserModules.AnyAsync(x => x.UserId == item.UserId && x.ModuleId == item.ModuleId, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(item), $"{item.UserId}, {item.ModuleId}");
            }

            var entity = (await DbContext.UserModules.AddAsync(item, cancellationToken)).Entity;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(UserModule), $"{entity.UserId}, {entity.ModuleId}"));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<UserModule> UpdateItemAsync(Guid userId, Guid moduleId, UserModule item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(UserModule), $"{userId}, {moduleId}"));

            var userModule = await GetItemAsync(userId, moduleId, cancellationToken);
            if (userModule is null)
            {
                throw new EntityNotFoundException(nameof(UserModule), $"{userId}, {moduleId}");
            }

            userModule.Role = item.Role;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(UserModule), $"{userId}, {moduleId}"));

            return userModule;
        }

        /// <inheritdoc/>
        public async Task<UserModule?> DeleteItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(UserModule), $"{userId}, {moduleId}"));

            var userModule = await GetItemAsync(userId, moduleId, cancellationToken);
            if (userModule is null)
            {
                return null;
            }

            var result = DbContext.UserModules.Remove(userModule).Entity;

            result.DomainEvents.Add(new UserRemovedFromModuleDomainEvent(userId: result.UserId,
                                                                         moduleId: result.ModuleId));

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(UserModule), $"{userId}, {moduleId}"));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserModule>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(UserModule)));

            var userModules = DbContext.UserModules.ToList();
            DbContext.UserModules.RemoveRange(userModules);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(UserModule)));

            return userModules;
        }

        /// <inheritdoc/>
        public Task<UserModule?> GetItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken)
        {
            return DbContext.UserModules.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.ModuleId.Equals(moduleId), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<UserModule?> GetItemAsync(ISingleResultSpecification<UserModule> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserModules.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<UserModule> GetItems(ISpecification<UserModule> specification)
        {
            return DbContext.UserModules.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<UserModule> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserModules.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
