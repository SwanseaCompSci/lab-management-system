using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.TimeAvailabilitySpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.UserLabEvents
{
    // TODO: Add docs comments
    public sealed class UserRemovedFromLabDomainEventHandler : INotificationHandler<DomainEventNotification<UserRemovedFromLabDomainEvent>>
    {
        public UserRemovedFromLabDomainEventHandler(IDateTimeService dateTimeService,
                                                    ILabRepository labRepository,
                                                    ITimeAvailabilityRepository timeAvailabilityRepository,
                                                    IUserLabScheduleRepository userLabScheduleRepository)
        {
            DateTimeService = dateTimeService;
            LabRepository = labRepository;
            TimeAvailabilityRepository = timeAvailabilityRepository;
            UserLabScheduleRepository = userLabScheduleRepository;
        }

        private IDateTimeService DateTimeService { get; }
        private ILabRepository LabRepository { get; }
        private ITimeAvailabilityRepository TimeAvailabilityRepository { get; }
        private IUserLabScheduleRepository UserLabScheduleRepository { get; }

        public async Task Handle(DomainEventNotification<UserRemovedFromLabDomainEvent> notification, CancellationToken cancellationToken)
        {
            // Remove user from lab schedules of a given lab
            var specification = new GetUserLabSchedulesWhereLabFromDateTimeSpecification(userId: notification.Event.UserId,
                                                                                         labId: notification.Event.LabId,
                                                                                         dateTime: DateTimeService.UtcNow);

            var userLabSchedules = UserLabScheduleRepository.GetItems(specification: specification);

            _ = await UserLabScheduleRepository.DeleteRangeAsync(items: userLabSchedules,
                                                                 cancellationToken: cancellationToken);

            // Update user's time availability
            var lab = await LabRepository.GetItemAsync(id: notification.Event.LabId, cancellationToken: cancellationToken);
            var timeAvailabilitySpecification = new GetTimeAvailabilityForUserWithinTimePeriodSpecification(userId: notification.Event.UserId,
                                                                                                            day: lab!.Day,
                                                                                                            startTime: lab!.StartTime,
                                                                                                            endTime: lab!.EndTime);

            var timeAvailabilities = TimeAvailabilityRepository.GetItems(specification: timeAvailabilitySpecification);
            foreach (var item in timeAvailabilities)
            {
                item.IsAllocated = false;
            }

            await TimeAvailabilityRepository.UpdateRangeAsync(items: timeAvailabilities, cancellationToken: cancellationToken);
        }
    }
}
