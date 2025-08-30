namespace Sharpener.Injection.Tests.Models;

public class TestRepository : ITestRepository
{
    public string GetData()
    {
        return "TestRepository";
    }
}
