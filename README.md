# 🥋 AI Sensei (Backend API)

![.NET 10](https://img.shields.io/badge/.NET-10.0-purple?style=flat&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Refactored-success)
![License](https://img.shields.io/badge/License-MIT-blue)

**AI Sensei** is a production-ready Web API that generates personalized learning quizzes using Generative AI.
Built on .NET 10, the project focuses on clean architecture, security, and long-term maintainability rather than rapid prototyping.

> **📜 The Evolution (v1 vs v2)**
> This project is a complete architectural rewrite of the original prototype(v1).
>
> | Feature | Legacy (v1) | **Current (v2)** |
> | :--- | :--- | :--- |
> | **Framework** | .NET 8.0 | **.NET 10.0 (Latest Performance)** |
> | **Architecture** | Tightly Coupled | **Clean 2-Layer Architecture** |
> | **Security** | Basic/Open | **JWT Identity & Data Isolation** |
> | **AI Logic** | Hardcoded | **Interface-Based & Strongly Typed** |
>
> **[🔗 View the Legacy (v1) Project Here](https://github.com/mo-chips/AI-Sensei-v1)**

## 🏗️ Architecture

The solution implements a strict **2-Layer Architecture** to balance separation of concerns with development velocity.

## 1. `AI_Sensei.Core` (The Contract)
* **Role:** Defines *what* the system does.
* **Content:** Pure domain models (`Quiz`, `User`) and strict interfaces (`IQuizGenerator`, `IQuizRepository`).
* **Dependency Rule:** This layer has **zero dependencies**. It does not know about the database, the AI provider, or the web frameworks.

### 2. `AI_Sensei.API` (The Implementation)
* **Role:** Defines *how* the system works.
* **Services:** Concrete implementations of the Core interfaces (e.g., `OpenAIQuizService` using `gpt-4o-mini`, `QuizRepository` using EF Core).
* **Controllers:** Handles HTTP traffic, JWT validation, and Dependency Injection.

### 3. `AI_Sensei.Client` (Frontend)
* *(Coming Soon)* A React application designed to consume this secure API.

## 🚀 Key Features

* **⚡ .NET 10 Performance:** Utilizes the latest runtime optimizations for JSON serialization and async task handling.
* **🤖 AI Integration:** Dynamic content generation via OpenAI, strictly typed to ensure valid JSON responses every time.
* **🔒 Secure Authentication:** Full ASP.NET Core Identity implementation with JWT Bearer tokens.
* **🛡️ Data Isolation:** "Horizontal Partitioning" ensures users can strictly access only their own data.
* **💾 Flexible Persistence:** Built with Entity Framework Core and SQLite for rapid development, ready for SQL Server in production.

## 📋 Prerequisites

* **[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)** (Required)
* An [OpenAI API Key](https://platform.openai.com/)
* Git

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone [https://github.com/your-username/ai-sensei-backend.git](https://github.com/your-username/ai-sensei-backend.git)
cd ai-sensei-backend
```

### 2. Configure Secrets
**Security Notice:** This project does not store API keys in source control. We use the **Secret Manager** tool and **Environment Variables**.

Run the following commands in the `AI_Sensei.API` directory:
```bash
# Initialize User Secrets
dotnet user-secrets init

# Set your JWT Signing Key (Must be min 32 characters)
dotnet user-secrets set "Jwt:Key" "YOUR_SUPER_LONG_AND_SECURE_RANDOM_KEY_HERE"

# Set your OpenAI API Key
Set key as Environment variable: "OPEN_AI_KEY" "sk-..." 
```

### 3. Database Migration
Initialize the SQLite database:
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the API
```bash
dotnet run
```
The API will launch at `http://localhost:5248` or check `launchSettings.json`. Navigate to `/swagger` to test endpoints.

## 📡 API Endpoints

**Authentication**
| Method | Endpoint             | Description                  |
| ------ | -------------------- | ---------------------------- |
| POST   | `/api/auth/register` | Create a new user            |
| POST   | `/api/auth/login`    | Authenticate and receive JWT |

All requests must include: 
Authorization: Bearer <token>
**Quiz (Protected)**
| Method | Endpoint             | Description                          |
| ------ | -------------------- | ------------------------------------ |
| POST   | `/api/quiz/generate` | Generate quiz via AI (not persisted) |
| POST   | `/api/quiz/save`     | Save quiz to user history            |
| GET    | `/api/quiz/getAll`   | Retrieve user quizzes                |
| GET    | `/api/quiz/{id}`     | Get quiz by ID                       |
| DELETE | `/api/quiz/{id}`     | Delete quiz                          |

##🧪 Testing

Testing is planned but not yet implemented.

Current focus:
* Architectural correctness
* Clear domain boundaries
* Secure request handling

## 📄 License
This project is licensed under the MIT License.
```
