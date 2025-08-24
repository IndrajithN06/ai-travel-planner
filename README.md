# AI Travel Planner Backend API

A comprehensive ASP.NET Core Web API for AI-powered travel planning with clean architecture.

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
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Swagger Documentation

When running in development mode, Swagger UI is available at:
`https://localhost:7001/swagger`

## CORS

The API is configured to allow CORS from any origin for development. In production, you should configure specific origins.

## Future Enhancements

- User authentication and authorization
- Real AI integration for recommendations
- Weather API integration
- Currency conversion
- Booking integration
- Image upload for travel plans
- Social features (likes, comments, sharing)
- Mobile app support

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License. 
