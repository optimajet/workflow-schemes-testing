using WF.Sample.Business.DataAccess;

namespace WF.Sample.Tests.Mockups;

public class TestDataServiceProvider : IDataServiceProvider
{
    T IDataServiceProvider.Get<T>()
    {
        return (T) Get(typeof(T));
    }

    private object Get(Type type)
    {
        return type.Name switch
        {
            nameof(IPersistenceProviderContainer) => new TestPersistenceProviderContainer(),
            nameof(IEmployeeRepository) => _employeeRepository,
            nameof(IDocumentRepository) => _documentsRepository,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private readonly TestEmployeeRepository _employeeRepository = new(TestData.Employees);
    private readonly TestDocumentRepository _documentsRepository = new();
}