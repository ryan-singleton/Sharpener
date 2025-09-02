// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection.Tests.Models;

public class TestRepository : ITestRepository
{
    public string GetData()
    {
        return "TestRepository";
    }
}
