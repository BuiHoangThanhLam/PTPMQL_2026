using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_BenhVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "BenhViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "BenhViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "BenhViens",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TinhTrang",
                table: "BenhViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "BenhViens");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "BenhViens");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "BenhViens");

            migrationBuilder.DropColumn(
                name: "TinhTrang",
                table: "BenhViens");
        }
    }
}
