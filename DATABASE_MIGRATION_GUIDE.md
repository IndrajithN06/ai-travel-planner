# Database Migration Guide for Payment Tables

## Step-by-Step Instructions

### Step 1: Stop the Running API

**Important:** You must stop the running API process before creating migrations.

1. **If running in Visual Studio:**
   - Stop the debugger (Shift + F5)
   - Or close the application

2. **If running in terminal:**
   - Press `Ctrl + C` in the terminal where the API is running
   - Or close the terminal window

3. **If running as a service:**
   - Stop the service using Windows Services or your process manager

### Step 2: Create the Migration

Open a new terminal/command prompt and run:

```bash
cd D:\AITravelPlanner\ai-travel-planner\backend
dotnet ef migrations add AddPaymentTables --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
```

This will create a new migration file in:
`AITravelPlanner.Infrastructure/Migrations/`

### Step 3: Review the Migration (Optional)

The migration file will be created with a timestamp. You can review it to ensure it includes:
- `Payments` table
- `PaymentTransactions` table
- `StripeCustomerId` column in `Users` table
- All indexes and foreign keys

### Step 4: Apply the Migration

Apply the migration to your database:

```bash
dotnet ef database update --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
```

This will:
- Create the `Payments` table
- Create the `PaymentTransactions` table
- Add `StripeCustomerId` column to `Users` table
- Create all indexes and foreign key relationships

### Step 5: Verify the Migration

You can verify the migration was successful by:

1. **Check SQL Server:**
   ```sql
   USE AITravelPlannerDb;
   SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Payments', 'PaymentTransactions');
   SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'StripeCustomerId';
   ```

2. **Or check in SQL Server Management Studio:**
   - Connect to your database
   - Expand Tables
   - You should see `Payments` and `PaymentTransactions` tables
   - Check `Users` table has `StripeCustomerId` column

### Step 6: Start the API

After migration is complete, start your API again:

```bash
cd D:\AITravelPlanner\ai-travel-planner\backend
dotnet run --project AITravelPlanner.Api
```

The API will now use `Database.Migrate()` instead of `EnsureCreated()`, which is better for production.

---

## Alternative: If You Prefer EnsureCreated()

If you want to keep using `EnsureCreated()` (simpler for development), you can:

1. **Revert the Program.cs change:**
   ```csharp
   // Use EnsureCreated instead of Migrate
   context.Database.EnsureCreated();
   // context.Database.Migrate(); // Commented out
   ```

2. **Delete the migration files** (if created):
   - Delete the migration files from `Migrations/` folder
   - Keep `ApplicationDbContextModelSnapshot.cs`

3. **Run the application:**
   - The database will auto-create on first run
   - All tables including Payments will be created automatically

---

## Troubleshooting

### Error: "Process cannot access the file"
- **Solution:** Stop the running API process first

### Error: "Migration already exists"
- **Solution:** Delete the existing migration files or use a different name:
  ```bash
  dotnet ef migrations add AddPaymentTablesV2 --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api
  ```

### Error: "Cannot connect to database"
- **Solution:** Check your connection string in `appsettings.json`
- Ensure SQL Server is running
- Verify database exists or can be created

### Error: "Table already exists"
- **Solution:** The database might already have the tables from `EnsureCreated()`
- You can either:
  1. Drop and recreate the database (development only!)
  2. Or manually add the missing columns/tables
  3. Or use `EnsureCreated()` instead of migrations

---

## Quick Commands Summary

```bash
# 1. Stop API (Ctrl+C or close terminal)

# 2. Create migration
cd D:\AITravelPlanner\ai-travel-planner\backend
dotnet ef migrations add AddPaymentTables --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api

# 3. Apply migration
dotnet ef database update --project AITravelPlanner.Infrastructure --startup-project AITravelPlanner.Api

# 4. Start API
dotnet run --project AITravelPlanner.Api
```

---

## What Gets Created

### Payments Table
- `Id` (Primary Key)
- `TravelPlanId` (Foreign Key)
- `UserId` (Foreign Key)
- `StripePaymentIntentId` (Unique Index)
- `StripeCustomerId`
- `Amount`, `Currency`, `Status`
- `PaymentMethod`, `Description`, `ReceiptUrl`
- `CreatedDate`, `CompletedDate`

### PaymentTransactions Table
- `Id` (Primary Key)
- `PaymentId` (Foreign Key)
- `StripeEventId` (Unique Index)
- `EventType`, `Status`
- `RawData` (JSON)
- `CreatedDate`

### Users Table Update
- `StripeCustomerId` column added

---

**Note:** The migration approach is recommended for production, while `EnsureCreated()` is fine for development and quick prototyping.
