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
    public sealed class UserLabScheduleRepository : IUserLabScheduleRepository
    {
        public UserLabScheduleRepository(IApplicationDbContext dbContext,
                                         ILogger<IUserLabScheduleRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IUserLabScheduleRepository> Logger { get; }

        /// <inheritdoc/>
        public async Task<UserLabSchedule> AddItemAsync(UserLabSchedule item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(UserLabSchedule)));

            if (await DbContext.UserLabSchedules.AnyAsync(x => x.UserId == item.UserId && x.LabScheduleId == item.LabScheduleId, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(UserLabSchedule), $"{item.UserId}, {item.LabScheduleId}");
            }

            var entity = (await DbContext.UserLabSchedules.AddAsync(item, cancellationToken)).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(UserLabSchedule), $"{entity.UserId}, {entity.LabScheduleId}"));

            return entity;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLabSchedule>> AddRangeAsync(IEnumerable<UserLabSchedule> items, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntitiesLogMessage(nameof(UserLabSchedule)));

            var output = new LinkedList<UserLabSchedule>();

            foreach (var item in items)
            {
                if (await DbContext.UserLabSchedules.AnyAsync(x => x.UserId == item.UserId && x.LabScheduleId == item.LabScheduleId, cancellationToken))
                {
                    throw new DuplicateEntityException(nameof(UserLabSchedule), $"{item.UserId}, {item.LabScheduleId}");
                }

                var entity = (await DbContext.UserLabSchedules.AddAsync(item, cancellationToken)).Entity;
                output.AddLast(entity);
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            if (Logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var item in output)
                {
                    Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(UserLabSchedule), $"{item.UserId}, {item.LabScheduleId}"));
                }
            }

            return output;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLabSchedule>> DeleteRangeAsync(IEnumerable<UserLabSchedule> items, CancellationToken cancellationToken)
        {
            DbContext.UserLabSchedules.RemoveRange(items);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            return items;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLabSchedule>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(UserLabSchedule)));

            var userLabSchedules = DbContext.UserLabSchedules.ToList();
            DbContext.UserLabSchedules.RemoveRange(userLabSchedules);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(UserLabSchedule)));

            return userLabSchedules;
        }

        /// <inheritdoc/>
        public Task<UserLabSchedule?> GetItemAsync(ISingleResultSpecification<UserLabSchedule> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserLabSchedules.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<UserLabSchedule> GetItems(ISpecification<UserLabSchedule> specification)
        {
            return DbContext.UserLabSchedules.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<UserLabSchedule> specification, CancellationToken cancellationToken)
        {
            return DbContext.UserLabSchedules.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
