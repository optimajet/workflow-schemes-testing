using Newtonsoft.Json;
using OptimaJet.Workflow.Core.Runtime;
using WF.Sample.Business.DataAccess;
using WF.Sample.Business.Model;
using WF.Sample.Business.Workflow;
using WF.Sample.Tests.Mockups;
using WF.Sample.Tests.Testcontainers;

namespace WF.Sample.Tests;

public static class TestData
{
    static TestData()
    {
        var employeesJson = File.ReadAllText(Path.Combine(ConfigPath, EmployeesConfigName));
        var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson) 
                        ?? throw new InvalidOperationException("Json deserialization error.");

        Employees = employees;
    
        var optionsJson = File.ReadAllText(Path.Combine(ConfigPath, OptionsConfigName));
        var options = JsonConvert.DeserializeObject<DbOptions>(optionsJson) 
                      ?? throw new InvalidOperationException("Json deserialization error.");

    
        _dbRuntime = new DbRuntime(options);
    }

    public static async Task StartAsync()
    {
        await _dbRuntime.StartAsync();
        WorkflowInit.Create(new TestDataServiceProvider());
        WorkflowRuntime = WorkflowInit.Runtime;
    }

    public static async Task StopAsync()
    {
        await _dbRuntime.StopAsync();
    }

    public static WorkflowRuntime WorkflowRuntime
    {
        get => _workflowRuntime ?? throw new NullReferenceException("WorkflowRuntime not initialized.");
        private set => _workflowRuntime = value;
    }

    public static IDataServiceProvider DataServiceProvider => WorkflowInit.DataServiceProvider;
    
    public static string ConnectionString => _dbRuntime.ConnectionString;
    public static List<Employee> Employees { get; }
    
    //The SchemeCode is taken from init scripts for the database
    public const string SchemeCode = "SimpleWF";

    public const string ConfigPath = "./configs";
    public const string OptionsConfigName = "options.json";
    private const string EmployeesConfigName = "employees.json";
    

    private static readonly DbRuntime _dbRuntime;
    private static WorkflowRuntime? _workflowRuntime;
}