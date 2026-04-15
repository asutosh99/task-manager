# 🚀 Task Manager API (.NET 8 + AI-Ready Backend)

A production-style **Task Management Backend API** built with **ASP.NET Core (.NET 8)**, focusing on **security, clean architecture, and testable design**.

---

## 📌 Overview

This project is a **secure, scalable backend system** that supports:

* JWT-based authentication
* Role-based authorization (User/Admin)
* User-level data isolation (ownership validation)
* Clean service-based architecture
* Centralized exception handling
* Unit & integration-style testing (~70–75% coverage)

Designed as a foundation for a **full-stack AI-powered system**.

---

## 🧠 Key Features

### 🔐 Authentication & Authorization

* JWT-based authentication
* Password hashing using BCrypt
* Role-based access (`User`, `Admin`)
* Secure endpoints using `[Authorize]`

---

### 🔥 Ownership-Based Security (Critical Feature)

> Not just “is user logged in?” — but
> **“Can this user access THIS resource?”**

* Users can access only **their own tasks**
* Admins can access **all tasks**

```csharp
if (task.UserId != currentUserId && role != "Admin")
    throw new UnauthorizedAccessException();
```

---

### 📋 Task Management

* Create, Update, Delete tasks
* Get user-specific tasks
* Pagination
* Filtering (status)
* Sorting (asc/desc)

---

### 🛡 Validation

* FluentValidation for request validation
* Centralized validation logic
* No manual validation in controllers

---

### 📊 Logging

* Serilog integration
* Structured logging
* Console + file output

---

### ⚠️ Error Handling (Middleware-Based)

All exceptions are handled centrally:

| Exception                   | HTTP Code |
| --------------------------- | --------- |
| UnauthorizedAccessException | 401       |
| KeyNotFoundException        | 404       |
| ValidationException         | 400       |
| Others                      | 500       |

Example response:

```json
{
  "success": false,
  "message": "Task not found",
  "data": null
}
```

---

## 🏗 Architecture

```text
Controller → Service → DbContext → Database
                ↑
         Interfaces (DI)
```

### Layers

* **Controllers**

  * Handle HTTP requests/responses
  * No business logic

* **Services**

  * Core business logic
  * Authorization & ownership rules

* **Interfaces**

  * Decouple implementation
  * Enable unit testing

* **Middleware**

  * Global exception handling

* **DTOs**

  * Request/response models only

---

## 👤 Current User Handling

Abstracted via:

```csharp
ICurrentUserService
```

* Extracts user info from JWT claims
* Used inside services (not controllers)
* Makes business logic **testable**

---

## 🧪 Testing

### Tools

* xUnit
* Moq
* FluentAssertions
* EF Core InMemory

### Coverage

* ~70–75% (focused on business logic)

### What is tested

* Task creation
* Update authorization
* Delete logic (user vs admin)
* Not-found scenarios

---

## 📂 Project Structure

```text
task_manager/
│
├── Controllers/
├── Services/
├── Interfaces/
├── DTO/
├── Models/
├── Data/
├── Middleware/
├── Program.cs
└── appsettings.json

task_manager.Tests/
```

---

## 🔄 Request Flow

```text
Client → Controller → Service → Database
                         ↓
                  Business Logic
                         ↓
                DTO Response → Client
```

---

## 🔐 Authentication Flow

1. User logs in
2. Server validates credentials
3. JWT token generated
4. Client sends token in header
5. Backend validates token
6. Claims extracted (UserId, Role)

---

## 🧪 API Endpoints

### Auth

| Method | Endpoint           | Description   |
| ------ | ------------------ | ------------- |
| POST   | /api/auth/register | Register user |
| POST   | /api/auth/login    | Login user    |

### Tasks

| Method | Endpoint        | Description    |
| ------ | --------------- | -------------- |
| GET    | /api/tasks      | Get user tasks |
| POST   | /api/tasks      | Create task    |
| PUT    | /api/tasks/{id} | Update task    |
| DELETE | /api/tasks/{id} | Delete task    |

---

## ⚙️ Technologies Used

* ASP.NET Core (.NET 8)
* Entity Framework Core
* SQL Server
* JWT Authentication
* FluentValidation
* Serilog

---

## 🚀 How to Run

```bash
git clone https://github.com/asutosh99/task-manager
cd task_manager
dotnet ef database update
dotnet run
```

Swagger:

```text
https://localhost:<port>/swagger
```

---

## 🧪 Run Tests

```bash
dotnet test
```

---

## 🔒 Security Practices

* Password hashing (BCrypt)
* JWT token expiration
* Role-based authorization
* Resource-level access control

---

## ⚠️ Known Limitations

* No refresh tokens
* No caching layer
* No frontend yet

---

## 📈 Future Improvements

### Backend

* Refresh tokens
* Redis caching
* Advanced logging (Azure)

### Frontend

* React + TanStack Query
* Protected routes
* Token handling

### AI Features

* Task summarization
* Smart suggestions
* Priority prediction

### DevOps

* Docker
* CI/CD
* Azure deployment

---

## 🧠 Engineering Principles Followed

* Separation of concerns
* Exception-driven flow
* Interface-based design
* Testable services
* Resource-based authorization

---

## 👨‍💻 Author

Built as part of transitioning from:

**React Developer → .NET Full Stack Engineer (AI-focused)**

---

## 🧨 Final Note

This is not a tutorial-level CRUD app.

It focuses on:

* correctness
* security
* architecture
* testability

— not just “making it work”.
