namespace Sharpener.Injection.Tests.Models;

public class ServiceWithArrayDependency(ITestService[] services)
{
    public ITestService[] Services { get; } = services;
}
