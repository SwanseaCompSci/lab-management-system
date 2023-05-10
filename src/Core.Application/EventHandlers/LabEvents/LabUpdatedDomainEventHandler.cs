using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.LabEvents
{
    // TODO: Add docs comments
    public sealed class LabUpdatedDomainEventHandler : INotificationHandler<DomainEventNotification<LabUpdatedDomainEvent>>
    {
        public LabUpdatedDomainEventHandler(IDateTimeService dateTimeService,
                                            ILabScheduleRepository labScheduleRepository)
        {
            DateTimeService = dateTimeService;
            LabScheduleRepository = labScheduleRepository;
        }

        private IDateTimeService DateTimeService { get; }
        private ILabScheduleRepository LabScheduleRepository { get; }

        public async Task Handle(DomainEventNotification<LabUpdatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var oldLab = notification.Event.OldLab;
            var newLab = notification.Event.NewLab;

            if (oldLab.Day != newLab.Day
                || oldLab.StartTime != newLab.StartTime
                || oldLab.EndTime != newLab.EndTime)
            {
                // Update future LabSchedules

                var specification = new GetLabSchedulesWhereLabFromDateTimeSpecification(labId: oldLab.Id,
                                                                                         dateTime: DateTimeService.UtcNow);
                var labSchedules = LabScheduleRepository.GetItems(specification: specification);

                // How many days forwards/backwards 
                var daysChange = (int)newLab.Day - (int)oldLab.Day;

                foreach (var item in labSchedules)
                {
                    item.Start = item.Start.AddDays(daysChange);
                    item.Start = new DateTime(year: item.Start.Year,
                                              month: item.Start.Month,
                                              day: item.Start.Day).Add(newLab.StartTime.ToTimeSpan());
                    item.End = new DateTime(year: item.Start.Year,
                                            month: item.Start.Month,
                                            day: item.Start.Day).Add(newLab.EndTime.ToTimeSpan());
                }

                _ = await LabScheduleRepository.UpdateRangeAsync(items: labSchedules,
                                                                 cancellationToken: cancellationToken);
            }
        }
    }
}
