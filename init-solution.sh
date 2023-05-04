#!/bin/bash

Solution='./Authentication.sln'
if [ ! -d "$Solution" ]
then
    # More about creating a solution here https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln?source=recommendations
    dotnet new sln --name Authentication # initializing the solution

    # Read more about the project types here https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22#arguments
    echo -e "\e[36mCreating the project [Authentication.API]\e[0m"
    dotnet new webapi --name Authentication.API

    echo -e "\e[36mCreating the project [Authentication.Database]\e[0m"
    dotnet new classlib --name Authentication.Database

    echo -e "\e[36mCreating the project [Authentication.Models]\e[0m"
    dotnet new classlib --name Authentication.Models

    echo -e "\e[36mCreating the project [Authentication.Services]\e[0m"
    dotnet new classlib --name Authentication.Services

    echo -e "\e[36mCreating the project [Authentication.Tests]\e[0m"
    dotnet new mstest --name Authentication.Tests

    # Connecting the projects to the solution https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln?source=recommendations#add
    echo -e "\e[36mAdded [Authentication.API] to [Authentication] solution\e[0m"
    dotnet sln Authentication.sln add ./Authentication.API/Authentication.API.csproj

    echo -e "\e[36mAdded [Authentication.Database] to [Authentication] solution\e[0m"
    dotnet sln Authentication.sln add ./Authentication.Database/Authentication.Database.csproj

    echo -e "\e[36mAdded [Authentication.Models] to [Authentication] solution\e[0m"
    dotnet sln Authentication.sln add ./Authentication.Models/Authentication.Models.csproj

    echo -e "\e[36mAdded [Authentication.Services] to [Authentication] solution\e[0m"
    dotnet sln Authentication.sln add ./Authentication.Services/Authentication.Services.csproj

    echo -e "\e[36mAdded [Authentication.Tests] to [Authentication] solution\e[0m"
    dotnet sln Authentication.sln add ./Authentication.Tests/Authentication.Tests.csproj
fi