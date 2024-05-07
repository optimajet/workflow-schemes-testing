using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.DbPersistence;
using OptimaJet.Workflow.Migrator;
using WF.Sample.Business.Migrations;

namespace WF.Sample.Tests.Testcontainers;

public class DbRuntime
{
    public DbRuntime(DbOptions options)
    {
        _options = options;
    }

    public string Name => _options.Name;
    public string ConnectionString => TestContainer.ConnectionString;
    public AzureDatabase TestContainer => _dbTestContainer 
                                          ?? throw new Exception($"The instance of '{nameof(DbRuntime)}' not initialized");
    
    public async Task StartAsync()
    {
        _dbTestContainer = new TestcontainersBuilder<AzureDatabase>()
            .WithDatabase(new AzureConfiguration().WithSettings(_options))
            .Build();
    
        await _dbTestContainer.StartAsync();
    
        //This method is needed to make sure the database is up and ready to accept our requests.
        EnsureDatabaseReady();

        InitDatabase(ConnectionString);
    }

    public async Task StopAsync()
    {
        await TestContainer.DisposeAsync().AsTask();
    }

    private void EnsureDatabaseReady()
    {
        for (var i = 0; i < 300; i++)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();
                return;
            }
            catch (Exception)
            {
                Thread.Sleep(100);
            }
        }
    }
    private static void InitDatabase(string connectionString)
    {
        var mssqlProvider = new MSSQLProvider(connectionString);
        new WorkflowRuntime()
            .WithPersistenceProvider(mssqlProvider)
            .RunMigrations()
            .RunCustomMigration(typeof(Migration2000010CreateObjects).Assembly);
    }

    private readonly DbOptions _options;
    private AzureDatabase? _dbTestContainer;
}