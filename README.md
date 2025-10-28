# 🔐 Uepex API Demo (.NET 9)

A **Web API built with ASP.NET Core 9**, implementing **JWT Authentication**, **Entity Framework Core**, and **BCrypt password hashing** for secure user management.  
This project is part of my **developer portfolio**, showcasing authentication logic, database persistence, clean architecture, and logging best practices.  

---

## 🚀 Features

- 🔑 **JWT Authentication** (JSON Web Tokens)  
- 🔒 **Password hashing** using `BCrypt.Net`  
- 🧠 **Entity Framework Core** for ORM and database migrations  
- 🧾 **Seeded Admin User** (`admin / admin1234`)  
- 🗂️ **Structured Architecture** (`Controllers`, `Models`, `Services`, `Data`)  
- 🧰 **Dependency Injection** and `ILogger` for clean service logging  
- 🧩 **Custom Authentication Service** (`UsuarioService`)  
- 🧪 **Swagger UI** for endpoint testing  
- 🛡️ **Role-based access structure** ready for future expansion  

📦 **Project Structure**
UepexApiDemo/
┣ 📁 Controllers/
┣ 📁 Data/
┣ 📁 Models/
┣ 📁 Services/
┣ 📜 Program.cs
┣ 📜 appsettings.json
┣ 📜 appsettings.Development.json
┣ 📜 UepexApiDemo.csproj
┗ 📜 README.md

---

## ⚙️ Installation & Setup

### 1️⃣ Clone the repository
```bash
git clone https://github.com/Fer1211/UepexApiDemo.git
cd UepexApiDemo

2️⃣ Restore dependencies
dotnet restore

3️⃣ Apply migrations (creates the local database)
dotnet ef database update

4️⃣ Run the API
dotnet run


The API will start on
➡️ http://localhost:5003

👤 Default User

| Usuario | Contraseña | Rol           |
| ------- | ---------- | ------------- |
| admin   | admin1234  | Administrador |

🧩 This user is created automatically via EF Core seed data during migration.

📚 Technologies

⚙️ .NET 9 / ASP.NET Core Web API
🗃️ Entity Framework Core
🔐 JWT Authentication
🔑 BCrypt.Net-Next (for secure password hashing)
🧾 SQL Server LocalDB / SQLite
🧠 Dependency Injection & Logging
🧰 Swagger for API documentation and testing

✨ Key Features & Architecture
🔑 Authentication Flow

Controller: AuthController.cs

Service: UsuarioService.cs

Password verification via BCrypt.Net.BCrypt

Token generation in AuthController using JwtSecurityTokenHandler

🗂️ Database & Persistence

Context: ApplicationDbContext.cs

Entities: Usuario, Estudiante

EF Core used for migrations, seeding, and validation

🧾 Logging

ILogger integrated in both Controller and Service for real-time debug tracing

## 📸 Preview

### 🧠 Project Structure
![Estructura del Proyecto](docs/project_structure.png)

### 🔐 Login Success (Console)
![Login Exitoso](docs/login_success_console.png)

### 🧾 Swagger UI (Endpoints)
![Swagger UI](docs/swagger_ui.png)

### 🪄 JWT Response
![JWT Token](docs/jwt_response.png)

### 🗄️ Database View (Usuarios Table)
![Base de Datos](docs/database_tables.png)

### 📋 Logs / Servicio de Autenticación
![Logs Detallados](docs/log_detail.png)

### 📊 Excel Export (Data Output)
![Export Excel](docs/excel_export.png)

### 📄 CSV Export (Data Output)
![Export CSV](docs/csv_export.png)


💡 About the Project

Uepex API Demo simulates a secure backend service for authentication and user management.
It demonstrates how to:

Implement secure password hashing

Build clean service-based architecture

Use dependency injection and logging

Manage database seeding and migrations

The project follows best practices for backend development in .NET Core environments and can be used as a base for production-grade APIs.

🤝 Contributions & Feedback

Contributions, issues, and feature requests are welcome!
Feel free to open an Issue or submit a Pull Request if you’d like to collaborate or suggest improvements.

📩 For feedback, you can reach me through my GitHub profile.

📝 License

This project is licensed under the MIT License — see the LICENSE
 file for details.

## 💬 Closing Note

Thank you for visiting this project!  
This API represents my journey into backend development with ASP.NET Core — combining clean architecture, authentication, and security best practices.  
I hope it helps others learn or inspires new ideas for building robust and maintainable systems.  

Made with 💻, ☕ and a lot of curiosity by  
**Fernando Ramírez**  
🩵 *Backend Developer & Continuous Learner*
