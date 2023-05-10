namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Common
{
    /// <summary>
    /// Defines properties for auditable entities.
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        // TODO: Add docs comments

        public DateTime UtcCreated { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? UtcUpdated { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
