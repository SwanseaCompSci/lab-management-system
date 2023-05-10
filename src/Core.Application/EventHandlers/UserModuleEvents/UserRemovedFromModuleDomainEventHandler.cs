using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserModuleEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.UserModuleEvents
{
    // TODO: Add docs comments
    public sealed class UserRemovedFromModuleDomainEventHandler : INotificationHandler<DomainEventNotification<UserRemovedFromModuleDomainEvent>>
    {
        public UserRemovedFromModuleDomainEventHandler(IUserLabRepository userLabRepository)
        {
            UserLabRepository = userLabRepository;
        }

        private IUserLabRepository UserLabRepository { get; }

        public async Task Handle(DomainEventNotification<UserRemovedFromModuleDomainEvent> notification, CancellationToken cancellationToken)
        {
            var specification = new GetUserLabsWhereModuleSpecification(userId: notification.Event.UserId,
                                                                        moduleId: notification.Event.ModuleId);

            var userLabs = UserLabRepository.GetItems(specification: specification);

            _ = await UserLabRepository.DeleteRangeAsync(items: userLabs,
                                                         cancellationToken: cancellationToken);
        }
    }
}
