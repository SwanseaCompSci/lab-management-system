# Database Migration Utility

## Prerequisite
- [Entity Framework Core tools (.NET Core CLI)](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
   - Install: `dotnet tool install --global dotnet-ef`
   - Update: `dotnet tool update --global dotnet-ef`
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/)
   - Install / Update: `dotnet add package Microsoft.EntityFrameworkCore.Design`

## Generate Migrations
- Navigate to `LabManagementSystem\utils\DatabaseMigrationUtility`
- Run the following command

  ```
  dotnet ef migrations add <name-of-migration> --framework net6.0 --project ..\..\src\Infrastructure.Persistence\ --output-dir ..\..\src\Infrastructure.Persistence\Migrations
  ```
  > Replace `<name-of-migration>` with a name of the migration

## Notes
The utility uses `Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=SwanseaCompSciLabManagementSystemMigration` as its connection string.