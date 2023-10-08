using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPATIUM_backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserProjectManagment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_AspNetUsers_UserId1",
                table: "UserProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_Projects_ProjectId1",
                table: "UserProjects");

            migrationBuilder.DropIndex(
                name: "IX_UserProjects_ProjectId1",
                table: "UserProjects");

            migrationBuilder.DropIndex(
                name: "IX_UserProjects_UserId1",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserProjects");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "UserProjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "UserProjects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "UserProjects");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "UserProjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserProjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProjectId1",
                table: "UserProjects",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_UserId1",
                table: "UserProjects",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_AspNetUsers_UserId1",
                table: "UserProjects",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_Projects_ProjectId1",
                table: "UserProjects",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
