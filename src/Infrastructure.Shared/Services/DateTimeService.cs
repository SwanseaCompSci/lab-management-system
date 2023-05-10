using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Shared.Services
{
    /// <summary>
    /// A service that provides <see cref="DateTime"/> information.
    /// </summary>
    public sealed class DateTimeService : IDateTimeService
    {
        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <inheritdoc/>
        public DateTime Today => DateTime.Today;
    }
}
