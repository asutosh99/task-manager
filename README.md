# 🚀 Task Manager API (.NET 8 + React + AI Ready)

## 📌 Overview

This project is a **production-ready backend API** built using **ASP.NET Core (.NET 8)** following clean architecture principles.

It supports:

* Secure authentication (JWT)
* User-based data isolation
* Role-based authorization
* Structured logging
* Clean validation
* Scalable architecture

This backend is designed to be extended into a **full-stack AI-powered application**.

---

## 🧠 Key Features

### 🔐 Authentication & Authorization

* JWT-based authentication
* Password hashing using BCrypt
* Role-based access control (User/Admin)
* Secure endpoints using `[Authorize]`

---

### 👤 User Management

* Register & Login APIs
* Secure password storage (hashed)
* Role support (`User`, `Admin`)

---

### 📋 Task Management

* Create, Update, Delete tasks
* Get tasks (user-specific)
* Pagination
* Filtering
* Sorting

---

### 🧾 API Design

* DTO-based architecture
* Standard API response format
* Clean separation of concerns

---

### 🛡 Validation

* FluentValidation for request validation
* Centralized validation logic
* Automatic model validation

---

### 📊 Logging

* Serilog integration
* Structured logging
* Console + File logging

---

### ⚠️ Error Handling

* Global exception middleware
* Consistent error responses

---

## 🏗 Architecture

```text
Controller → Service → DbContext → Database
```

### Layers:

* **Controllers** → Handle HTTP requests
* **Services** → Business logic
* **DTOs** → Input/Output models
* **Validators** → Input validation
* **Middleware** → Exception handling
* **Models** → Database entities

---

## 📂 Project Structure

```text
task_manager/
│
├── Controllers/
│   ├── TasksController.cs
│   └── AuthController.cs
│
├── Services/
│   ├── TaskService.cs
│   ├── AuthService.cs
│   └── CurrentUserService.cs
│
├── DTO/
│   ├── CreateTaskDto.cs
│   ├── UpdateTaskDto.cs
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── TaskDTO.cs
│   └── Validators/
│       ├── CreateTaskDtoValidator.cs
│       ├── UpdateTaskDtoValidator.cs
│       ├── LoginDtoValidator.cs
│       └── RegisterDtoValidator.cs
│
├── Models/
│   ├── TaskItem.cs
│   └── User.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Middleware/
│   └── ExceptionMiddleware.cs
│
├── Program.cs
└── appsettings.json
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

```text
1. User logs in
2. Server validates credentials
3. JWT token generated
4. Client sends token in header
5. Backend validates token
6. Claims extracted (UserId, Role)
```

---

## 🔑 JWT Example

```http
Authorization: Bearer <your_token>
```

---

## 👤 Current User Access

User info is extracted using:

```csharp
_currentUser.UserId
```

This avoids repeated claim extraction.

---

## 🧪 API Endpoints

### Auth

| Method | Endpoint           | Description   |
| ------ | ------------------ | ------------- |
| POST   | /api/auth/register | Register user |
| POST   | /api/auth/login    | Login user    |

---

### Tasks

| Method | Endpoint        | Description                   |
| ------ | --------------- | ----------------------------- |
| GET    | /api/tasks      | Get all tasks (user-specific) |
| POST   | /api/tasks      | Create task                   |
| PUT    | /api/tasks/{id} | Update task                   |
| DELETE | /api/tasks/{id} | Delete task                   |

---

## 📊 Pagination Example

```http
GET /api/tasks?page=1&pageSize=10
```

---

## 🔍 Filtering & Sorting

```http
GET /api/tasks?status=Completed&sortBy=title&order=desc
```

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

### 1. Clone repository

```bash
git clone (https://github.com/asutosh99/task-manager)
```

---

### 2. Setup database

```bash
dotnet ef database update
```

---

### 3. Run project

```bash
dotnet run
```

---

### 4. Open Swagger

```text
https://localhost:<port>/swagger
```

---

## 🔒 Security Practices

* Passwords hashed using BCrypt
* JWT tokens with expiration
* Role-based authorization
* User data isolation

---

## 📈 Future Improvements

### 🔥 Backend

* Refresh tokens
* Advanced logging (Serilog → Azure)
* Caching (Redis)

---
### currently working on - Al features , Frontend , and Devops
### ⚛ Frontend

* React UI
* Protected routes
* Token handling

---
### 🤖 AI Features

* Task summarization
* Smart suggestions
* Priority prediction
* Natural language task creation

---

### ☁️ DevOps

* Dockerization
* CI/CD pipeline
* Azure deployment
* Application Insights (Telemetry)

---

## 🧠 Learning Highlights

This project demonstrates:

* Clean architecture design
* Secure authentication implementation
* Real-world API development practices
* Scalable backend design

---

## ⚠️ Notes for Future Developers / AI

* Services contain business logic — do not move logic into controllers
* DTOs are strictly for request/response — do not expose models directly
* Validation is handled via FluentValidation — avoid manual validation
* Authentication uses JWT — do not mix with session-based auth
* Always use CurrentUserService for user context

---

## 👨‍💻 Author

Built as part of a journey to become a **Full Stack AI Engineer** using:

* React
* .NET
* AI Integration

---
