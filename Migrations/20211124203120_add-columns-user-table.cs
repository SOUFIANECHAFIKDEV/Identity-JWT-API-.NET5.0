using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityAPI.Migrations
{
    public partial class addcolumnsusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "08a876a7-7323-4ee8-acbd-657718419736");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "99bb7555-3a87-48d8-837f-7ce98a717f39");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "security",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "security",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "security",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "security",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "security",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "security",
                table: "users");

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "08a876a7-7323-4ee8-acbd-657718419736", "cc4d153e-163b-4825-b872-3c48e1bd3197", "superAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "99bb7555-3a87-48d8-837f-7ce98a717f39", "9ce7801c-77ed-4586-80e6-38f4400102cb", "admin", "ADMIN" });
        }
    }
}
