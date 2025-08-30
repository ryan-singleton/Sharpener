namespace Sharpener.Injection.Tests.Models;

public class ServiceWithCollectionDependency
{
    public ServiceWithCollectionDependency(IEnumerable<ITestService> services)
    {
        Services = services;
    }

    public IEnumerable<ITestService> Services { get; }
}
