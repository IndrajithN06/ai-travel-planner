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

## Database Setup (Local Development)

The application uses **Entity Framework Core** with **SQL Server**. The database will be created automatically when you first run the application.

### Step 1: Install SQL Server (Choose based on your OS)

#### 🪟 Windows:

**Option 1: SQL Server LocalDB (Recommended for development)**
- ✅ **Included with Visual Studio** - No separate installation needed
- ✅ Lightweight and perfect for local development
- ✅ Database files stored in user profile

**Option 2: SQL Server Express (Full installation)**
- 📥 [Download SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- Full-featured SQL Server for local development

#### 🐧 Linux / 🍎 macOS:

**Option 1: Docker (Recommended)**
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 -d \
  --name sqlserver \
  mcr.microsoft.com/mssql/server:2022-latest
```

**Option 2: SQL Server for Linux**
- 📖 [Installation Guide](https://docs.microsoft.com/sql/linux/sql-server-linux-setup)

### Step 2: Configure Connection String

Navigate to `backend/AITravelPlanner.Api/appsettings.json` and update the connection string:

#### For Windows (LocalDB) - Default:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### For Windows (SQL Server Express/Full):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### For Linux/macOS (SQL Authentication):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**💡 Connection String Parameters Explained:**
- `Server=(localdb)\\mssqllocaldb` or `Server=localhost` - SQL Server instance location
- `Database=AITravelPlannerDb` - Database name (will be created automatically)
- `Trusted_Connection=true` - Windows Authentication (Windows only)
- `User Id=sa;Password=...` - SQL Authentication (Linux/macOS/Docker)
- `MultipleActiveResultSets=true` - Allows multiple queries on same connection
- `TrustServerCertificate=true` - Trusts certificate without validation (local dev only)

### Step 3: Run the Application

The database will be **automatically created** when you first run the application.

1. **Navigate to backend directory:**
   ```bash
   cd backend
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Run the API:**
   ```bash
   dotnet run --project AITravelPlanner.Api
   ```

4. **✅ Database Creation:**
   - On first run, `EnsureCreated()` method (in `Program.cs`) automatically creates:
     - Database: `AITravelPlannerDb`
     - All tables: Users, TravelPlans, Activities, Accommodations, Transportations, etc.
   - No manual database setup required! 🎉

### Database Creation Method

**Current Setup:** The application uses **`EnsureCreated()`** (configured in `Program.cs`), which automatically creates the database and all tables on first application startup.

**✅ Perfect for:**
- Local development
- Quick prototyping
- Testing scenarios
- Personal projects

**🔄 Alternative - Using Migrations (Optional):**

If you prefer using Entity Framework Migrations instead:

1. Comment out `EnsureCreated()` in `backend/AITravelPlanner.Api/Program.cs`:
   ```csharp
   // Ensure database is created
   // using (var scope = app.Services.CreateScope())
   // {
   //     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
   //     context.Database.EnsureCreated();
   // }
   ```

2. Create and apply initial migration:
   ```bash
   cd backend
   dotnet ef migrations add InitialCreate --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
   dotnet ef database update --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
   ```

**⚠️ Note:** `EnsureCreated()` and Migrations should **not** be used together. Choose one approach based on your needs.

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
