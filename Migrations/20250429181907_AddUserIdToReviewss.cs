using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLab.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToReviewss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "admin@example.com", "$2a$12$6A0e4QnH.D5GgFq8Y8JMYoV6t7q.PpLaUNeI3RxiXxgmbwOrrzZ6y", "Admin", "admin" });
        }
    }
}
