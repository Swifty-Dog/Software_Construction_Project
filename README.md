# Software Construction

### Required Packages 
# ASP.NET Core packages
dotnet add package Microsoft.AspNetCore.Mvc // deleted cuz old versions warnings
dotnet add package Microsoft.AspNetCore.Authentication // deleted cuz old versions warnings
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Entity Framework Core with SQLite packages
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

# Testing packages
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq
dotnet add package Microsoft.AspNetCore.Mvc.Testing

# Logger packages
dotnet add package Serilog
dotnet add package Serilog.Extensions.Logging
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.AspNetCore

(pip install razor als je Razor language version downgrade hebt) 

*note 
when working with the database use 
# Create an initial migration
dotnet ef migrations add {name}
example dotnet ef migrations add TableItems

# Apply the migration and update the database
dotnet ef database update



### Explanation:
- **ASP.NET Core packages**: These are needed to handle MVC or Web API functionalities, authentication, and identity management.
- **Entity Framework Core packages**: These are necessary for database interactions using Entity Framework Core with SQLite as the provider.
- **Migration commands**: Instructions for creating and applying database migrations.
- **Serilog packages**: These are needed to log the changes in the database.

# maybe needed
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package FluentAssertions
dotnet add package NUnit
dotnet add package NUnit3TestAdapter
dotnet add package Microsoft.AspNetCore.Mvc.Testing

test command 
dotnet test --filter "FullyQualifiedName~TransfersTest"
or by folder
dotnet test ./Test

# to install 15-11-2024
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
dotnet add package coverlet.collector
dotnet add Test.csproj package coverlet.collector

## commmand to give a coverage report. -- need to ask more about this 
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~WarehousesTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~LocationsTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~ClientsTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~SuppliersTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~TransfersTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~ItemLineTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~ItemGroupsTest
dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal --filter FullyQualifiedName~ItemTest

# for unit testing with Moq
first: cd .\Test\
then: dotnet add package Moq
