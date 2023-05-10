using Microsoft.AspNetCore.Components.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Services
{
    /// <summary>
    /// A service that provides information about the current user.
    /// </summary>
    internal class CurrentUserService : ICurrentUserService
    {
        /// <summary>
        /// Creates a new instance of <see cref="CurrentUserService"/>.
        /// </summary>
        /// <param name="authenticationStateProvider">Provides information about the authentication state of the current user.</param>
        public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Provides information about the authentication state of the current user.
        /// </summary>
        private AuthenticationStateProvider AuthenticationStateProvider { get; }

        /// <inheritdoc/>
        public ClaimsPrincipal User => AuthenticationStateProvider.GetAuthenticationStateAsync().GetAwaiter().GetResult().User;
        /// <inheritdoc/>
        public Guid? UserId
        {
            get
            {
                return Helpers.GetUserId(user: User);
            }
        }
        /// <inheritdoc/>
        public string? UserName => Helpers.GetUserName(user: User);
        /// <inheritdoc/>
        public string? UserFirstName => Helpers.GetUserFirstName(user: User);
        /// <inheritdoc/>
        public string? UserSurname => Helpers.GetUserSurname(user: User);
        /// <inheritdoc/>
        public ApplicationRole? UserApplicationRole
        {
            get
            {
                return Helpers.GetApplicationRole(user: User);
            }
        }
    }
}
