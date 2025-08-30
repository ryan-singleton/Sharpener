namespace Sharpener.Injection.Tests.Models;

public class AlternativeTestService : ITestService
{
    public string GetMessage()
    {
        return "AlternativeTestService";
    }
}
