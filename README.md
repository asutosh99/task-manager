# 🚀 Task Manager API (.NET 8 + AI-Ready Backend)

A production-style Task Management Backend API built with ASP.NET Core (.NET 8), focusing on **security, clean architecture, testability, and real-world infrastructure readiness**.

---

# 📌 Overview

This project is a secure, scalable backend system that supports:

* JWT-based authentication
* Role-based authorization (User/Admin)
* Resource-level (ownership) security
* Clean service-based architecture
* Centralized exception handling
* Unit testing (~70–75% coverage)
* Dockerized database setup (SQL Server)
* Environment-based configuration (Dev/Prod)

---

# 🧠 Key Features

## 🔐 Authentication & Authorization

* JWT-based authentication
* BCrypt password hashing
* Role-based access control
* Secure endpoints using `[Authorize]`

---

## 🔥 Resource-Level Security (Critical)

Not just authentication — **authorization per resource**

```csharp
if (task.UserId != currentUserId && role != "Admin")
    throw new UnauthorizedAccessException();
```

* Users → only their own tasks
* Admin → full access

---

## 📋 Task Management

* Create, Update, Delete tasks
* Get user-specific tasks
* Pagination
* Filtering (status)
* Sorting (asc/desc)

---

## 🛡 Validation

* FluentValidation
* Centralized validation logic
* No manual validation in controllers

---

## ⚠️ Error Handling (Middleware-Based)

| Exception                   | HTTP Code |
| --------------------------- | --------- |
| UnauthorizedAccessException | 401       |
| KeyNotFoundException        | 404       |
| ValidationException         | 400       |
| Others                      | 500       |

Example:

```json
{
  "success": false,
  "message": "Task not found",
  "data": null
}
```

---

## 📊 Logging

* Serilog
* Structured logging
* Console + file output

---

# 🏗 Architecture

```
Controller → Service → DbContext → Database
                ↑
         Interfaces (DI)
```

### Layers

* **Controllers** → HTTP handling only
* **Services** → business logic + authorization
* **Interfaces** → abstraction + testability
* **Middleware** → global exception handling
* **DTOs** → request/response models

---

# 👤 Current User Handling

```csharp
ICurrentUserService
```

* Extracts user info from JWT claims
* Used inside services (not controllers)
* Enables testable business logic

---

# 🧪 Testing

### Tools

* xUnit
* Moq
* FluentAssertions
* EF Core InMemory

### Coverage

~70–75% (business logic focused)

### Tested Scenarios

* Task creation
* Authorization (owner vs admin)
* Delete logic
* Not-found handling

---

# 🔄 API Response Standard

All responses follow:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": {}
}
```

Pagination:

```json
{
  "success": true,
  "data": [],
  "page": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5
}
```

---

# ⚙️ Environment Configuration

Supports environment-based configuration:

* `appsettings.json` → base config
* `appsettings.Development.json` → dev overrides
* `appsettings.Production.json` → production
* **User Secrets (dev)**
* **Environment Variables (prod)**

### Example (User Secrets)

```bash
dotnet user-secrets set "Jwt:Key" "your_secret"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_db"
```

### Example (Production Env Vars)

```bash
Jwt__Key=your_secret
ConnectionStrings__DefaultConnection=your_db
```

---

# 🐳 Docker Setup (SQL Server)

Run SQL Server in Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=StrongPass123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Connection string:

```text
Server=localhost,1433;Database=TaskFlowDb;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True;
```

---

# 🐳 Docker Compose (Full System)

```yaml
version: '3.8'

services:
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "StrongPass123!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"

  api:
    build:
      context: .
      dockerfile: task manager/Dockerfile
    ports:
      - "5000:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=TaskFlowDb;User=sa;Password=StrongPass123!;
      - Jwt__Key=dev_secret_key
    depends_on:
      - db
```

Run:

```bash
docker-compose up --build
```

---

# 🧪 API Endpoints

## Auth

| Method | Endpoint           | Description |
| ------ | ------------------ | ----------- |
| POST   | /api/auth/register | Register    |
| POST   | /api/auth/login    | Login       |

## Tasks

| Method | Endpoint        | Description |
| ------ | --------------- | ----------- |
| GET    | /api/tasks      | Get tasks   |
| POST   | /api/tasks      | Create      |
| PUT    | /api/tasks/{id} | Update      |
| DELETE | /api/tasks/{id} | Delete      |

---

# 🚀 How to Run (Dev)

```bash
git clone https://github.com/asutosh99/task-manager
cd task_manager
dotnet user-secrets init
dotnet ef database update
dotnet run
```

Swagger:

```
https://localhost:<port>/swagger
```

---

# 🧪 Run Tests

```bash
dotnet test
```

---

# 🔒 Security Practices

* BCrypt password hashing
* JWT expiration
* Role-based authorization
* Resource-level access control
* Environment-based secrets

---

# ⚠️ Known Limitations

* No refresh tokens
* No caching layer
* No frontend yet

---

# 📈 Future Improvements

## Backend

* Refresh tokens
* Redis caching
* Rate limiting

## DevOps

* CI/CD pipeline
* Azure deployment
* Docker optimization

## AI

* Task summarization
* Smart suggestions
* Priority prediction

---

# 🧠 Engineering Principles

* Separation of concerns
* Exception-driven flow
* Interface-based design
* Testable services
* Resource-based authorization

---

# 👨‍💻 Author

Built as part of transitioning from:

**React Developer → .NET Full Stack Engineer (AI-focused)**

---

# 🧨 Final Note

This is NOT a tutorial CRUD app.

It focuses on:

* correctness
* security
* architecture
* real-world backend practices
