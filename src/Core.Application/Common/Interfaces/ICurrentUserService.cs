using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Defines a contract for a service that provides information about the current user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Returns ClaimsPrincipal of the current user.
        /// </summary>
        ClaimsPrincipal User { get; }

        /// <summary>
        /// Gets an Id of the current user.
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Returns a name of the current user.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Returns a first name of the current user.
        /// </summary>
        string? UserFirstName { get; }

        /// <summary>
        /// Returns a surname of the current user.
        /// </summary>
        string? UserSurname { get; }

        /// <summary>
        /// Returns the User's role from ID token.
        /// </summary>
        ApplicationRole? UserApplicationRole { get; }
    }
}
