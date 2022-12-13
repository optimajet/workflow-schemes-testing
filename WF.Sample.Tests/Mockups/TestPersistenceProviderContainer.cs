using OptimaJet.Workflow.Core.Persistence;
using OptimaJet.Workflow.DbPersistence;
using WF.Sample.Business.DataAccess;

namespace WF.Sample.Tests.Mockups;

public class TestPersistenceProviderContainer : IPersistenceProviderContainer
{
    public TestPersistenceProviderContainer()
    {
        Provider = new MSSQLProvider(TestData.ConnectionString);
    }

    public IWorkflowProvider Provider { get; }
}