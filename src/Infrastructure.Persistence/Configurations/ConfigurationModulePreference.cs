using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="ModulePreference"/>.
    /// </summary>
    internal sealed class ConfigurationModulePreference : IEntityTypeConfiguration<ModulePreference>
    {
        public void Configure(EntityTypeBuilder<ModulePreference> builder)
        {
            builder.ToTable("ModulePreferences", x => x.IsTemporal());

            builder.HasKey(x => new { x.UserId, x.ModuleId });

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany(x => x.ModulePreferences)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.ModuleId).IsRequired();
            builder.HasOne(x => x.Module)
                .WithMany(x => x.ModulePreferences)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(20)
                .HasConversion(new ValueConverter<Status, string>(
                    v => v.ToString(),
                    v => Enum.Parse<Status>(v)))
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
