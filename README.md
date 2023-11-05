# Lab Management System

This project is a Blazor web application for managing teaching assistants at Swansea University. The application allows for the allocation of teaching assistants to modules and labs based on their module preferences, time availability, as well as requirements of the modules.

The implementation follows several design patterns and best practices, discussed in a [dissertation](./docs/dissertation.pdf) available in the `/docs` folder.

## Prerequisite

- [Microsoft Azure](https://azure.microsoft.com/en-gb/)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
- [SQL Server](https://www.microsoft.com/en-GB/sql-server/sql-server-downloads)

## Development

### Registering the application in Azure Active Directory

1. Register a new application in your Azure Active Directory
   - Go to: _Azure Active Directory > App registrations > New registration_
   - Register the application with the following values:
      - Name: _Lab Manager - Development_
      - Supported account types: _Accounts in this organizational directory only (Northwind Traders only - Single tenant)_
1. Create a new client secret
   - Go to: _Certificates & secrets > New client secret_
   - Fill in the form and hit _Add_
1. Add API permissions
   - Go to: _API permissions > Add a permission_
   - Add the following Microsoft Graph Permissions:
      - AppRoleAssignment.ReadWrite.All
      - User.Read.All
   - Grant admin consent for Northwind Traders
1. Add application roles
   - Go to: _App roles > Create app role_
   - Create the following roles:  
      | Display name  | Allowed member types | Value         | Description        |
      |---------------|----------------------|---------------|--------------------|
      | Administrator | Users/Groups         | Administrator | Administrator role |
      | User          | Users/Groups         | User          | User role          |
1. Grant Administrator role to users
   - Go to: _Azure Active Directory > Enterprise applications > Lab Manager - Development > Users and groups > Add user/group_
   - Fill in the form with the following details:
      - Users: _Select all users who should have access as Administrators_
      - Select a role: _Administrator_

### Running the application

1. Update the following section in your _appsettings.Development.json_ (Presentation.BlazorServer)

   ```json
   {
      "AzureAd": {
         "Instance": "https://login.microsoftonline.com/",
         "Domain": "example.com",
         "TenantId": "00000000-0000-0000-0000-000000000000",
         "ClientId": "00000000-0000-0000-0000-000000000000",
         "CallbackPath": "/signin-oidc",
         "SignedOutCallbackPath": "/signout-callback-oidc"
      }
   }
   ```

1. Add secrets
   - `cd .\src\Presentation.BlazorServer\`
   - `dotnet user-secrets set "ConnectionStrings:DbConnection" "Data Source=localhost,1433;Initial Catalog=LabManagementSystemDb;User ID=SA;Password=Password_123;TrustServerCertificate=True;"`
   - `dotnet user-secrets set "AzureAd:ClientSecret" "YourClientSecret"`
1. Run the application
   - `dotnet run --project .\src\Presentation.BlazorServer\LabManagementSystem.Presentation.BlazorServer.csproj`

### Testing the application

1. Add secrets
   - IntegrationTests.Core.Application
      - `cd .\tests\Core\LabManagementSystem.IntegrationTests.Core.Application\`
      - `dotnet user-secrets set "ConnectionStrings:DbConnection" "Data Source=localhost,1433;Initial Catalog=LabManagementSystemIntegrationTestsDb;User ID=SA;Password=Password_123;TrustServerCertificate=True;"`
   - IntegrationTests.Core.Application.Allocation
      - `cs .\tests\Core\LabManagementSystem.IntegrationTests.Core.Application.Allocation`
      - `dotnet user-secrets set "ConnectionStrings:DbConnection" "Data Source=localhost,1433;Initial Catalog=LabManagementSystemIntegrationTestsDb;User ID=SA;Password=Password_123;TrustServerCertificate=True;"`
1. Run the tests
   - `dotnet test`
