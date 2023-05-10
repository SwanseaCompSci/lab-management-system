namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services
{
    /// <summary>
    /// Defines a contract for a service that provides <see cref="DateTime"/> information.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }
        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <returns>
        /// An object that is set to today's date, with the time component set to 00:00:00.
        /// </returns>
        DateTime Today { get; }
    }
}
