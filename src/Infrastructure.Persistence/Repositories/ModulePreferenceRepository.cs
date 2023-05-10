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
    public sealed class ModulePreferenceRepository : IModulePreferenceRepository
    {
        public ModulePreferenceRepository(IApplicationDbContext dbContext,
                                          ILogger<IModulePreferenceRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IModulePreferenceRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<ModulePreference> AddItemAsync(ModulePreference item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(ModulePreference)));

            if (await DbContext.ModulePreferences.AnyAsync(x => x.UserId == item.UserId && x.ModuleId == item.ModuleId, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(item), $"{item.UserId}, {item.ModuleId}");
            }

            var entity = (await DbContext.ModulePreferences.AddAsync(item, cancellationToken)).Entity;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(ModulePreference), $"{entity.UserId}, {entity.ModuleId}"));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ModulePreference>> AddRangeAsync(IEnumerable<ModulePreference> items, CancellationToken cancellationToken)
        {
            LinkedList<ModulePreference> output = new();

            Logger.LogDebug(RepositoryLogMessages.GetAddingEntitiesLogMessage(nameof(ModulePreference)));

            foreach (var item in items)
            {
                if (await DbContext.ModulePreferences.AnyAsync(x => x.UserId == item.UserId && x.ModuleId == item.ModuleId, cancellationToken))
                {
                    throw new DuplicateEntityException(nameof(item), $"{item.UserId}, {item.ModuleId}");
                }

                var entity = (await DbContext.ModulePreferences.AddAsync(item, cancellationToken)).Entity;
                output.AddLast(entity);

                Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(ModulePreference), $"{item.UserId}, {item.ModuleId}"));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return output;
        }

        /// <inheritdoc/>
        public async Task<ModulePreference> UpdateItemAsync(Guid userId, Guid moduleId, ModulePreference item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(ModulePreference), $"{userId}, {moduleId}"));

            var entity = await GetItemAsync(userId, moduleId, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException(nameof(ModulePreference), $"{userId}, {moduleId}");
            }

            entity.Status = item.Status;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(ModulePreference), $"{userId}, {moduleId}"));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ModulePreference>> DeleteRangeByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntitiesLogMessage(nameof(ModulePreference), $"UserId={userId}"));

            var entities = DbContext.ModulePreferences.Where(x => x.UserId.Equals(userId)).ToList();
            DbContext.ModulePreferences.RemoveRange(entities);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedEntitiesLogMessage(nameof(ModulePreference), $"UserId={userId}"));

            return entities;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ModulePreference>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(ModulePreference)));

            var modulePreferences = DbContext.ModulePreferences.ToList();
            DbContext.ModulePreferences.RemoveRange(modulePreferences);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(ModulePreference)));

            return modulePreferences;
        }

        /// <inheritdoc/>
        public Task<ModulePreference?> GetItemAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken)
        {
            return DbContext.ModulePreferences.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.ModuleId.Equals(moduleId), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<ModulePreference?> GetItemAsync(ISingleResultSpecification<ModulePreference> specification, CancellationToken cancellationToken)
        {
            return DbContext.ModulePreferences.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<ModulePreference> GetItems(ISpecification<ModulePreference> specification)
        {
            return DbContext.ModulePreferences.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<ModulePreference> specification, CancellationToken cancellationToken)
        {
            return DbContext.ModulePreferences.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
