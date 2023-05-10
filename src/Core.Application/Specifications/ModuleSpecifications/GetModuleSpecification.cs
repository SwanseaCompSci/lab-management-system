using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetModuleSpecification : Specification<Module>, ISingleResultSpecification<Module>
    {
        public GetModuleSpecification(Guid moduleId)
        {
            Query.Where(x => x.Id.Equals(moduleId));
            Query.Take(1);
        }
    }
}
