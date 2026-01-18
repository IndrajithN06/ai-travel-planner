# AI Travel Planner 🚀

A comprehensive full-stack AI-powered travel planner and journal built with **ASP.NET Core Web API** (backend) and **Angular** (frontend), featuring JWT authentication and advanced travel planning capabilities.

<img width="1907" height="873" alt="image" src="https://github.com/user-attachments/assets/cf8ea879-1dd0-4a02-b3a1-22b0cf371d94" />

<img width="1899" height="869" alt="image" src="https://github.com/user-attachments/assets/f4e1aa6d-a841-41b9-b502-3ba73828b103" />



## Features

- **Travel Plan Management**: Create, read, update, and delete travel plans
- **AI-Powered Generation**: Generate travel plans with AI recommendations
- **Activity Management**: Manage activities within travel plans
- **Accommodation Management**: Handle accommodation bookings
- **Transportation Management**: Manage transportation details
- **Public/Private Plans**: Share travel plans publicly or keep them private
- **Search & Filter**: Search by destination, travel style, and date range

## Architecture

The project follows Clean Architecture principles with the following layers:

- **Domain**: Entities, interfaces, and DTOs
- **Application**: Business logic and services
- **Infrastructure**: Data access and external services
- **API**: Controllers and HTTP endpoints

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB for development)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the backend directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Update the connection string in `appsettings.json` if needed
5. Run the application:
   ```bash
   dotnet run --project AITravelPlanner.Api
   ```

The API will be available at `https://localhost:7001` (or the configured port).

## Database

The application uses Entity Framework Core with SQL Server. The database will be created automatically when you first run the application using `EnsureCreated()`.

### SQL Server Installation

**Before running the application, ensure SQL Server is installed:**

#### Windows:
- **SQL Server Express** (Recommended for development): [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **SQL Server LocalDB** (Lightweight, included with Visual Studio): Already installed if you have Visual Studio
- The default connection string works out-of-the-box with LocalDB/Express

#### Linux/macOS:
- **SQL Server for Linux**: [Installation Guide](https://docs.microsoft.com/sql/linux/sql-server-linux-setup)
- **Docker** (Recommended): `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest`
- Use SQL Authentication connection string (see below)

### Connection String

The default connection string in `appsettings.json` works for **Windows with SQL Server Express/LocalDB**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**For Windows users:** The connection string above should work immediately if SQL Server is installed.

**For Linux/macOS or SQL Authentication:** Update the connection string to use SQL Authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**For SQL Server LocalDB (Windows):** If using LocalDB specifically:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**Connection String Parameters:**
- `Trusted_Connection=true` - Windows Authentication (Windows only)
- `User Id=sa;Password=...` - SQL Authentication (all platforms)
- `MultipleActiveResultSets=true` - Allows multiple queries on the same connection
- `TrustServerCertificate=true` - Trusts server certificate without validation (for local development)

### Database Creation Methods

The application currently uses **`EnsureCreated()`** method (configured in `Program.cs`), which automatically creates the database and tables when the application starts for the first time.

**When to use `EnsureCreated()`:**
- ✅ Development/Prototyping
- ✅ Quick setup without migrations
- ✅ Testing scenarios

**When to use Migrations:**
- ✅ Production environments
- ✅ Version control of database schema changes
- ✅ Team collaboration with schema evolution

**If you prefer using Migrations instead:**
1. Comment out `EnsureCreated()` in `Program.cs`:
   ```csharp
   // context.Database.EnsureCreated();
   ```

2. Create and apply migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
   dotnet ef database update --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
   ```

**⚠️ Important:** `EnsureCreated()` and Migrations should not be used together. Choose one approach based on your needs.

### Setup Steps

#### Install Dependencies
```bash
dotnet restore
```

#### Run the API
```bash
dotnet run --project AITravelPlanner.Api
```

The database will be created automatically on first run.

The API will be available at:
- **API**: `http://localhost:5015`
- **Swagger UI**: `http://localhost:5015/swagger/index.html`

### 3. Frontend Setup (Coming Soon)
```bash
cd frontend
npm install
ng serve
```

## 🔐 Authentication

### JWT Configuration
The API uses JWT tokens for authentication. Configuration is in `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-here-minimum-32-characters",
    "Issuer": "AITravelPlanner",
    "Audience": "AITravelPlannerUsers",
    "ExpirationHours": 24
  }
}
```

### Using Swagger UI
1. Open `http://localhost:5015/swagger/index.html`
2. Register a new user using `POST /api/auth/register`
3. Login using `POST /api/auth/login` to get a JWT token
4. Click the "Authorize" button and enter: `Bearer your_token_here`
5. All protected endpoints will now include the token automatically

## 🔧 Development

### Project Structure
```
ai-travel-planner/
├── backend/                        # Backend API
│   ├── AITravelPlanner.Api/       # API Controllers & Configuration
│   ├── AITravelPlanner.Application/# Business Logic Services
│   ├── AITravelPlanner.Domain/    # Entities & Interfaces
│   ├── AITravelPlanner.Infrastructure/ # Data Access & Repositories
│   └── AITravelPlanner.sln        # Solution file
├── frontend/                      # Angular Frontend (Coming Soon)
├── README.md                      # This file
└── LICENSE                        # License file
```

### Clean Architecture
The backend follows Clean Architecture principles:
- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Business logic and use cases
- **Infrastructure Layer**: Data access and external services
- **API Layer**: Controllers and HTTP handling


### API Testing
Use Swagger UI at `http://localhost:5015/swagger/index.html` for testing all endpoints.

### Database Testing
- SQL Server Management Studio (SSMS)
- Connection string: `Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true`

## 🚀 Deployment

### Backend Deployment
1. Build the project: `dotnet build`
2. Publish: `dotnet publish -c Release`
3. Deploy to Azure App Service, AWS, or other cloud platforms

### Database Deployment
1. Create production database
2. Update connection string
3. Run migrations: `dotnet ef database update`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature-name`
3. Commit changes: `git commit -m 'Add feature'`
4. Push to branch: `git push origin feature-name`
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:
- Create an issue in the GitHub repository
- Check the API documentation at `/swagger`
- Review the backend README in the `backend/` folder

**Built with ❤️ using ASP.NET Core and Angular**
