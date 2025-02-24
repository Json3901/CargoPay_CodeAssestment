using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoPay.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updating_PaymentFee_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PaymentFees",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PaymentFees",
                newName: "UpdatedAt");
        }
    }
}
