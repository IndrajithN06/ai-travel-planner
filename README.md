# AI Travel Planner

AI Travel Planner is a full-stack travel planning app with:
- **ASP.NET Core Web API** backend (Clean Architecture)
- **Angular 19** frontend
- **JWT authentication**
- **Stripe payment flow** for travel plans

## Tech Stack

### Backend
- .NET 9 / ASP.NET Core Web API
- Entity Framework Core + SQL Server
- JWT bearer authentication
- Stripe .NET SDK

### Frontend
- Angular 19
- Angular Material / CDK
- Stripe.js (`@stripe/stripe-js`)

---

## Repository Structure

```text
ai-travel-planner/
├── backend/
│   ├── AITravelPlanner.Api/              # API host, controllers, DI, auth, Swagger
│   ├── AITravelPlanner.Application/      # Application services/use-cases
│   ├── AITravelPlanner.Domain/           # Entities, DTOs, interfaces
│   ├── AITravelPlanner.Infrastructure/   # EF Core DbContext, migrations, repositories
│   └── AITravelPlanner.sln
├── frontend/                             # Angular client app
├── PAYMENT_SETUP_GUIDE.md
├── DATABASE_MIGRATION_GUIDE.md
└── README.md
```

---

## Current Features

- User registration/login with JWT + refresh token flow
- Protected API endpoints with `[Authorize]`
- Travel plan CRUD
- Travel plan generation endpoint (`/api/travelplans/generate`)
- Nested management for activities, accommodations, and transportation
- Search/filter travel plans by destination, style, and date range
- Stripe payment intent creation + webhook handling
- User payment history endpoints
- Angular UI with auth pages, dashboard, and travel planning pages

---

## Backend Setup

### 1) Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB/SQL Express/full SQL Server)

### 2) Configure API settings
Edit:
- `backend/AITravelPlanner.Api/appsettings.Development.json`
- `backend/AITravelPlanner.Api/appsettings.json`

Required configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AITravelPlannerDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Jwt": {
    "SecretKey": "<your-secret-key>",
    "Issuer": "AITravelPlanner",
    "Audience": "AITravelPlannerUsers",
    "ExpirationHours": 24
  },
  "Stripe": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_...",
    "WebhookSecret": "whsec_...",
    "Currency": "USD"
  }
}
```

### 3) Run backend

```bash
cd backend
dotnet restore
dotnet run --project AITravelPlanner.Api
```

On startup, the app applies pending EF Core migrations automatically (`context.Database.Migrate()`).

Default local URLs (Development profile):
- `http://localhost:5015`
- `https://localhost:7121`

Swagger:
- `https://localhost:7121/swagger`
- `http://localhost:5015/swagger`

---

## Frontend Setup

### 1) Install dependencies

```bash
cd frontend
npm install
```

### 2) Verify environment API URL

`frontend/src/environments/environment.ts` should point to your API (default in repo is):

```ts
apiUrl: 'https://localhost:7121/api'
```

### 3) Run frontend

```bash
npm start
```

Frontend runs at `http://localhost:4200` by default.

---

## Key API Endpoints

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh-token`
- `POST /api/auth/logout`
- `GET /api/auth/me`

### Travel Plans
- `GET /api/travelplans`
- `GET /api/travelplans/public`
- `GET /api/travelplans/{id}`
- `GET /api/travelplans/destination/{destination}`
- `GET /api/travelplans/style/{travelStyle}`
- `GET /api/travelplans/date-range?startDate=...&endDate=...`
- `POST /api/travelplans/CreateTravelPlan`
- `POST /api/travelplans/generate`
- `PUT /api/travelplans/{id}`
- `DELETE /api/travelplans/{id}`

Nested resources:
- Activities: `/api/travelplans/{id}/activities`, `/api/travelplans/activities/{activityId}`
- Accommodations: `/api/travelplans/{id}/accommodations`, `/api/travelplans/accommodations/{accommodationId}`
- Transportations: `/api/travelplans/{id}/transportations`, `/api/travelplans/transportations/{transportationId}`

### Payments
- `POST /api/payment/create-intent`
- `GET /api/payment/{paymentIntentId}/status`
- `GET /api/payment/{paymentId}`
- `GET /api/payment/user/{userId}`
- `GET /api/payment/my-payments`
- `POST /api/payment/webhook` (anonymous for Stripe callbacks)

---

## App Routes (Frontend)

- `/` home
- `/auth/login`, `/auth/signup`
- `/dashboard`
- `/create-plan`
- `/travel/demo`
- `/travel/list`
- `/travel/details/:id`

Most app routes are protected by the Angular auth guard.

---

## Useful Docs in this Repo

- `PAYMENT_SETUP_GUIDE.md`
- `PAYMENT_QUICK_REFERENCE.md`
- `PAYMENT_UI_EXPLANATION.md`
- `PAYMENT_INTEGRATION_PLAN.md`
- `DATABASE_MIGRATION_GUIDE.md`
- `MIGRATION_COMPLETE.md`

---

## License

This project is licensed under the terms in `LICENSE`.
