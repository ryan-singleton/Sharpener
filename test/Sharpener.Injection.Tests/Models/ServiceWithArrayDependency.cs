// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection.Tests.Models;

public class ServiceWithArrayDependency(ITestService[] services)
{
    public ITestService[] Services { get; } = services;
}
