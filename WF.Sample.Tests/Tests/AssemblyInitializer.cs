using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WF.Sample.Tests.Tests;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyInitialize]
    public static async Task AssemblyInit(TestContext context)
    {
        await TestData.StartAsync();
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        await TestData.StopAsync();
    }
}