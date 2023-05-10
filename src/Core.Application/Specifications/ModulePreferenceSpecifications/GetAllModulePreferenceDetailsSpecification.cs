using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModulePreferenceSpecifications
{
    /// <summary>
    /// Query logic to retrieve all <see cref="ModulePreference"/> entities from the database.
    /// </summary>
    public sealed class GetAllModulePreferenceDetailsSpecification : Specification<ModulePreference>
    {
        public GetAllModulePreferenceDetailsSpecification()
        {
            Query.Include(x => x.User);
            Query.Include(x => x.Module);
        }
    }
}
