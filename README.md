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

# RaceDay – Programming 2B POE Part 3

## Project Overview

RaceDay is a web-based event management system developed for the Programming 2B Portfolio of Evidence (POE). Part 3 extends the existing RaceDay API by introducing an ASP.NET Core MVC web application.

The system allows organisers to manage running events and categories, while participants can enrol in events and view their information.

## Technologies Used

- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server
- Azure Blob Storage
- HTML and CSS
- Bootstrap
- Visual Studio 2022
- GitHub and GitHub Actions
- Docker and Docker Compose

## Project Structure

The solution contains the following main projects:

- **RaceDay.API** – Handles application data, authentication, event management, participant enrolments and file uploads.
- **RaceDay.Web** – The MVC web application that provides the user interface and communicates with the API.
- **RaceDay.Tests** – Contains automated tests for application functionality.

The solution also includes Docker configuration files:

- `RaceDay.API/Dockerfile`
- `RaceDay.Web/Dockerfile`
- `docker-compose.yml`

## Features Implemented in Part 3

### 1. MVC Web Application

An ASP.NET Core MVC application was added to provide a user-friendly interface for the RaceDay system.

### 2. Organiser Functionality

Organisers can:
- View and manage events.
- Create, update and delete events.
- Manage event categories.
- Upload files through the application.

### 3. Participant Functionality

Participants can:
- View available events.
- Enrol in events.
- View their enrolments.
- View their results.
- View and manage their profile information.

### 4. API Integration

The MVC application communicates with the existing RaceDay Web API through the `RaceDayApiService`.

The MVC application uses the API for data operations instead of communicating directly with the database.

### 5. Azure Blob Storage

Azure Blob Storage was integrated to support file uploads.

The API handles file uploads through the `BlobStorageService`. The MVC application sends files to the API, and the API stores them in Azure Blob Storage.

Supported file types:
- JPEG
- PNG
- PDF

The maximum upload size is 5 MB.

Azure connection details are stored securely using Visual Studio User Secrets and should not be committed to GitHub.

### 6. Docker Configuration

Dockerfiles were added for both the API and MVC projects, together with a Docker Compose configuration to define the application services.

The Docker configuration is included as part of the Part 3 deployment work. The current SQL Server LocalDB connection requires additional configuration before the application can run fully inside Linux containers.

## Requirements

To run the application locally, you need:

- Visual Studio 2022
- .NET 8 SDK
- SQL Server LocalDB
- A configured Azure Blob Storage account for file uploads

## Running the Application Locally

1. Clone or download this repository.
2. Open the `RaceDay.API` solution in Visual Studio 2022.
3. Check the database connection string in the API configuration.
4. Make sure SQL Server LocalDB is installed and the RaceDay database is available.
5. Configure the Azure Blob Storage connection string using Visual Studio User Secrets.
6. Set `RaceDay.API` and `RaceDay.Web` as the startup projects, where applicable.
7. Run the API and MVC application.
8. Open the MVC application in your browser.

The API Swagger interface can be accessed at:

`https://localhost:7209/swagger`

The MVC application URL depends on the launch settings configured in Visual Studio.

## Azure Blob Storage Configuration

The Azure Blob Storage connection string must be configured through User Secrets.

Do not add real Azure connection strings, passwords, API keys or other private credentials to GitHub.

## GitHub Repository

**Repository:**  
https://github.com/Mpiloo11/PROG6212-RaceDay

Part 3 development is maintained in the existing RaceDay repository to preserve the work completed in Parts 1 and 2.
The project uses GitHub Actions to build the application and run its automated tests. A successful CI run confirms that the build completes and the tests pass.


