using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="UserLab"/>.
    /// </summary>
    internal sealed class ConfigurationUserLab : IEntityTypeConfiguration<UserLab>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserLab> builder)
        {
            builder.ToTable("UserLabs", x => x.IsTemporal());

            builder.HasKey(x => new { x.UserId, x.LabId });

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserLabs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.LabId).IsRequired();
            builder.HasOne(x => x.Lab)
                .WithMany(x => x.UserLabs)
                .HasForeignKey(x => x.LabId)
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
