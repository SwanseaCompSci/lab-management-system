using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.TimeAvailabilitySpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.UserLabEvents
{
    // TODO: Add docs comments
    public sealed class UserAddedToLabDomainEventHandler : INotificationHandler<DomainEventNotification<UserAddedToLabDomainEvent>>
    {
        public UserAddedToLabDomainEventHandler(IDateTimeService dateTimeService,
                                                ILabRepository labRepository,
                                                ILabScheduleRepository labScheduleRepository,
                                                IUserLabScheduleRepository userLabScheduleRepository,
                                                ITimeAvailabilityRepository timeAvailabilityRepository)
        {
            DateTimeService = dateTimeService;
            LabRepository = labRepository;
            LabScheduleRepository = labScheduleRepository;
            UserLabScheduleRepository = userLabScheduleRepository;
            TimeAvailabilityRepository = timeAvailabilityRepository;
        }

        private IDateTimeService DateTimeService { get; }
        private ILabRepository LabRepository { get; }
        private ILabScheduleRepository LabScheduleRepository { get; }
        private IUserLabScheduleRepository UserLabScheduleRepository { get; }
        private ITimeAvailabilityRepository TimeAvailabilityRepository { get; }

        public async Task Handle(DomainEventNotification<UserAddedToLabDomainEvent> notification, CancellationToken cancellationToken)
        {
            // Get future lab schedules for lab
            var labScheduleSpecification = new GetLabSchedulesWhereLabFromDateTimeSpecification(labId: notification.Event.LabId,
                                                                                                dateTime: DateTimeService.UtcNow);
            var labSchedules = LabScheduleRepository.GetItems(specification: labScheduleSpecification);

            // Instantiate UserLabSchedules to add
            var userLabSchedules = new LinkedList<UserLabSchedule>();
            foreach (var labSchedule in labSchedules)
            {
                userLabSchedules.AddLast(new UserLabSchedule(userId: notification.Event.UserId,
                                                             labScheduleId: labSchedule.Id));
            }

            // Add UserLabSchedules
            await UserLabScheduleRepository.AddRangeAsync(items: userLabSchedules,
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
                item.IsAllocated = true;
            }

            await TimeAvailabilityRepository.UpdateRangeAsync(items: timeAvailabilities, cancellationToken: cancellationToken);
        }
    }
}
