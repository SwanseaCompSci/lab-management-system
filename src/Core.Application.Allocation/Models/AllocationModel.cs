namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models
{
    // TODO: Add docs comments
    public sealed class AllocationModel
    {
        public AllocationModel(Guid userId, Guid moduleId, Guid labId)
        {
            UserId = userId;
            ModuleId = moduleId;
            LabId = labId;
        }

        public Guid UserId { get; }
        public Guid ModuleId { get; }
        public Guid LabId { get; }
    }
}
