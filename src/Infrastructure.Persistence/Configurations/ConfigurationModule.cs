using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="Module"/>.
    /// </summary>
    internal sealed class ConfigurationModule : IEntityTypeConfiguration<Module>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules", x => x.IsTemporal());

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Code)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Level)
                .HasMaxLength(10)
                .HasConversion(new ValueConverter<Level, string>(
                    v => v.ToString(),
                    v => Enum.Parse<Level>(v)))
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
