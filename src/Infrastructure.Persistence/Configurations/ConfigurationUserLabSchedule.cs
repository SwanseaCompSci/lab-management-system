using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="UserLabSchedule"/>.
    /// </summary>
    internal sealed class ConfigurationUserLabSchedule : IEntityTypeConfiguration<UserLabSchedule>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserLabSchedule> builder)
        {
            builder.ToTable("UserLabSchedules", x => x.IsTemporal());

            builder.HasKey(x => new { x.UserId, x.LabScheduleId });

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserLabSchedules)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.LabScheduleId).IsRequired();
            builder.HasOne(x => x.LabSchedule)
                .WithMany(x => x.UserLabSchedules)
                .HasForeignKey(x => x.LabScheduleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

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
