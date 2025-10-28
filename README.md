
# 🔐 Uepex API Demo (.NET 9)

A **Web API built with ASP.NET Core 9**, implementing **JWT Authentication**, **Entity Framework Core**, and **BCrypt password hashing** for secure user management.  
This project is part of my **developer portfolio**, showcasing authentication logic, database persistence, clean architecture, and logging best practices. 

## 📚 Technologies

- ⚙️ **.NET 9 / ASP.NET Core Web API**.
- 🗃️ **Entity Framework Core**.  
- 🔐 **JWT Authentication**. 
- 🔑 **BCrypt.Net-Next (for password hashing)**.  
- 🧾 **SQL Server LocalDB / SQLite**.  
- 🧠 **Dependency Injection & Logging**.  
- 🧰 **Swagger (for API documentation and testing)**.

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


## Installation & Setup

1️⃣ Clone this repository:

```bash
  git clone https://github.com/Fer1211/UepexApiDemo.git
cd UepexApiDemo
```

2️⃣ Restore dependencies:
```bash
dotnet restore
```

3️⃣ Run the application:
```bash
dotnet restore
```

3️⃣ Apply migrations (creates the local database):
```bash
dotnet ef database update
```

4️⃣ Run the API:
```bash
dotnet run
```

The API will start on:
```bash
➡️ http://localhost:5003
```

👤 Default User

| Usuario | Contraseña | Rol           |
| ------- | ---------- | ------------- |
| admin   | admin1234  | Administrador |


🧩 This user is created automatically via EF Core seed data during migration.

## 💡 About the Project

Uepex API Demo simulates a secure backend service for authentication and user management.
It demonstrates how to:

Implement secure password hashing
Build clean service-based architecture
Use dependency injection and logging
Manage database seeding and migrations

The project follows best practices for backend development in .NET Core environments and can be used as a base for production-grade APIs.



## ✨ Key Features

Authentication Flow:

Controller: AuthController.cs

Service: UsuarioService.cs

Password hashing & verification via BCrypt.Net.BCrypt

Token generation in AuthController using JwtSecurityTokenHandler

Database & Persistence:

Context: ApplicationDbContext.cs

Entities: Usuario, Estudiante

EF Core used for migrations, seeding, and validation

Logging:

ILogger integrated in both Controller and Service for real-time debug tracing


## 📸 Preview

### 🧠 Project Structure / Estructura del proyecto
![Project Structure](Docs/project_structure.png)

### 🔐 Login Success (Console) / Inicio de sesión exitoso (Consola)
![Login Success (Console)](Docs/login_success_console.png)

### 🧾 Swagger UI (Endpoints) / Interfaz de usuario Swagger (Endpoints)
![Swagger UI (Endpoints)](Docs/swagger_ui.png)

### 🪄 JWT Response / Respuesta JWT
![JWT Response](Docs/jwt_response.png)

### 📗 Export Excel / Exportar Excel
![Exportar Excel](Docs/ExcelExport.png)

### 📄 Export CSV / Exportar CSV
![Exportar CSV](Docs/CSVExport.png)

### 🗄️ Database View (Tables) / Vista base de datos (tablas)
![Database View (Tables)](Docs/database_tables.png)

### 🗂️ DB Diagram / Diagrama DB
![DB Diagram](Docs/database_tables.png)

### 📋 Logs / Servicio de Autenticación
![Logs](Docs/log_detail.png)

## 🤝 Contributions & Suggestions
Contributions, issues, and feature requests are welcome!
Feel free to open an Issue or submit a Pull Request if you’d like to collaborate or suggest improvements.

📩 For feedback, you can reach me through my GitHub profile.

## 📝 License

This project is licensed under the MIT License — see the LICENSE
 file for details.
