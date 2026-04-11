# AridentIAM

AridentIAM is an enterprise-oriented **Identity and Access Management (IAM)** API built with **ASP.NET Core Web API**, **Clean/Onion Architecture**, **CQRS with MediatR**, and **Entity Framework Core**.

The solution is currently implemented up to the **Task 6** milestone, which includes:

- layered solution structure
- rich Domain model
- Application CQRS infrastructure
- first end-to-end **Users** API slice
- EF Core `DbContext` and core persistence wiring
- structured database configuration
- startup migration and bootstrap seed flow
- Swagger, Serilog, middleware, health checks, and rate limiting

---

## Contents

- [Overview](#overview)
- [Current Implementation Status](#current-implementation-status)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Solution Structure](#solution-structure)
- [Database Configuration](#database-configuration)
- [Getting Started](#getting-started)
- [EF Core Migrations](#ef-core-migrations)
- [Run the Application](#run-the-application)
- [Swagger and First API Slice](#swagger-and-first-api-slice)
- [Expected Seed Data](#expected-seed-data)
- [Troubleshooting](#troubleshooting)
- [Roadmap](#roadmap)

# Overview

The project is organized into four primary layers:

- **AridentIam.Domain**  
  Core business model, aggregates, value objects, enums, domain events, repository contracts, and specifications.

- **AridentIam.Application**  
  CQRS use-case layer using MediatR, validation behaviors, application exceptions, and first user management feature flow.

- **AridentIam.Infrastructure**  
  EF Core, repositories, Unit of Work, structured database configuration, startup migrations, and seed logic.

- **AridentIam.WebApi**  
  REST API, middleware, startup composition, Swagger, health checks, authentication, rate limiting, and contracts.

The first implemented end-to-end feature is:

- **Users**
  - `POST /api/v1/users`
  - `GET /api/v1/users/{userExternalId}`

---

# Current Implementation Status

## Completed up to Task 6

### Domain
- broad IAM domain model organized by business modules
- aggregate roots and supporting entities
- value objects and enums
- repository abstractions
- domain events
- specifications foundation

### Application
- MediatR registration
- CQRS marker interfaces
- pipeline behaviors:
  - logging
  - validation
  - transaction
- common exceptions
- first user feature:
  - `CreateUser`
  - `GetUserByExternalId`

### Infrastructure
- structured database settings
- SQL Server connection-string factory
- `AridentIamDbContext`
- EF Core entity configurations for:
  - `Tenant`
  - `Principal`
  - `User`
- repositories for:
  - `Principal`
  - `Tenant`
  - `User`
- `UnitOfWork`
- startup migration extension
- bootstrap seeding extension

### Web API
- `UsersController`
- correlation ID middleware
- exception handling middleware
- startup extension methods
- Swagger/OpenAPI
- JWT bearer authentication setup
- CORS
- health checks
- rate limiting

---

# Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core 8**
- **SQL Server**
- **MediatR**
- **FluentValidation**
- **Serilog**
- **Swagger / Swashbuckle**

---

# Prerequisites

Before running the project, make sure you have:

- **.NET SDK 8.0**
- **SQL Server** accessible from your machine
- **Visual Studio 2026** or **Visual Studio Code**
- **EF Core CLI tools**

Install EF tools if needed:

```powershell
dotnet tool update --global dotnet-ef --version 8.0.15
```

---

# Solution Structure

```text
AridentIam
├── AridentIam.Domain
│   ├── Common
│   ├── Entities
│   │   ├── Attributes
│   │   ├── Auditing
│   │   ├── Authorization
│   │   ├── Credentials
│   │   ├── Delegations
│   │   ├── Governance
│   │   ├── Groups
│   │   ├── Integrations
│   │   ├── Organizations
│   │   ├── Permissions
│   │   ├── Policies
│   │   ├── Principals
│   │   ├── Relationships
│   │   ├── Resources
│   │   ├── Roles
│   │   ├── Sessions
│   │   ├── Tenants
│   │   ├── Users
│   │   └── Workflows
│   ├── Enums
│   ├── Events
│   ├── Interfaces
│   ├── Specifications
│   └── ValueObjects
│
├── AridentIam.Application
│   ├── Behaviors
│   ├── Common
│   │   ├── CQRS
│   │   ├── Exceptions
│   │   └── Interfaces
│   ├── Features
│   │   └── Users
│   │       ├── Commands
│   │       ├── DTOs
│   │       ├── Mappings
│   │       └── Queries
│   └── DependencyInjection.cs
│
├── AridentIam.Infrastructure
│   ├── Configuration
│   │   ├── DatabaseConnectionStringFactory.cs
│   │   └── DatabaseSettings.cs
│   ├── Persistence
│   │   ├── Context
│   │   ├── Migrations
│   │   ├── Seed
│   │   ├── Startup
│   │   └── UnitOfWork
│   ├── Repositories
│   ├── Services
│   └── DependencyInjection.cs
│
└── AridentIam.WebApi
    ├── Contracts
    ├── Controllers
    ├── Extensions
    ├── Middleware
    ├── Properties
    ├── Program.cs
    ├── appsettings.json
    └── appsettings.Development.json
```

---

# Database Configuration

The project uses a **structured database settings** model, not a single raw connection string.

## Configuration section

`appsettings.json` and `appsettings.Development.json` use this shape:

```json
"Database": {
  "Host": "MSI",
  "Port": null,
  "Database": "AridentIam",
  "Username": "abhishek.patil",
  "Password": "Admin@123",
  "TrustedConnection": false,
  "Encrypt": false,
  "TrustServerCertificate": true,
  "MultipleActiveResultSets": true,
  "CommandTimeoutSeconds": 30
}
```

## Supported modes

### Windows Authentication
Use:

```json
"TrustedConnection": true,
"Username": null,
"Password": null
```

### SQL Authentication
Use:

```json
"TrustedConnection": false,
"Username": "your_sql_login",
"Password": "your_sql_password"
```

## Important note
For local SQL Server, keep `Port` as `null` unless you have explicitly confirmed that SQL Server is listening on a fixed TCP port such as `1433`.

---

# Getting Started

## 1. Restore packages

```powershell
dotnet restore
```

## 2. Build the solution

```powershell
dotnet build
```

---

# EF Core Migrations

## Important

The application is configured to:

1. apply migrations on startup
2. then run a database seed

However, the **initial migration must exist first**.

## Create the initial migration

Run this from the solution root:

```powershell
dotnet ef migrations add InitialCreate `
  --project AridentIam.Infrastructure `
  --startup-project AridentIam.WebApi `
  --context AridentIamDbContext `
  --output-dir Persistence/Migrations/Generated
```

## Apply the migration manually

```powershell
dotnet ef database update `
  --project AridentIam.Infrastructure `
  --startup-project AridentIam.WebApi `
  --context AridentIamDbContext
```

## Remove the last migration if needed

```powershell
dotnet ef migrations remove `
  --project AridentIam.Infrastructure `
  --startup-project AridentIam.WebApi `
  --context AridentIamDbContext
```

## Expected migration output
After creating the initial migration, verify that migration files are generated under:

```text
AridentIam.Infrastructure/Persistence/Migrations/Generated
```

and that the schema includes at minimum:

- `Tenants`
- `TenantSettings`
- `Principals`
- `Users`

---

# Run the Application

Once the initial migration exists, run the API:

```powershell
dotnet run --project AridentIam.WebApi
```

If startup succeeds, the application should:

1. build the service container
2. apply pending migrations
3. seed the default tenant if needed
4. start the HTTP pipeline
5. expose Swagger and the health endpoint

---

# Swagger and First API Slice

## Swagger URL
When running locally, open:

```text
https://localhost:<port>/swagger
```

The exact port depends on your launch profile.

---

## Implemented endpoints

### Create a user
`POST /api/v1/users`

### Get user by ExternalId
`GET /api/v1/users/{userExternalId}`

---

## Create user via Swagger

Use the seeded default tenant:

```json
{
  "tenantExternalId": "11111111-1111-1111-1111-111111111111",
  "firstName": "Abhishek",
  "lastName": "Patil",
  "displayName": "Abhishek Patil",
  "email": "abhishek.patil@example.com",
  "username": "abhishek.patil",
  "phoneNumber": "+919999999999",
  "jobTitle": "Software Engineer",
  "employmentType": 1
}
```

### Expected response
- **201 Created**
- response payload:

```json
{
  "principalExternalId": "GUID",
  "userExternalId": "GUID",
  "tenantExternalId": "11111111-1111-1111-1111-111111111111",
  "displayName": "Abhishek Patil",
  "email": "abhishek.patil@example.com",
  "username": "abhishek.patil"
}
```

---

## Fetch the created user

`GET /api/v1/users/{userExternalId}`

### Expected response
- **200 OK**
- payload shape:

```json
{
  "userExternalId": "GUID",
  "principalExternalId": "GUID",
  "tenantExternalId": "11111111-1111-1111-1111-111111111111",
  "firstName": "Abhishek",
  "lastName": "Patil",
  "displayName": "Abhishek Patil",
  "email": "abhishek.patil@example.com",
  "username": "abhishek.patil",
  "phoneNumber": "+919999999999",
  "employmentType": 1,
  "jobTitle": "Software Engineer",
  "isEmailVerified": false,
  "isPhoneVerified": false
}
```

---

# Expected Seed Data

On startup, the application attempts to seed a **default tenant**.

## Default tenant
- **TenantExternalId:** `11111111-1111-1111-1111-111111111111`
- **Code:** `DEFAULT`
- **Name:** `Default Tenant`

This seed runs only after migrations are applied and the schema is available.

---

# Troubleshooting

## 1. `Swagger 500.30`
This means the application failed during startup.

Check:
- database settings in `appsettings.Development.json`
- SQL Server accessibility
- whether the initial migration exists
- whether the `Tenants` table was created
- startup logs in:

```text
AridentIam.WebApi/logs
```

---

## 2. `Invalid object name 'Tenants'`
This usually means:

- the initial migration was not created
- or the migration was not applied
- or startup seeding ran before the schema existed

Fix:
1. create the migration
2. run `dotnet ef database update`
3. restart the API

---

## 3. `Your startup project 'AridentIam.WebApi' doesn't reference Microsoft.EntityFrameworkCore.Design`
If EF tooling reports this, add `Microsoft.EntityFrameworkCore.Design` to `AridentIam.WebApi.csproj` or use the correct startup project configuration.

---

## 4. `HostAbortedException` during `dotnet ef`
This can happen during EF design-time execution when the host is intentionally spun up and aborted by the tooling. It can be misleading if logged as a fatal runtime crash.

---

## 5. SQL Server connection timeout
If SQL Server works in SSMS but not from the app:
- do **not** force a port unless needed
- prefer:
  - `"Host": "MSI", "Port": null`
  - or `"Host": ".", "Port": null`
- verify `TrustedConnection` vs `Username/Password` are configured consistently

---

## 6. MediatR license warning
A development warning may appear for the Lucky Penny MediatR license package. This does not block local development, but production use requires proper licensing.

---

# Roadmap

The project is currently at an early but solid foundation stage. Likely next milestones include:

- finalizing EF migrations and schema bootstrap
- stabilizing startup and database initialization
- expanding the first API slice tests
- adding more vertical slices:
  - Tenants
  - Organizations
  - Roles
  - Permissions
  - Policies
- introducing integration tests
- strengthening authentication and authorization flows

---

# Notes

- The project uses a **startup migration + seed model**, so the first migration must exist before expecting a successful first run.
- The current API is intentionally small and focused while the architecture foundation is being stabilized.
- JWT authentication is configured, but the first Swagger flow may be tested without a full login/token issuance flow depending on current endpoint attributes.

---

# License / Internal Usage Note

This project uses MediatR and may emit a development-time Lucky Penny license warning. Confirm licensing requirements before production deployment.
