#!/bin/bash

Folder="./Migrations"

# If the dotnet ef command isnt recognized install it with the following command:
# dotnet tool install -g dotnet-ef

echo "Checking for Migrations in $Folder"
if [ ! -d "$Folder" ]; then
    echo -e "Adding necessary Migrations" "\033[33m"

    echo -e "Adding [ApplicationDbContext] Migrations" "\033[36m"
    dotnet ef --startup-project ../Authentication.API/ migrations add InitIdentityUser -c ApplicationDbContext -o ./Migrations/Identity/
    
    # https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html#database-schema-changes-and-using-ef-migrations
    echo -e "Adding [PersistedGrantDbContext] Migrations" "\033[36m"
    dotnet ef --startup-project ../Authentication.API/ migrations add InitialIdentityServerPersistedGrantDbMigration  -c PersistedGrantDbContext -o ./Migrations/IdentityServer/PersistedGrantDb
 
    echo -e "Adding [ConfigurationDbContext] Migrations" "\033[36m"
    dotnet ef --startup-project ../Authentication.API/ migrations add  InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o ./Migrations/IdentityServer/ConfigurationDb
fi

echo -e "Applying Migrations for [ApplicationDbContext]" "\033[36m"
dotnet ef --startup-project ../Authentication.API/ database update -c ApplicationDbContext

echo -e "Applying Migrations for [PersistedGrantDbContext]" "\033[36m"
dotnet ef --startup-project ../Authentication.API/ database update -c PersistedGrantDbContext

echo -e "Applying Migrations for [ConfigurationDbContext]" "\033[36m"
dotnet ef --startup-project ../Authentication.API/ database update -c ConfigurationDbContext