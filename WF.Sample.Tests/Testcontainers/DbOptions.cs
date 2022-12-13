namespace WF.Sample.Tests.Testcontainers;

public class DbOptions
{
    public DbOptions(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public int? Port { get; set; }
    public string? Password { get; set; }
}