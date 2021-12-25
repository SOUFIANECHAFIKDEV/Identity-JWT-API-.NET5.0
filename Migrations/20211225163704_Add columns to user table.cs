using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityAPI.Migrations
{
    public partial class Addcolumnstousertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3f74cc27-8580-421e-b9bb-238f7ca9f245");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "478b66f2-dcaa-4765-9fa4-59d1c3004bae");

            migrationBuilder.EnsureSchema(
                name: "references");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                schema: "security",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LegalStatusId",
                schema: "security",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "City",
                schema: "references",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalStatus",
                schema: "references",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "references",
                table: "City",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rabat" },
                    { 2, "Casablanca" }
                });

            migrationBuilder.InsertData(
                schema: "references",
                table: "LegalStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Individual" },
                    { 2, "Entreprise" }
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "98383f39-8e04-41b3-bd27-bc504bb9c5e5", "0b985780-69b8-41ae-8606-f7809dbe03bb", "superAdmin", "SUPERADMIN" },
                    { "c6c55cd5-3e53-4315-85dc-973b1fdd6d44", "a7738a85-8347-41d8-ad6e-6fc363028069", "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_CityId",
                schema: "security",
                table: "users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_users_LegalStatusId",
                schema: "security",
                table: "users",
                column: "LegalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_City_CityId",
                schema: "security",
                table: "users",
                column: "CityId",
                principalSchema: "references",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_LegalStatus_LegalStatusId",
                schema: "security",
                table: "users",
                column: "LegalStatusId",
                principalSchema: "references",
                principalTable: "LegalStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_City_CityId",
                schema: "security",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_LegalStatus_LegalStatusId",
                schema: "security",
                table: "users");

            migrationBuilder.DropTable(
                name: "City",
                schema: "references");

            migrationBuilder.DropTable(
                name: "LegalStatus",
                schema: "references");

            migrationBuilder.DropIndex(
                name: "IX_users_CityId",
                schema: "security",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_LegalStatusId",
                schema: "security",
                table: "users");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "98383f39-8e04-41b3-bd27-bc504bb9c5e5");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c6c55cd5-3e53-4315-85dc-973b1fdd6d44");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "security",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LegalStatusId",
                schema: "security",
                table: "users");

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "478b66f2-dcaa-4765-9fa4-59d1c3004bae", "5a5e4dbe-7293-48e5-b808-cc730c5f6964", "superAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3f74cc27-8580-421e-b9bb-238f7ca9f245", "4cb34cea-cef2-4e8b-b2ee-af07e4524fbc", "admin", "ADMIN" });
        }
    }
}
