$Solution = './Authentication'
if(!(Test-Path $Solution))
{
    # More about creating a solution here https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-sln?source=recommendations
    dotnet new sln --name Authentication # initializing the solution

    # Read more about the project types here https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new#arguments
    Write-Host "Creating the project [Authentication.API]" -f Cyan
    dotnet new webapi --name Authentication.API

    Write-Host "Creating the project [Authentication.Database]" -f Cyan
    dotnet new classlib --name Authentication.Database

    Write-Host "Creating the project [Authentication.Models]" -f Cyan
    dotnet new classlib --name Authentication.Models

    Write-Host "Creating the project [Authentication.Services]" -f Cyan
    dotnet new classlib --name Authentication.Services

    Write-Host "Creating the project [Authentication.Tests]" -f Cyan
    dotnet new classlib --name Authentication.Tests

    # Connecting the projects to the solution https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-sln#add
    Write-Host "Added [Authentication.API] to [Authentication] solution" -f Cyan
    dotnet sln Authentication.sln add ./Authentication.API/Authentication.API.csproj

    Write-Host "Added [Authentication.Database] to [Authentication] solution" -f Cyan
    dotnet sln Authentication.sln add ./Authentication.Database/Authentication.Database.csproj

    Write-Host "Added [Authentication.Models] to [Authentication] solution" -f Cyan
    dotnet sln Authentication.sln add ./Authentication.Models/Authentication.Models.csproj

    Write-Host "Added [Authentication.Services] to [Authentication] solution" -f Cyan
    dotnet sln Authentication.sln add ./Authentication.Services/Authentication.Services.csproj

    Write-Host "Added [Authentication.Tests] to [Authentication] solution" -f Cyan
    dotnet sln Authentication.sln add ./Authentication.Tests/Authentication.Tests.csproj
}
