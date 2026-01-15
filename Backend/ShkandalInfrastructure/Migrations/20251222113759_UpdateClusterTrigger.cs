using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkandalInfrastructure.Migrations
{
    public partial class UpdateClusterTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS tr_maintain_cluster_dates ON article;
                DROP FUNCTION IF EXISTS sync_clusters_full_recalc();

                CREATE OR REPLACE FUNCTION sync_clusters_full_recalc()
                RETURNS TRIGGER AS $$
                DECLARE
                    _current_cluster_ts timestamptz;
                BEGIN
                    IF (TG_OP = 'UPDATE' OR TG_OP = 'DELETE') AND OLD.cluster_id IS NOT NULL THEN
                        IF (OLD.is_relevant = TRUE) THEN
                            IF (TG_OP = 'DELETE') 
                                OR (NEW.cluster_id IS DISTINCT FROM OLD.cluster_id)
                                OR (NEW.is_relevant = FALSE) THEN

                                SELECT last_updated_at INTO _current_cluster_ts
                                FROM cluster
                                WHERE id = OLD.cluster_id;

                                IF (OLD.published_at IS NOT DISTINCT FROM _current_cluster_ts) THEN
                                    UPDATE cluster
                                    SET last_updated_at = (
                                        SELECT published_at
                                        FROM article
                                        WHERE cluster_id = OLD.cluster_id
                                        AND is_relevant = TRUE  
                                        ORDER BY published_at DESC
                                        LIMIT 1
                                    )
                                    WHERE id = OLD.cluster_id;
                                END IF;
                            END IF;
                        END IF;
                    END IF;

                    IF (TG_OP = 'INSERT' OR TG_OP = 'UPDATE') AND NEW.cluster_id IS NOT NULL THEN
                        IF (NEW.is_relevant = TRUE) THEN
                            UPDATE cluster
                            SET last_updated_at = NEW.published_at
                            WHERE id = NEW.cluster_id
                            AND (last_updated_at IS NULL OR NEW.published_at > last_updated_at);
                        END IF;
                    END IF;

                    RETURN NULL;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER tr_maintain_cluster_dates
                AFTER INSERT OR UPDATE OR DELETE ON article
                FOR EACH ROW
                EXECUTE FUNCTION sync_clusters_full_recalc();
            """);
            migrationBuilder.Sql("""
                UPDATE cluster c
                SET last_updated_at = a.latest_date
                FROM (
                    SELECT cluster_id, MAX(published_at) as latest_date
                    FROM article
                    WHERE is_relevant = TRUE
                    GROUP BY cluster_id
                ) a
                WHERE c.id = a.cluster_id;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS tr_maintain_cluster_dates ON article;
                DROP FUNCTION IF EXISTS sync_clusters_full_recalc();
            """);
        }
    }
}