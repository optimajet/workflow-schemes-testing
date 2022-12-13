using OptimaJet.Workflow.Core.Persistence;
using WF.Sample.Business.DataAccess;
using WF.Sample.Business.Model;

namespace WF.Sample.Tests.Mockups;

public class TestEmployeeRepository : IEmployeeRepository
{
    public TestEmployeeRepository(List<Employee> employees)
    {
        Employees = employees;
    }

    public const string Unknown = "Unknown";
    public List<Employee> Employees { get; }

    public List<Employee> GetAll()
    {
        return new List<Employee>(Employees);
    }

    public string GetNameById(Guid id)
    {
        return Employees.FirstOrDefault(e => e.Id == id)?.Name ?? Unknown;
    }

    public IEnumerable<string> GetInRole(string roleName)
    {
        return Employees
            .Where(e => e.EmployeeRoles.Any(r => r.Role.Name == roleName))
            .Select(e => e.Id.ToString());
    }

    public bool CheckRole(Guid employeeId, string roleName)
    {
        return Employees.Any(e => e.Id == employeeId && e.EmployeeRoles.Any(r => r.Role.Name == roleName));
    }

    public List<Employee> GetWithPaging(string? userName = null, SortDirection sortDirection = SortDirection.Asc, Paging? paging = null)
    {
        var result = new List<Employee>(Employees);

        if (userName != null)
        {
            result = result.Where(e => e.Name.Contains(userName)).ToList();
        }

        if (sortDirection == SortDirection.Desc)
        {
            result = result.OrderByDescending(e => e.Name).ToList();
        }
        else
        {
            result = result.OrderBy(e => e.Name).ToList();
        }

        if (paging != null)
        {
            result = result.Skip(paging.SkipCount()).Take(paging.PageSize).ToList();
        }

        return result;
    }
}