using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="UserModule"/>.
    /// </summary>
    internal sealed class ConfigurationUserModule : IEntityTypeConfiguration<UserModule>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserModule> builder)
        {
            builder.ToTable("UserModules", x => x.IsTemporal());

            builder.HasKey(x => new { x.UserId, x.ModuleId });

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserModules)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.ModuleId).IsRequired();
            builder.HasOne(x => x.Module)
                .WithMany(x => x.UserModules)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.Role)
                .HasMaxLength(20)
                .HasConversion(new ValueConverter<ModuleRole, string>(
                    v => v.ToString(),
                    v => Enum.Parse<ModuleRole>(v)))
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
