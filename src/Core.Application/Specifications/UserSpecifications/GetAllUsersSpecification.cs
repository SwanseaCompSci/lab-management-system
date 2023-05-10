using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    /// <summary>
    /// Query logic to retrieve all <see cref="User"/> entities from the database.
    /// </summary>
    public sealed class GetAllUsersSpecification : Specification<User> { }
}
