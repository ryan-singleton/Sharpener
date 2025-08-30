namespace Sharpener.Injection.Tests.Models;

public class TestService : ITestService
{
    public string GetMessage()
    {
        return "TestService";
    }
}
