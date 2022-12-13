using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimaJet.Workflow.Core.Runtime;
using WF.Sample.Tests.Mockups;

namespace WF.Sample.Tests.Tests;

[TestClass]
public class VacationRequestTests
{
    public WorkflowRuntime Runtime => TestData.WorkflowRuntime;
    
    [TestMethod]
    [DataRow(1.0)]
    [DataRow(10000.0)]
    [DataRow(100000000000.0)]
    [DataRow(1.9)]
    [DataRow(1.99999)]
    [DataRow(1.999999999999)]
    public async Task SumSavingTest(double sum)
    {
        var document = new TestDocument(TestData.Employees.First(e => e.Name == "SuperUser")) {Sum = Convert.ToDecimal(sum)};
        await Runtime.CreateInstanceAsync(document);

        var instance = await Runtime.GetProcessInstanceAndFillProcessParametersAsync(document.Id);

        var sumParameter = await instance.GetParameterAsync("Sum");
    
        Assert.AreEqual(Convert.ToDecimal(sum), sumParameter.Value);
    }
    
    [TestMethod]
    [DataRow(10, "StartSigning", "Approve", "Paid")]
    [DataRow(1000, "StartSigning", "Approve", "Approve", "Paid")]
    public async Task SchemeRouteTest(int sum, params string[] commandNames)
    {
        var document = new TestDocument(TestData.Employees.First(e => e.Name == "SuperUser")) {Sum = sum};
        await Runtime.CreateInstanceAsync(document);

        foreach (var commandName in commandNames)
        {
            var commands = await Runtime.GetAvailableCommandsAsync(document.Id, document.Author.Id.ToString());
            var command = commands.First(c => c.CommandName == commandName);
            var result = await Runtime.ExecuteCommandAsync(command, document.Author.Id.ToString(), document.Author.Id.ToString());
            Assert.IsTrue(result.WasExecuted);
        }

        var status = await Runtime.GetCurrentStateAsync(document.Id);
        Assert.AreEqual("RequestApproved", status.Name);
    }
    
    [TestMethod]
    [DataRow("BigBoss")]
    [DataRow("Accountant")]
    [DataRow("User")]
    [DataRow("Manager", "Approve", "Reject")]
    [DataRow("Guest")]
    public async Task AvailableCommandsTest(string employee, params string[] commandNames)
    {
        var document = new TestDocument(TestData.Employees.First(e => e.Name == "User"))
        {
            Manager = TestData.Employees.First(e => e.Name == "Manager")
        };
    
        await Runtime.CreateInstanceAsync(document);
    
        var initCommand = (await Runtime.GetAvailableCommandsAsync(document.Id, document.Author.Id.ToString()))
            .First(c => c.CommandName == "StartSigning");
        await Runtime.ExecuteCommandAsync(initCommand, document.Author.Id.ToString(), document.Author.Id.ToString());

        var assignee = TestData.Employees.First(e => e.Name == employee);
        var availableCommandNames = (await Runtime.GetAvailableCommandsAsync(document.Id, assignee.Id.ToString()))
            .Select(c => c.CommandName).ToList();

        var outerJoin = commandNames
            .Union(availableCommandNames).Distinct()
            .Except(commandNames.Intersect(availableCommandNames));
    
        Assert.AreEqual(0, outerJoin.Count());
    }
}