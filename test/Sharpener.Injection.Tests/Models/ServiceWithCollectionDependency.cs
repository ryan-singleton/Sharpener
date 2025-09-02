// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection.Tests.Models;

public class ServiceWithCollectionDependency
{
    public ServiceWithCollectionDependency(IEnumerable<ITestService> services)
    {
        Services = services;
    }

    public IEnumerable<ITestService> Services { get; }
}
