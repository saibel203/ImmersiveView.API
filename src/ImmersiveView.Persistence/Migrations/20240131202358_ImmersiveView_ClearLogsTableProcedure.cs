using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmersiveView.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImmersiveView_ClearLogsTableProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sqlString = @"
IF EXISTS(SELECT * FROM sys.objects WHERE [Object_Id] = OBJECT_ID(N'[dbo].[Logs_Clear]') AND [Type] IN (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE [dbo].[Logs_Clear];
	END;
GO

CREATE PROCEDURE [dbo].[Logs_Clear] AS
	BEGIN
		TRUNCATE TABLE [LogEvents];
	END;
";
            
            migrationBuilder.Sql(sqlString);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
