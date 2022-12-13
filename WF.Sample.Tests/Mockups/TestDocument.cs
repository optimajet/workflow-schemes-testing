using WF.Sample.Business.Model;

namespace WF.Sample.Tests.Mockups;

public class TestDocument
{
    public TestDocument(Employee author)
    {
        Author = author;
        Manager = author;
        Id = Guid.NewGuid();
        Sum = 100;
    }

    public TestDocument(Document document)
    {
        Id = document.Id;
        Sum = document.Sum;
        Author = document.Author;
        Manager = document.Manager;
    }

    public Guid Id { get; set; }
    public decimal Sum { get; set; }
    public Employee Author { get; set;}
    public Employee Manager { get; set; }

    public Document ToDocument()
    {
        return new Document
        {
            Id = Id,
            Name  = Id.ToString(),
            Comment = String.Empty,
            AuthorId = Author.Id,
            ManagerId = Manager?.Id,
            Sum = Sum,
            State = String.Empty,
            StateName = String.Empty,
            Author = Author,
            Manager = Manager,
        };
    }
}