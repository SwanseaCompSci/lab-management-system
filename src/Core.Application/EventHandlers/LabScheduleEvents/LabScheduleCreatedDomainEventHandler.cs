using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabScheduleEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.LabScheduleEvents
{
    // TODO: Add docs comments
    public class LabScheduleCreatedDomainEventHandler : INotificationHandler<DomainEventNotification<LabScheduleCreatedDomainEvent>>
    {
        public LabScheduleCreatedDomainEventHandler(IUserLabRepository userLabRepository,
                                                    IUserLabScheduleRepository userLabScheduleRepository)
        {
            UserLabRepository = userLabRepository;
            UserLabScheduleRepository = userLabScheduleRepository;
        }

        private IUserLabRepository UserLabRepository { get; }
        private IUserLabScheduleRepository UserLabScheduleRepository { get; }

        public async Task Handle(DomainEventNotification<LabScheduleCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var userLabSpecifications = new GetUserLabsWhereLabSpecification(labId: notification.Event.LabSchedule.LabId);
            var userLabs = UserLabRepository.GetItems(specification: userLabSpecifications);

            var userLabSchedules = new LinkedList<UserLabSchedule>();
            foreach (var item in userLabs)
            {
                userLabSchedules.AddLast(new UserLabSchedule(userId: item.UserId,
                                                             labScheduleId: notification.Event.LabSchedule.Id));
            }

            await UserLabScheduleRepository.AddRangeAsync(items: userLabSchedules,
                                                          cancellationToken: cancellationToken);
        }
    }
}
