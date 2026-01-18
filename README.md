# AI Travel Planner 🚀

A comprehensive full-stack AI-powered travel planner and journal built with **ASP.NET Core Web API** (backend) and **Angular** (frontend), featuring JWT authentication and advanced travel planning capabilities.

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

### Connection String
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**Note:** The connection string uses:
- `Trusted_Connection=true` - Windows Authentication (for local development)
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

### Adding New Features
1. Define entities in `Domain/Entities/`
2. Create interfaces in `Domain/Interfaces/`
3. Implement business logic in `Application/Services/`
4. Create repositories in `Infrastructure/Repositories/`
5. Add controllers in `Api/Controllers/`
6. Update database with migrations

## 🧪 Testing

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

## 🔮 Roadmap

- [ ] Angular frontend implementation
- [ ] AI integration for travel recommendations
- [ ] Email verification system
- [ ] Role-based authorization
- [ ] Mobile app development
- [ ] Social sharing features
- [ ] Advanced analytics and reporting
- [ ] Integration with travel APIs (booking, weather, etc.)

---

**Built with ❤️ using ASP.NET Core and Angular**
