using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the entity of type <see cref="TimeAvailability"/>.
    /// </summary>
    internal sealed class ConfigurationTimeAvailability : IEntityTypeConfiguration<TimeAvailability>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TimeAvailability> builder)
        {
            builder.ToTable("TimeAvailabilities", x => x.IsTemporal());

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany(x => x.TimeAvailabilities)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
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

            builder.Property(x => x.IsAllocated)
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
