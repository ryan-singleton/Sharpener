// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection.Tests.Models;

public class TestService : ITestService
{
    public string GetMessage()
    {
        return "TestService";
    }
}
