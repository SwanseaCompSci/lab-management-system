using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.AllocationSpecifications
{
    /// <summary>
    /// Query logic to retrieve all <see cref="Lab"/> allocation details from the database.
    /// </summary>
    public sealed class GetAllAllocationResultsSpecification : Specification<Lab>
    {
        public GetAllAllocationResultsSpecification()
        {
            Query.Include(x => x.Module);
            Query.Include(x => x.UserLabs).ThenInclude(x => x.User);
        }
    }
}
