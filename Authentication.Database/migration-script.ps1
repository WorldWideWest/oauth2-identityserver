$Folder = '.\Migrations'

Write-Host "Checking for Migrations in $Folder"
if (!(Test-Path $Folder)) {
    Write-Host "Adding necessary Migrations" -f Yellow

    Write-Host "Adding [ApplicationDbContext] Migrations" -f Cyan
    dotnet ef --startup-project ..\Authentication.API\ migrations add InitIdentityUser -c ApplicationDbContext -o .\Migrations\Identity\
    
    # https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html#database-schema-changes-and-using-ef-migrations
    Write-Host "Adding [PersistedGrantDbContext] Migrations" -f Cyan
    dotnet ef --startup-project ..\Authentication.API\ migrations add InitialIdentityServerPersistedGrantDbMigration  -c PersistedGrantDbContext -o .\Migrations\IdentityServer\PersistedGrantDb
 
    Write-Host "Adding [ConfigurationDbContext] Migrations" -f Cyan
    dotnet ef --startup-project ..\Authentication.API\ migrations add  InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o .\Migrations\IdentityServer\ConfigurationDb
   }

Write-Host "Applying Migrations for [ApplicationDbContext]" -f Cyan
dotnet ef --startup-project ..\Authentication.API\ database update -c ApplicationDbContext

Write-Host "Applying Migrations for [PersistedGrantDbContext]" -f Cyan
dotnet ef --startup-project ..\Authentication.API\ database update -c PersistedGrantDbContext

Write-Host "Applying Migrations for [ConfigurationDbContext]" -f Cyan
dotnet ef --startup-project ..\Authentication.API\ database update -c ConfigurationDbContext 