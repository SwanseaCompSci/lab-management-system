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

    public sealed class ModuleRepository : IModuleRepository
    {
        public ModuleRepository(IApplicationDbContext dbContext,
                                ILogger<IModuleRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IModuleRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<Module> AddItemAsync(Module item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(Module)));

            var result = await DbContext.Modules.AddAsync(item, cancellationToken);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(Module), result.Entity.Id.ToString()));

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<Module> UpdateItemAsync(Guid id, Module item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(Module), id.ToString()));

            var module = await GetItemAsync(id, cancellationToken);
            if (module is null)
            {
                throw new EntityNotFoundException(nameof(Module), id);
            }

            module.Name = item.Name;
            module.Code = item.Code;
            module.Level = item.Level;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(Module), id.ToString()));

            return module;
        }

        /// <inheritdoc/>
        public async Task<Module?> DeleteItemAsync(Guid id, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(Module), id.ToString()));

            var module = await GetItemAsync(id, cancellationToken);
            if (module is null)
            {
                return null;
            }

            var result = DbContext.Modules.Remove(module).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(Module), id.ToString()));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Module>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(Module)));

            var modules = DbContext.Modules.ToList();
            DbContext.Modules.RemoveRange(modules);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(Module)));

            return modules;
        }

        /// <inheritdoc/>
        public Task<Module?> GetItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbContext.Modules.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Module?> GetItemAsync(ISingleResultSpecification<Module> specification, CancellationToken cancellationToken)
        {
            return DbContext.Modules.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<Module> GetItems(ISpecification<Module> specification)
        {
            return DbContext.Modules.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<Module> specification, CancellationToken cancellationToken)
        {
            return DbContext.Modules.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
