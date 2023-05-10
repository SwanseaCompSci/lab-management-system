using Microsoft.EntityFrameworkCore;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence
{
    /// <summary>
    /// Defines a contract for a <see cref="DbContext"/> of the application.
    /// </summary>
    /// <remarks>
    /// The implementation of this interface is in the Infrastructure.Persistence layer.
    /// </remarks>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Provides access to <see cref="Module"/> entities in the database.
        /// </summary>
        public DbSet<Module> Modules { get; }
        /// <summary>
        /// Provides access to <see cref="ModulePreference"/> entities in the database.
        /// </summary>
        public DbSet<ModulePreference> ModulePreferences { get; }
        /// <summary>
        /// Provides access to <see cref="Lab"/> entities in the database.
        /// </summary>
        public DbSet<Lab> Labs { get; }
        /// <summary>
        /// Provides access to <see cref="LabSchedule"/> entities in the database.
        /// </summary>
        public DbSet<LabSchedule> LabSchedules { get; }

        /// <summary>
        /// Provides access to <see cref="UserModule"/> entities in the database.
        /// </summary>
        public DbSet<UserModule> UserModules { get; }
        /// <summary>
        /// Provides access to <see cref="UserLab"/> entities in the database.
        /// </summary>
        public DbSet<UserLab> UserLabs { get; }
        /// <summary>
        /// Provides access to <see cref="UserLabSchedule"/> entities in the database.
        /// </summary>
        public DbSet<UserLabSchedule> UserLabSchedules { get; }

        /// <summary>
        /// Provides access to <see cref="User"/> entities in the database.
        /// </summary>
        public DbSet<User> Users { get; }

        /// <summary>
        /// Provides access to <see cref="TimeAvailability"/> entities in the database.
        /// </summary>
        public DbSet<TimeAvailability> TimeAvailabilities { get; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Applies any pending migrations for the context to the database. Will create the database if it does not already exist.
        /// </summary>
        void Migrate();
    }
}
