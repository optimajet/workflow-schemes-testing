using FluentMigrator;

namespace WF.Sample.Business.Migrations
{
    [Migration(2000010)]
    public class Migration2000010CreateObjects : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript(MigrationExtensions.GetEmbeddedPath("CreateObjects.sql"));
        }

        public override void Down()
        {
        }
    }
}
