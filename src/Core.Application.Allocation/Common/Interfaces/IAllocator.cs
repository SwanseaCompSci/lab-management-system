using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common.Interfaces
{
    public interface IAllocator
    {
        IEnumerable<AllocationModel> Allocate(IReadOnlyCollection<UserModel> users,
                                              IReadOnlyCollection<LabModel> labs,
                                              IReadOnlyCollection<AllocationModel> allocations);
    }
}
