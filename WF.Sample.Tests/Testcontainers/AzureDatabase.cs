using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;

namespace WF.Sample.Tests.Testcontainers;

public class AzureDatabase : TestcontainerDatabase
{
    internal AzureDatabase(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger)
    {
    }

    public override string ConnectionString => $"Server={Hostname},{Port};Database={Database};" +
                                               $"User Id={Username};Password={Password};" +
                                               $"TrustServerCertificate=True";
}