using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityAPI.Migrations
{
    public partial class IgnoreUserNamecolumnatusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "UserName",
                schema: "security",
                table: "users");

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0cd4d1a7-a6b5-40e0-b3d7-b0b31b593fc9", "c98454d2-763c-4ac1-94b3-e64acf0ea488", "superAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0bfe7e13-35fa-41c3-b574-60678d366aff", "b81cdba5-05e1-4569-b2eb-d4a18d4245f9", "admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0bfe7e13-35fa-41c3-b574-60678d366aff");

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0cd4d1a7-a6b5-40e0-b3d7-b0b31b593fc9");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "security",
                table: "users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "98383f39-8e04-41b3-bd27-bc504bb9c5e5", "0b985780-69b8-41ae-8606-f7809dbe03bb", "superAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c6c55cd5-3e53-4315-85dc-973b1fdd6d44", "a7738a85-8347-41d8-ad6e-6fc363028069", "admin", "ADMIN" });
        }
    }
}
