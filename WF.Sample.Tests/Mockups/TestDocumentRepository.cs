using OptimaJet.Workflow.Core.Persistence;
using WF.Sample.Business.DataAccess;
using WF.Sample.Business.Model;

namespace WF.Sample.Tests.Mockups;

public class TestDocumentRepository : IDocumentRepository
{
    public TestDocumentRepository()
    {
        Documents = new List<TestDocument>();
    }

    public List<TestDocument> Documents { get; }

    public Document InsertOrUpdate(Document document)
    {
        var testDocument = Documents.FirstOrDefault(d => d.Id == document.Id);

        if (testDocument != null)
        {
            testDocument.Author = document.Author;
            testDocument.Manager = document.Manager;
            testDocument.Sum = document.Sum;
        }
        else
        {
            Documents.Add(new TestDocument(document));
        }
    
        return Documents.First(d => d.Id == document.Id).ToDocument();
    }

    public List<Document> Get(out int count, int page = 1, int pageSize = 128)
    {
        var paging = new Paging(page, pageSize);

        var result = Documents.Take(paging.PageSize).Skip(paging.SkipCount()).Select(d => d.ToDocument()).ToList();

        count = result.Count;
    
        return result;
    }

    public Document? Get(Guid id, bool loadChildEntities = true)
    {
        return Documents.FirstOrDefault(d => d.Id == id)?.ToDocument();
    }

    //We dont track number in TestDocument so we didn't need to implement this method
    public Document? GetByNumber(int number)
    {
        return null;
    }

    public List<Document> GetByIds(List<Guid> ids)
    {
        return Documents.Where(d => ids.Contains(d.Id)).Select(d => d.ToDocument()).ToList();
    }

    public void Delete(Guid[] ids)
    {
        foreach (var document in Documents)
        {
            if (ids.Contains(document.Id))
            {
                Documents.Remove(document);
            }
        }
    }

    //We dont track state in TestDocument so we didn't need to implement this method
    public void ChangeState(Guid id, string nextState, string nextStateName) { }

    //This method is unused so we didn't need to implement it
    public bool IsAuthorsBoss(Guid documentId, Guid identityId)
    {
        throw new NotImplementedException();
    }

    //This method is unused so we didn't need to implement it
    public IEnumerable<string> GetAuthorsBoss(Guid documentId)
    {
        throw new NotImplementedException();
    }
}