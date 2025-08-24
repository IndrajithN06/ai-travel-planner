using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AITravelPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TravelPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AIRecommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TravelStyle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPlanId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostPerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPlanId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transportations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPlanId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FromLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ToLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transportations_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TravelPlans",
                columns: new[] { "Id", "AIRecommendations", "Budget", "CreatedDate", "Description", "Destination", "EndDate", "GroupSize", "IsPublic", "StartDate", "Title", "TravelStyle", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, 5000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A romantic week in the City of Light", "Paris, France", new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Couple", true, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Romantic Paris Getaway", "Luxury", null, null },
                    { 2, null, 3000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Exploring the vibrant culture of Tokyo", "Tokyo, Japan", new DateTime(2024, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Solo", true, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tokyo Adventure", "Adventure", null, null }
                });

            migrationBuilder.InsertData(
                table: "Accommodations",
                columns: new[] { "Id", "Address", "CheckInDate", "CheckOutDate", "CostPerNight", "Description", "Name", "TravelPlanId", "Type" },
                values: new object[,]
                {
                    { 1, "15 Place Vendôme, 75001 Paris, France", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 800m, "Luxury hotel in the heart of Paris", "Hotel Ritz Paris", 1, "Hotel" },
                    { 2, "Shibuya, Tokyo, Japan", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, "Traditional Japanese capsule hotel experience", "Capsule Hotel", 2, "Hostel" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "Cost", "Description", "Duration", "Location", "Name", "ScheduledDate", "TravelPlanId" },
                values: new object[,]
                {
                    { 1, "Sightseeing", 30m, "Visit the iconic Eiffel Tower", new TimeSpan(0, 2, 0, 0, 0), "Eiffel Tower, Paris", "Eiffel Tower Visit", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "Culture", 17m, "Explore the world's largest art museum", new TimeSpan(0, 3, 0, 0, 0), "Louvre Museum, Paris", "Louvre Museum", new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, "Sightseeing", 0m, "Experience the world's busiest pedestrian crossing", new TimeSpan(0, 1, 0, 0, 0), "Shibuya, Tokyo", "Shibuya Crossing", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "Transportations",
                columns: new[] { "Id", "ArrivalTime", "Cost", "DepartureTime", "FromLocation", "Notes", "Provider", "ToLocation", "TravelPlanId", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), 800m, new DateTime(2024, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "New York", "Direct flight to Charles de Gaulle Airport", "Air France", "Paris", 1, "Flight" },
                    { 2, new DateTime(2024, 7, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), 1200m, new DateTime(2024, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Los Angeles", "Direct flight to Narita Airport", "Japan Airlines", "Tokyo", 2, "Flight" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_TravelPlanId",
                table: "Accommodations",
                column: "TravelPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TravelPlanId",
                table: "Activities",
                column: "TravelPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_TravelPlanId",
                table: "Transportations",
                column: "TravelPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlans_UserId",
                table: "TravelPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropTable(
                name: "TravelPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
