namespace WF.Sample.Business.Migrations
{
    public static class MigrationExtensions
    {
        public static string GetEmbeddedPath(string fileName)
        {
            return $"WF.Sample.Business.Scripts.{fileName}";
        }
    }
}
