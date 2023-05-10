using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="Lab"/>.
    /// </summary>
    internal sealed class ConfigurationLab : IEntityTypeConfiguration<Lab>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Lab> builder)
        {
            builder.ToTable("Labs", x => x.IsTemporal());

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.ModuleId).IsRequired();
            builder.HasOne(x => x.Module)
                .WithMany(x => x.Labs)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Day)
                .HasMaxLength(10)
                .HasConversion(new ValueConverter<WorkDayOfWeek, string>(
                    v => v.ToString(),
                    v => Enum.Parse<WorkDayOfWeek>(v)))
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasConversion(new ValueConverter<TimeOnly, DateTime>(
                    v => new DateTime(1, 1, 1, v.Hour, v.Minute, v.Second),
                    v => TimeOnly.FromDateTime(v)))
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasConversion(new ValueConverter<TimeOnly, DateTime>(
                    v => new DateTime(1, 1, 1, v.Hour, v.Minute, v.Second),
                    v => TimeOnly.FromDateTime(v)))
                .IsRequired();

            builder.Property(x => x.MinNumberOfStaff)
                .IsRequired();

            builder.Property(x => x.MaxNumberOfStaff)
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
