using Microsoft.EntityFrameworkCore;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext instance represents a session with the database and can be used to query and save instances of your entities.
    /// <see cref="ApplicationDbContext"/> is a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        /// <summary>
        /// The unique identifier of the system user.
        /// </summary>
        private readonly Guid SystemUser = Guid.Empty;

        /// <summary>
        /// The service that provides access to information about the current user.
        /// </summary>
        private readonly ICurrentUserService _currentUserService;
        /// <summary>
        /// The service that provides the current <see cref="DateTime"/> information.
        /// </summary>
        private readonly IDateTimeService _dateTimeService;
        /// <summary>
        /// The service that provides functionality for dispatching <see cref="DomainEvent"/>s.
        /// </summary>
        private readonly IDomainEventService _domainEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class using the specified options and services.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="currentUserService">A service that provides access to information about the current user.</param>
        /// <param name="dateTimeService">A service that provides the current <see cref="DateTime"/> information.</param>
        /// <param name="domainEventService">A service that provides functionality for dispatching <see cref="DomainEvent"/>s.</param>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            IDomainEventService domainEventService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
            _domainEventService = domainEventService;
        }

        /// <inheritdoc/>
        public DbSet<Module> Modules { get; private set; } = null!;
        /// <inheritdoc/>
        public DbSet<Lab> Labs { get; private set; } = null!;
        /// <inheritdoc/>
        public DbSet<LabSchedule> LabSchedules { get; private set; } = null!;

        /// <inheritdoc/>
        public DbSet<ModulePreference> ModulePreferences { get; private set; } = null!;
        /// <inheritdoc/>
        public DbSet<UserModule> UserModules { get; private set; } = null!;
        /// <inheritdoc/>
        public DbSet<UserLab> UserLabs { get; private set; } = null!;
        /// <inheritdoc/>
        public DbSet<UserLabSchedule> UserLabSchedules { get; private set; } = null!;

        /// <inheritdoc/>
        public DbSet<User> Users { get; private set; } = null!;

        /// <inheritdoc/>
        public DbSet<TimeAvailability> TimeAvailabilities { get; private set; } = null!;

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        public void Migrate() => base.Database.Migrate();

        /// <inheritdoc/>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? SystemUser;
                        entry.Entity.UtcCreated = _dateTimeService.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId ?? SystemUser;
                        entry.Entity.UtcUpdated = _dateTimeService.UtcNow;
                        break;
                }
            }

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents(events);

            return result;
        }

        /// <summary>
        /// Configures database entities models.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Dispatches <see cref="DomainEvent"/>s.
        /// </summary>
        /// <param name="events">The events to dispatch.</param>
        /// <returns>A task that represents an asynchronous dispatch events operation.</returns>
        private async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }
    }
}
