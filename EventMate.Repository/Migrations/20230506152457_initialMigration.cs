using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventMate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifiedTicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(3353), true, "Cinema", null },
                    { 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(3367), true, "Music", null },
                    { 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(3369), true, "Technology", null },
                    { 4, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(3370), true, "Science", null },
                    { 5, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(3372), true, "Sport", null }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(4636), true, "İstenbul", null },
                    { 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(4640), true, "Ankara", null },
                    { 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(4642), true, "İzmir", null },
                    { 4, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(4643), true, "Samsun", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(7575), true, "Admin", null },
                    { 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(7579), true, "Personnel", null },
                    { 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(7580), true, "Paticipant", null }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Address", "CategoryId", "CityId", "CreatedBy", "CreatedDate", "Description", "EndDate", "IsActive", "IsApproved", "Name", "Quota", "StartDate", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Sample Address", 2, 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6152), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Rock'n Coke", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, "Sample Address", 2, 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6164), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "90'lar Türkçe Pop", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, "Sample Address", 1, 4, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6166), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Stanley Kubrick Sineması", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, "Sample Address", 3, 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6168), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Istanbul Technology and Innovation Meeting", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, "Sample Address", 4, 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6170), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Tesla'nın Dehası", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, "Sample Address", 5, 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(6208), "Sample Description", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Chanpions League Finale Istanbul 23", 1500, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Email", "IsActive", "LastActivity", "Name", "Password", "RefreshToken", "RefreshTokenExpireDate", "RoleId", "Surname", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(9875), "blackPearl@gmail.com", true, null, "Jack", "IMJD2023!*", null, null, 1, "Sparrow", null },
                    { 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(9881), "enesArat@gmail.com", true, null, "Enes", "EA2023!*", null, null, 3, "Arat", null },
                    { 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(9883), "erenArat@gmail.com", true, null, "Eren", "EA2023!*", null, null, 3, "Arat", null }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EventId", "IdentifiedTicketNumber", "IsActive", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(8671), 1, "EA_060522_01_f3e5fac1-4e67-413a-a40f-42c8312976ae", true, null, 2 },
                    { 2, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(8746), 3, "EA_060522_01_0417e8b9-ce3f-4501-90ee-69010b1645e0", true, null, 2 },
                    { 3, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(8750), 6, "EA_060522_01_7c26f7f2-9882-4b92-8c5e-b5e3c2e2a320", true, null, 2 },
                    { 4, "SYSTEM", new DateTime(2023, 5, 6, 18, 24, 57, 328, DateTimeKind.Local).AddTicks(8755), 6, "EA_060522_01_f1e89230-babb-4cbc-abc0-40e5e0e3e643", true, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CityId",
                table: "Events",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventId",
                table: "Ticket",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserId",
                table: "Ticket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
