using System.Data;
using Microsoft.Data.SqlClient;

namespace WF.Sample.Tests.Testcontainers;

public class MssqlScriptExecutor
{
    public MssqlScriptExecutor(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }

    public void Execute(string script)
    {
        if (script.Trim() == String.Empty) return;

        var connection = CreateConnection();
    
        if (connection.State != ConnectionState.Open) connection.Open();
    
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.ExecuteNonQuery();
    }

    public async Task ExecuteFileAsync(string? path)
    {
        if (!File.Exists(path)) return;
        var script = await File.ReadAllTextAsync(path);
        Execute(script);
    }

    protected IDbConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}