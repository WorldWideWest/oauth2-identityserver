#!/bin/bash

# If the dotnet ef command isnt recognized install it with the following command:
# dotnet tool install -g dotnet-ef

Folder="./Migrations"

echo "Checking for Migrations in $Folder"
if [ ! -d "$Folder" ]; then
    echo -e "Adding necessary Migrations" "\033[33m"

    echo -e "Adding [ApplicationDbContext] Migrations" "\033[36m"
    dotnet ef --startup-project ../Authentication.API/ migrations add InitIdentityUser -c ApplicationDbContext -o ./Migrations/Identity/
fi

echo -e "Applying Migrations for [ApplicationDbContext]" "\033[36m"
dotnet ef --startup-project ../Authentication.API/ database update -c ApplicationDbContext