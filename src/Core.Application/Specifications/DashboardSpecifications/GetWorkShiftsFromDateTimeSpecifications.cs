using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.DashboardSpecifications
{
    // TODO: Add docs comments
    public sealed class GetWorkShiftsFromDateTimeSpecifications : Specification<UserLabSchedule>
    {
        public GetWorkShiftsFromDateTimeSpecifications(Guid userId, DateTime dateTime)
        {
            Query.Where(x => x.UserId.Equals(userId));
            Query.Include(x => x.LabSchedule)
                .ThenInclude(x => x.Lab)
                .ThenInclude(x => x.Module)
                .Where(x => x.LabSchedule.End >= dateTime);
        }
    }
}
