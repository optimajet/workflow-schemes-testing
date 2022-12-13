using OptimaJet.Workflow.Core.Runtime;
using WF.Sample.Business.DataAccess;
using WF.Sample.Tests.Mockups;

namespace WF.Sample.Tests.Tests;

public static class Extensions
{
    public static async Task CreateInstanceAsync(this WorkflowRuntime runtime, TestDocument document)
    {
        var documents = TestData.DataServiceProvider.Get<IDocumentRepository>();
        documents.InsertOrUpdate(document.ToDocument());
    
        await runtime.CreateInstanceAsync(TestData.SchemeCode, document.Id);
    }
}