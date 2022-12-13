using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace WF.Sample.Tests.Testcontainers;

public class AzureConfiguration : TestcontainerDatabaseConfiguration
{
    private const string AzureSqlEdgeImage = "mcr.microsoft.com/azure-sql-edge";
    private const int AzureSqlEdgePort = 1433;

    public AzureConfiguration() : this(AzureSqlEdgeImage) { }

    public AzureConfiguration(string image) : base(image, AzureSqlEdgePort)
    {
        Environments["ACCEPT_EULA"] = "Y";
        OutputConsumer = Consume.RedirectStdoutAndStderrToStream(new MemoryStream(), new MemoryStream());
    }

    public override string Database
    {
        get => "master";
        set => throw new NotImplementedException();
    }

    public override string Username
    {
        get => "sa";
        set => throw new NotImplementedException();
    }

    public override string Password
    {
        get => Environments["SA_PASSWORD"];
        set => Environments["SA_PASSWORD"] = value;
    }

    public override IOutputConsumer OutputConsumer { get; }

    public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer()
        .UntilPortIsAvailable(AzureSqlEdgePort)
        .UntilMessageIsLogged(OutputConsumer.Stdout, "SQL Server is now ready for client connections");
}