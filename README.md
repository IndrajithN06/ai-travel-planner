# AI Travel Planner 🚀

A comprehensive full-stack AI-powered travel planner and journal built with **ASP.NET Core Web API** (backend) and **Angular** (frontend), featuring JWT authentication and advanced travel planning capabilities.

## 🌟 Features

### 🔐 Authentication & Authorization
- **JWT (JSON Web Token) Authentication**
- User registration and login
- Password hashing with SHA256
- Token refresh mechanism
- Protected API endpoints
- User profile management

### 🗺️ Travel Planning
- **AI-powered travel recommendations**
- Create and manage travel plans
- Destination-based planning
- Budget tracking and management
- Travel style customization (Luxury, Adventure, Budget, etc.)
- Group size considerations

### 📋 Comprehensive Travel Management
- **Activities**: Sightseeing, cultural experiences, adventures
- **Accommodations**: Hotels, hostels, vacation rentals
- **Transportation**: Flights, trains, buses, car rentals
- **Scheduling**: Date-based planning and coordination
- **Cost tracking**: Per-item and total budget management

### 🎯 User Experience
- **Public and Private Plans**: Share or keep plans private
- **Detailed Planning**: Comprehensive activity and accommodation details
- **Flexible Scheduling**: Customizable dates and durations
- **Location-based Services**: Address and location tracking

## 🏗️ Architecture

### Backend (ASP.NET Core Web API)
```
backend/
├── AITravelPlanner.Api/           # API Layer (Controllers, Middleware)
├── AITravelPlanner.Application/   # Business Logic Layer (Services)
├── AITravelPlanner.Domain/        # Domain Layer (Entities, Interfaces)
└── AITravelPlanner.Infrastructure/# Data Access Layer (DbContext, Repositories)
```

### Frontend (Angular)
```
frontend/                          # Angular Application (Coming Soon)
```

## 🛠️ Technology Stack

### Backend
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Database
- **JWT Bearer Tokens** - Authentication
- **Swagger/OpenAPI** - API documentation
- **CORS** - Cross-origin resource sharing

### Frontend (Planned)
- **Angular 17+** - Frontend framework
- **TypeScript** - Programming language
- **Angular Material** - UI components
- **RxJS** - Reactive programming

## 📋 Prerequisites

- **.NET 9.0 SDK**
- **SQL Server** (Local or Remote)
- **Visual Studio 2022** or **VS Code**
- **Node.js** (for frontend development)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ai-travel-planner.git
cd ai-travel-planner
```

### 2. Backend Setup

#### Navigate to Backend Directory
```bash
cd backend
```

#### Configure Database Connection
Update the connection string in `AITravelPlanner.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### Install Dependencies
```bash
dotnet restore
```

#### Create Database and Apply Migrations
```bash
dotnet ef database update --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
```

#### Run the API
```bash
dotnet run --project AITravelPlanner.Api
```

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

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `POST /api/auth/refresh-token` - Refresh JWT token
- `POST /api/auth/logout` - Logout user
- `GET /api/auth/me` - Get current user info
- `PUT /api/auth/me` - Update current user
- `DELETE /api/auth/me` - Delete current user
- `POST /api/auth/change-password` - Change password
- `POST /api/auth/forgot-password` - Forgot password
- `POST /api/auth/reset-password` - Reset password

### Travel Plan Endpoints (All require authentication)
- `GET /api/travelplans` - Get all travel plans
- `POST /api/travelplans` - Create new travel plan
- `GET /api/travelplans/{id}` - Get travel plan by ID
- `PUT /api/travelplans/{id}` - Update travel plan
- `DELETE /api/travelplans/{id}` - Delete travel plan
- `GET /api/travelplans/public` - Get public travel plans
- `POST /api/travelplans/{id}/generate-ai` - Generate AI recommendations

### Activity Endpoints
- `GET /api/travelplans/{id}/activities` - Get activities for travel plan
- `POST /api/travelplans/{id}/activities` - Add activity to travel plan
- `PUT /api/travelplans/{id}/activities/{activityId}` - Update activity
- `DELETE /api/travelplans/{id}/activities/{activityId}` - Delete activity

### Accommodation Endpoints
- `GET /api/travelplans/{id}/accommodations` - Get accommodations for travel plan
- `POST /api/travelplans/{id}/accommodations` - Add accommodation to travel plan
- `PUT /api/travelplans/{id}/accommodations/{accommodationId}` - Update accommodation
- `DELETE /api/travelplans/{id}/accommodations/{accommodationId}` - Delete accommodation

### Transportation Endpoints
- `GET /api/travelplans/{id}/transportations` - Get transportations for travel plan
- `POST /api/travelplans/{id}/transportations` - Add transportation to travel plan
- `PUT /api/travelplans/{id}/transportations/{transportationId}` - Update transportation
- `DELETE /api/travelplans/{id}/transportations/{transportationId}` - Delete transportation

## 🗄️ Database Schema

### Users Table
- `Id` (Primary Key)
- `FirstName`, `LastName`
- `Email` (Unique)
- `PasswordHash`
- `PhoneNumber`, `Country`, `City`
- `DateOfBirth`, `Gender`
- `IsEmailVerified`, `IsActive`
- `CreatedDate`, `LastLoginDate`, `UpdatedDate`

### TravelPlans Table
- `Id` (Primary Key)
- `Destination`, `Title`
- `StartDate`, `EndDate`
- `Description`, `AIRecommendations`
- `Budget`, `TravelStyle`, `GroupSize`
- `IsPublic`
- `UserId` (Foreign Key to Users)
- `CreatedDate`, `UpdatedDate`

### Activities Table
- `Id` (Primary Key)
- `TravelPlanId` (Foreign Key)
- `Name`, `Description`, `Location`
- `ScheduledDate`, `Duration`
- `Cost`, `Category`

### Accommodations Table
- `Id` (Primary Key)
- `TravelPlanId` (Foreign Key)
- `Name`, `Description`, `Address`
- `CheckInDate`, `CheckOutDate`
- `CostPerNight`, `Type`

### Transportations Table
- `Id` (Primary Key)
- `TravelPlanId` (Foreign Key)
- `Type`, `Provider`
- `FromLocation`, `ToLocation`
- `DepartureTime`, `ArrivalTime`
- `Cost`, `Notes`

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
