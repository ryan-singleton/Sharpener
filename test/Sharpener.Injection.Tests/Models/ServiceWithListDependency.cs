// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection.Tests.Models;

public class ServiceWithListDependency
{
    public ServiceWithListDependency(List<ITestService> services)
    {
        Services = services;
    }

    public List<ITestService> Services { get; }
}
