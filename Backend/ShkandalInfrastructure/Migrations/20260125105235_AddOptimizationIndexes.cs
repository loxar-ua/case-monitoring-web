using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkandalInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOptimizationIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE INDEX idx_cluster_relevant_view_counter 
            ON cluster (view_counter DESC) 
            WHERE is_relevant = true;
        ");

            migrationBuilder.Sql(@"
            CREATE INDEX idx_article_cluster_published_at 
            ON article (cluster_id, published_at DESC);
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_cluster_relevant_view_counter;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_article_cluster_published_at;");
        }
    }
}
