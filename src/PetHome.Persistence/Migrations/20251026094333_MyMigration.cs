using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IsNewsletterSubscribed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Breed = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiresSpecialDiet = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeclawed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiresExtraExercise = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PublicId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_photos_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stays_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PerformedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_stays_StayId",
                        column: x => x.StayId,
                        principalTable: "stays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "owners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_stays_StayId",
                        column: x => x.StayId,
                        principalTable: "stays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StayId",
                table: "Activities",
                column: "StayId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_OwnerId",
                table: "Pets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_photos_PetId",
                table: "photos",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_stays_PetId",
                table: "stays",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OwnerId",
                table: "Transactions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StayId",
                table: "Transactions",
                column: "StayId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "stays");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "owners");
        }
    }
}
