using DotNet.Testcontainers.Configurations;

namespace WF.Sample.Tests.Testcontainers;

public static class Extensions
{
    public static T WithSettings<T>(this T configuration, DbOptions options)
        where T : TestcontainerDatabaseConfiguration
    {
        if (options.Password is not null) configuration.Password = options.Password;
        if (options.Port is not null) configuration.Port = options.Port.Value;
        return configuration;
    }
}