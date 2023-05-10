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
    public sealed class UserRepository : IUserRepository
    {
        public UserRepository(IApplicationDbContext dbContext,
                              ILogger<IUserRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        private IApplicationDbContext DbContext { get; }
        private ILogger<IUserRepository> Logger { get; }

        /// <inheritdoc/>
        /// <exception cref="DuplicateEntityException"><see cref="User"/> with the specified Id already exists in the database.</exception>
        public async Task<User> AddItemAsync(User item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetAddingEntityLogMessage(nameof(User)));

            if (await DbContext.Users.AnyAsync(x => x.Id.Equals(item.Id)))
            {
                throw new DuplicateEntityException(nameof(User), item.Id);
            }

            var result = await DbContext.Users.AddAsync(item, cancellationToken);
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityAddedLogMessage(nameof(User), result.Entity.Id.ToString()));

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<User> UpdateItemAsync(Guid id, User item, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntityLogMessage(nameof(User), id.ToString()));

            var user = await GetItemAsync(id, cancellationToken);
            if (user is null)
            {
                throw new EntityNotFoundException(nameof(User), id);
            }

            user.FirstName = item.FirstName;
            user.Surname = item.Surname;
            user.AchievedLevel = item.AchievedLevel;
            user.MaxWeeklyWorkHours = item.MaxWeeklyWorkHours;
            user.QuestionnaireToken = item.QuestionnaireToken;

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(User), id.ToString()));

            return user;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> UpdateRangeAsync(IEnumerable<User> items, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetUpdatingEntitiesLogMessage(nameof(User)));

            var output = new LinkedList<User>();

            foreach (var item in items)
            {
                var user = await GetItemAsync(id: item.Id, cancellationToken: cancellationToken);
                if (user is null)
                {
                    throw new EntityNotFoundException(nameof(User), item.Id.ToString());
                }

                user.FirstName = item.FirstName;
                user.Surname = item.Surname;
                user.AchievedLevel = item.AchievedLevel;
                user.MaxWeeklyWorkHours = item.MaxWeeklyWorkHours;
                user.QuestionnaireToken = item.QuestionnaireToken;

                output.AddLast(user);

                Logger.LogDebug(RepositoryLogMessages.GetEntityUpdatedLogMessage(nameof(User), user.Id.ToString()));
            }

            _ = await DbContext.SaveChangesAsync(cancellationToken);
            
            return output;
        }

        /// <inheritdoc/>
        public async Task<User?> DeleteItemAsync(Guid id, CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingEntityLogMessage(nameof(User), id.ToString()));

            var user = await GetItemAsync(id, cancellationToken);
            if (user is null)
            {
                return null;
            }

            var result = DbContext.Users.Remove(user).Entity;
            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetEntityDeletedLogMessage(nameof(User), id.ToString()));

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> DeleteAllAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug(RepositoryLogMessages.GetDeletingAllEntitiesLogMessage(nameof(User)));

            var users = DbContext.Users.ToList();
            DbContext.Users.RemoveRange(users);

            _ = await DbContext.SaveChangesAsync(cancellationToken);

            Logger.LogDebug(RepositoryLogMessages.GetDeletedAllEntitiesLogMessage(nameof(User)));

            return users;
        }

        /// <inheritdoc/>
        public Task<User?> GetItemAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<User?> GetItemAsync(ISingleResultSpecification<User> specification, CancellationToken cancellationToken)
        {
            return DbContext.Users.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<User> GetItems(ISpecification<User> specification)
        {
            return DbContext.Users.WithSpecification(specification).ToList();
        }

        /// <inheritdoc/>
        public Task<int> GetItemsCountAsync(ISpecification<User> specification, CancellationToken cancellationToken)
        {
            return DbContext.Users.WithSpecification(specification).CountAsync(cancellationToken);
        }
    }
}
