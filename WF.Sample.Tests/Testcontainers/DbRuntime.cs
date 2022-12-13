using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;

namespace WF.Sample.Tests.Testcontainers;

public class DbRuntime
{
    public DbRuntime(DbOptions options, string[]? scripts = null)
    {
        _options = options;
        _scripts = scripts ?? new string[]{};
    }

    public string Name => _options.Name;
    public string ConnectionString => TestContainer.ConnectionString;
    public AzureDatabase TestContainer => _dbTestContainer 
                                          ?? throw new Exception($"The instance of '{nameof(DbRuntime)}' not initialized");

    public MssqlScriptExecutor ScriptExecutor => new(ConnectionString);

    public async Task StartAsync()
    {
        _dbTestContainer = new TestcontainersBuilder<AzureDatabase>()
            .WithDatabase(new AzureConfiguration().WithSettings(_options))
            .Build();
    
        await _dbTestContainer.StartAsync();
    
        //This method is needed to make sure the database is up and ready to accept our requests.
        EnsureDatabaseReady();

        foreach (var script in _scripts)
        {
            await ScriptExecutor.ExecuteFileAsync(script);
        }
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

    private readonly string[] _scripts;
    private readonly DbOptions _options;
    private AzureDatabase? _dbTestContainer;
}