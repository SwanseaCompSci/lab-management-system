using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.TimeAvailabilitySpecifications
{
    // TODO: Add code comments
    public sealed class GetTimeAvailabilityForUserWithinTimePeriodSpecification : Specification<TimeAvailability>
    {
        public GetTimeAvailabilityForUserWithinTimePeriodSpecification(Guid userId,
                                                                       WorkDayOfWeek day,
                                                                       TimeOnly startTime,
                                                                       TimeOnly endTime)
        {
            Query.Where(x => x.UserId.Equals(userId) && x.Day.Equals(day));
            Query.Where(x => startTime <= x.StartTime && x.EndTime <= endTime);
        }
    }
}
