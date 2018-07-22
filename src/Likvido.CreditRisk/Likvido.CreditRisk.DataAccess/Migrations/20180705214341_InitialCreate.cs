using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Likvido.CreditRisk.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RegistrationName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    AddressTwo = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    AddressTwo = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RegistrationSystemId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    DeptorId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    DeletionDate = table.Column<DateTime>(nullable: false),
                    Foundation = table.Column<string>(nullable: true),
                    AdministratorName = table.Column<string>(nullable: true),
                    AdministratorReference = table.Column<string>(nullable: true),
                    AdministratorPhone = table.Column<string>(nullable: true),
                    CreditorName = table.Column<string>(nullable: true),
                    CreditorPhone = table.Column<string>(nullable: true),
                    CreditorReference = table.Column<string>(nullable: true),
                    CompanyDataId = table.Column<Guid>(nullable: true),
                    PrivateDataId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_RegistrationCompanies_CompanyDataId",
                        column: x => x.CompanyDataId,
                        principalTable: "RegistrationCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_RegistrationUsers_PrivateDataId",
                        column: x => x.PrivateDataId,
                        principalTable: "RegistrationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CompanyDataId",
                table: "Registrations",
                column: "CompanyDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_PrivateDataId",
                table: "Registrations",
                column: "PrivateDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "RegistrationCompanies");

            migrationBuilder.DropTable(
                name: "RegistrationUsers");
        }
    }
}
