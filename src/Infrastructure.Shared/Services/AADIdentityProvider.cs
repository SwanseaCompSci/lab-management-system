using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Shared.Services
{
    // TODO: Add docs comments
    public partial class AADIdentityProvider : IIdentityProvider
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly string _clientId;

        public AADIdentityProvider(string tenantId, string clientId, string clientSecret)
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

            _graphServiceClient = new GraphServiceClient(clientSecretCredential);
            _clientId = clientId;
        }

        /// <inheritdoc/>
        public async Task AddToRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            var resourceId = (await GetServicePrincipal(cancellationToken)).Id;

            var appRoleAssignment = new AppRoleAssignment
            {
                PrincipalId = userId,
                ResourceId = Guid.Parse(resourceId),
                AppRoleId = roleId,
            };

            _ = await _graphServiceClient
                .Users[userId.ToString()]
                .AppRoleAssignments
                .PostAsync(appRoleAssignment, config => { }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveFromRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            var appRolesAssignments = await _graphServiceClient
                .Users[userId.ToString()]
                .AppRoleAssignments
                .GetAsync(config => { }, cancellationToken);

            var appRoleAssignmentId = appRolesAssignments.Value[0].Id;

            await _graphServiceClient
                .Users[userId.ToString()]
                .AppRoleAssignments[appRoleAssignmentId]
                .DeleteAsync(config => { }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Guid> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            var servicePrincipal = await GetServicePrincipal(cancellationToken);

            return servicePrincipal.AppRoles.FirstOrDefault(x => x.Value == role.ToString())?.Id
                ?? throw new NullReferenceException($"Application Role ({role}) not found.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserModel>> SearchUsersAsync(string searchExpression, CancellationToken cancellationToken = default)
        {
            var aadUsers = await _graphServiceClient.Users
                .GetAsync(config =>
                {
                    config.Headers.Add("ConsistencyLevel", "eventual");
                    config.QueryParameters.Filter = $"startswith(displayName,'{searchExpression}') or " +
                                                    $"startswith(givenName,'{searchExpression}') or " +
                                                    $"startswith(surname,'{searchExpression}') or " +
                                                    $"startswith(mail,'{searchExpression}')";
                    config.QueryParameters.Select = new string[] { "id", "givenName", "surname", "mail" };
                }, cancellationToken);

            return ConvertToUserModels(aadUsers.Value);
        }

        /// <inheritdoc/>
        public async Task<Guid?> GetUserRoleIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var appRolesAssignments = await _graphServiceClient
                .Users[userId.ToString()]
                .AppRoleAssignments
                .GetAsync(config => { }, cancellationToken);

            return appRolesAssignments.Value.Count == 0
                ? null
                : appRolesAssignments.Value[0].AppRoleId;
        }
    }

    public partial class AADIdentityProvider
    {
        private static UserModel ConvertToUserModel(User aadUser)
        {
            return new UserModel()
            {
                Id = Guid.Parse(aadUser.Id),
                FirstName = aadUser.GivenName,
                Surname = aadUser.Surname,
                AchievedLevel = null!,
                MaxWeeklyWorkHours = 0,
            };
        }

        private static IEnumerable<UserModel> ConvertToUserModels(IEnumerable<User> aadUsers)
        {
            return aadUsers.Select(x => ConvertToUserModel(x));
        }

        private async Task<ServicePrincipal> GetServicePrincipal(CancellationToken cancellationToken)
        {
            var servicePrincipals = await _graphServiceClient.ServicePrincipals
                .GetAsync(config => {
                    config.QueryParameters.Filter = $"AppId eq '{_clientId}'";
                }, cancellationToken);
            return servicePrincipals.Value[0];
        }
    }
}
