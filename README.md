# PROG6212-RaceDay
RaceDay Event Management System - Programming 2B Part 1
# RaceDay Event Management System

Programming 2B Part 1

RaceDay is a web-based event management system for South African
road running, walking and cycling events.

The system allows organisers to create and manage events, categories,
enrolments and participant results.

Participants can browse events, enrol in events and view their
enrolment and result information.

YT Link - https://youtu.be/aD3xFdHpo58?si=R4FrevxF6cjguy35

Part 2
RaceDay API – Setup Instructions

1. Project Overview

RaceDay is a web API developed using C#, ASP.NET Core Web API, Entity Framework Core, and SQL Server. It supports user authentication, role-based access, event management, event categories, enrolments, and race results.

2. Prerequisites

Before running the project, ensure you have installed:

- Visual Studio 2022 with the ASP.NET and web development workload.
- A compatible .NET SDK for the project.
- SQL Server (LocalDB or a compatible SQL Server instance).
- Git.
- A web browser for accessing the API through Swagger.

3. Clone the Repository

Open a terminal or Command Prompt and run:

git clone https://github.com/Mpiloo11/PROG6212-RaceDay.git
cd PROG6212-RaceDay

Alternatively, open the repository in GitHub, select Code, and download or clone the repository.

4. Open the Solution

1. Open Visual Studio.
2. Select Open a project or solution.
3. Open the RaceDay solution file (".sln") if one is available.
4. If there is no solution file, open the project file inside the "RaceDay.API" folder.
5. Allow Visual Studio to restore the required NuGet packages.

5. Configure the Database

1. Open the API project's configuration files.
2. Locate the "ConnectionStrings" section in "appsettings.json".
3. Ensure the connection string points to your installed SQL Server instance.
4. Confirm that the database name and authentication settings are correct for your environment.

Do not commit passwords or other sensitive credentials to GitHub.

6. Apply Entity Framework Core Migrations

Open the Visual Studio Package Manager Console and select "RaceDay.API" as the default project.

If migrations already exist, run:

Update-Database

Alternatively, from a terminal in the repository root, use the appropriate project paths:

dotnet ef database update --project RaceDay.API --startup-project RaceDay.API

If the Entity Framework Core CLI is not installed, install the compatible "dotnet-ef" tool first. Use the same major version as your project's Entity Framework Core packages.

7. Build and Run the API

In Visual Studio:

1. Set "RaceDay.API" as the startup project.
2. Select Build > Build Solution.
3. Run the application using F5 or Ctrl + F5.

Alternatively, from the repository root:

dotnet restore RaceDay.API
dotnet build RaceDay.API
dotnet run --project RaceDay.API

Use the application URL displayed in the terminal or Visual Studio output.

8. Access Swagger

When the API is running and Swagger is enabled, open the Swagger URL shown by the application, usually:

"https://localhost:<port>/swagger"

Swagger can be used to inspect and test the available API endpoints.

9. Run the Tests

In Visual Studio, open Test > Test Explorer and select Run All Tests.

Alternatively, run:

dotnet test RaceDay.Tests

Confirm that the tests pass before submitting changes.

10. Continuous Integration (CI)

The repository uses GitHub Actions to build and test the project when changes are pushed to GitHub, provided the workflow is configured for these steps.

To check the result:

1. Open the repository on GitHub.
2. Select the Actions tab.
3. Open the latest workflow run.
4. Confirm that the build and tests completed successfully.

A green check indicates that the workflow run passed.

11. User Roles and API Features

The API includes functionality for authentication and user profiles, event management, event categories, enrolments, and race results. Access to restricted operations depends on the user's assigned role and the endpoint's authorization rules.

The demonstration should show the API running, the Swagger interface, key endpoints, authentication and role-based access, and the automated tests.

Continuous Integration (CI)

The project uses GitHub Actions to build the application and run its automated tests. A successful CI run confirms that the build completes and the tests pass.

Successful CI Build

"Successful GitHub Actions CI build" (screenshots/ci-green-build.png)
