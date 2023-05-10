using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Utils.DatabaseMigrationUtility
{
    internal class CurrentUserService : ICurrentUserService
    {
        public Guid? UserId => null!;

        public ClaimsPrincipal User => null!;

        public string? UserName => null!;
        public string? UserFirstName => null!;
        public string? UserSurname => null!;

        public ApplicationRole? UserApplicationRole => null!;

        public ModuleRole? UserModuleRole(Guid userId, Guid moduleId) => null!;
    }
    internal class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
    }
    internal class DomainEventService : IDomainEventService
    {
        public Task Publish(DomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
