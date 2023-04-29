$Folder = '.\Migrations'

Write-Host "Checking for Migrations in $Folder"
if (!(Test-Path $Folder)) {
    Write-Host "Adding necessary Migrations" -f Yellow

    Write-Host "Adding [ApplicationDbContext] Migrations" -f Cyan
    dotnet ef --startup-project ..\Authentication.API\ migrations add InitIdentityUser -c ApplicationDbContext -o .\Migrations\Identity\
   }

Write-Host "Applying Migrations for [ApplicationDbContext]" -f Cyan
dotnet ef --startup-project ..\Authentication.API\ database update -c ApplicationDbContext