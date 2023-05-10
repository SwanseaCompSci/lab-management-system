using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabScheduleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserLabSchedulesWhereLabFromDateTimeSpecification : Specification<UserLabSchedule>
    {
        public GetUserLabSchedulesWhereLabFromDateTimeSpecification(Guid userId, Guid labId, DateTime dateTime)
        {
            Query.Where(x => x.UserId.Equals(userId));
            Query.Include(x => x.LabSchedule);
            Query.Where(x => x.LabSchedule.LabId.Equals(labId));
            Query.Where(x => x.LabSchedule.Start >= dateTime);
        }
    }
}
