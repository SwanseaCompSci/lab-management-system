using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="LabSchedule"/>.
    /// </summary>
    internal sealed class ConfigurationLabSchedule : IEntityTypeConfiguration<LabSchedule>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<LabSchedule> builder)
        {
            builder.ToTable("LabSchedules", x => x.IsTemporal());

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.LabId).IsRequired();
            builder.HasOne(x => x.Lab)
                .WithMany(x => x.LabSchedules)
                .HasForeignKey(x => x.LabId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.Start).IsRequired();
            builder.Property(x => x.End).IsRequired();

            #region AuditableEntity
            builder.Property(x => x.UtcCreated)
                .IsRequired();
            builder.Property(x => x.CreatedBy)
                .IsRequired(false);

            builder.Property(x => x.UtcUpdated)
                .IsRequired(false);
            builder.Property(x => x.UpdatedBy)
                .IsRequired(false);
            #endregion

            #region IHasDomainEvent
            builder.Ignore(x => x.DomainEvents);
            #endregion
        }
    }
}
