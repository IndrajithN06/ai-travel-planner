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

## API Endpoints

### Travel Plans

#### GET /api/travelplans
Get all travel plans

#### GET /api/travelplans/public
Get all public travel plans

#### GET /api/travelplans/{id}
Get a specific travel plan by ID

#### GET /api/travelplans/destination/{destination}
Get travel plans by destination

#### GET /api/travelplans/style/{travelStyle}
Get travel plans by travel style

#### GET /api/travelplans/date-range?startDate={date}&endDate={date}
Get travel plans by date range

#### POST /api/travelplans
Create a new travel plan[user specific]

**Request Body:**
```json
{
  "destination": "Paris, France",
  "title": "Romantic Paris Getaway",
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-06-08T00:00:00Z",
  "description": "A romantic week in the City of Light",
  "budget": 5000,
  "travelStyle": "Luxury",
  "groupSize": "Couple",
  "isPublic": true
}
```

#### POST /api/travelplans/generate
Generate an AI-powered travel plan

**Request Body:**
```json
{
  "destination": "Tokyo, Japan",
  "startDate": "2024-07-01T00:00:00Z",
  "endDate": "2024-07-08T00:00:00Z",
  "travelStyle": "Adventure",
  "groupSize": "Solo",
  "budget": 3000,
  "preferences": "Interested in technology and culture"
}
```

#### PUT /api/travelplans/{id}
Update an existing travel plan

#### DELETE /api/travelplans/{id}
Delete a travel plan

### Activities

#### GET /api/travelplans/{id}/activities
Get all activities for a travel plan

#### POST /api/travelplans/{id}/activities
Add an activity to a travel plan

#### GET /api/travelplans/activities/{activityId}
Get a specific activity

#### PUT /api/travelplans/activities/{activityId}
Update an activity

#### DELETE /api/travelplans/activities/{activityId}
Delete an activity

### Accommodations

#### GET /api/travelplans/{id}/accommodations
Get all accommodations for a travel plan

#### POST /api/travelplans/{id}/accommodations
Add an accommodation to a travel plan

#### GET /api/travelplans/accommodations/{accommodationId}
Get a specific accommodation

#### PUT /api/travelplans/accommodations/{accommodationId}
Update an accommodation

#### DELETE /api/travelplans/accommodations/{accommodationId}
Delete an accommodation

### Transportation

#### GET /api/travelplans/{id}/transportations
Get all transportation for a travel plan

#### POST /api/travelplans/{id}/transportations
Add transportation to a travel plan

#### GET /api/travelplans/transportations/{transportationId}
Get a specific transportation

#### PUT /api/travelplans/transportations/{transportationId}
Update transportation

#### DELETE /api/travelplans/transportations/{transportationId}
Delete transportation

## Data Models

### TravelPlan
- `Id`: Unique identifier
- `Destination`: Travel destination
- `Title`: Plan title
- `StartDate`: Trip start date
- `EndDate`: Trip end date
- `Description`: Plan description
- `AIRecommendations`: AI-generated recommendations
- `Budget`: Total budget
- `TravelStyle`: Travel style (Budget, Luxury, Adventure, Cultural)
- `GroupSize`: Group size (Solo, Couple, Family, Group)
- `IsPublic`: Whether the plan is public
- `CreatedDate`: Creation timestamp
- `UpdatedDate`: Last update timestamp

### Activity
- `Id`: Unique identifier
- `TravelPlanId`: Associated travel plan ID
- `Name`: Activity name
- `Description`: Activity description
- `ScheduledDate`: Scheduled date
- `Duration`: Activity duration
- `Location`: Activity location
- `Cost`: Activity cost
- `Category`: Activity category

### Accommodation
- `Id`: Unique identifier
- `TravelPlanId`: Associated travel plan ID
- `Name`: Accommodation name
- `Description`: Accommodation description
- `Address`: Accommodation address
- `CheckInDate`: Check-in date
- `CheckOutDate`: Check-out date
- `CostPerNight`: Cost per night
- `Type`: Accommodation type

### Transportation
- `Id`: Unique identifier
- `TravelPlanId`: Associated travel plan ID
- `Type`: Transportation type
- `Provider`: Service provider
- `FromLocation`: Departure location
- `ToLocation`: Arrival location
- `DepartureTime`: Departure time
- `ArrivalTime`: Arrival time
- `Cost`: Transportation cost
- `Notes`: Additional notes

## Database

The application uses Entity Framework Core with SQL Server. The database will be created automatically when you first run the application.

### Connection String
Update the connection string in `appsettings.json`:
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
