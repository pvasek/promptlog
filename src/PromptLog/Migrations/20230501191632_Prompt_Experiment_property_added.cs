using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromptLog.Migrations
{
    /// <inheritdoc />
    public partial class Prompt_Experiment_property_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Experiment",
                table: "Prompts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experiment",
                table: "Prompts");
        }
    }
}
