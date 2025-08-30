using Sharpener.Extensions;
using Sharpener.Injection.Tests.Models;

namespace Sharpener.Injection.Tests;

public class ServiceResolverTests
{
    private readonly ServiceResolver _resolver = new();

    [Fact]
    public void GetRegisteredNames_WithMixedRegistrations_ReturnsOnlyNamed()
    {
        _resolver.Register<ITestService, TestService>();
        _resolver.Register<ITestService, AlternativeTestService>("named");

        var names = _resolver.GetRegisteredNames<ITestService>().ToArray();

        names.ShouldContain("named");
        names.Length.ShouldBe(1);
    }

    [Fact]
    public void GetRegisteredNames_WithNamedRegistrations_ReturnsNames()
    {
        _resolver.Register<ITestService, TestService>("service1");
        _resolver.Register<ITestService, AlternativeTestService>("service2");

        var names = _resolver.GetRegisteredNames<ITestService>().ToArray();

        names.ShouldContain("service1");
        names.ShouldContain("service2");
        names.Length.ShouldBe(2);
    }

    [Fact]
    public void GetRegisteredNames_WithoutNamedRegistrations_ReturnsEmpty()
    {
        var names = _resolver.GetRegisteredNames<ITestService>();

        names.ShouldBeEmpty();
    }

    [Fact]
    public void IsRegistered_ForNamedRegisteredService_ReturnsTrue()
    {
        _resolver.Register<ITestService, TestService>("test");

        _resolver.IsRegistered<ITestService>("test").ShouldBeTrue();
    }

    [Fact]
    public void IsRegistered_ForNamedUnregisteredService_ReturnsFalse()
    {
        _resolver.IsRegistered<ITestService>("test").ShouldBeFalse();
    }

    [Fact]
    public void IsRegistered_ForRegisteredService_ReturnsTrue()
    {
        _resolver.Register<ITestService, TestService>();

        _resolver.IsRegistered<ITestService>().ShouldBeTrue();
    }

    [Fact]
    public void IsRegistered_ForUnregisteredService_ReturnsFalse()
    {
        _resolver.IsRegistered<ITestService>().ShouldBeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void IsRegistered_NamedWithNullOrEmptyName_ThrowsArgumentNullException(string? name)
    {
        var action = () => { _ = _resolver.IsRegistered<ITestService>(name!); };

        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Register_ConcreteClass_RegistersSuccessfully()
    {
        var result = _resolver.Register<TestService>();

        result.ShouldBe(_resolver);
        _resolver.IsRegistered<TestService>().ShouldBeTrue();
    }

    [Fact]
    public void Register_NamedWithoutOverwrite_DoesNotOverwriteExisting()
    {
        _resolver.Register<ITestService, TestService>("test");
        _resolver.Register<ITestService, AlternativeTestService>("test");

        var instance = _resolver.Resolve<ITestService>("test");
        instance.GetMessage().ShouldBe("TestService");
    }

    [Fact]
    public void Register_NamedWithOverwrite_OverwritesExisting()
    {
        _resolver.Register<ITestService, TestService>("test");
        _resolver.Register<ITestService, AlternativeTestService>("test", true);

        var instance = _resolver.Resolve<ITestService>("test");
        instance.GetMessage().ShouldBe("AlternativeTestService");
    }

    [Fact]
    public void Register_TransientService_RegistersSuccessfully()
    {
        var result = _resolver.Register<ITestService, TestService>();

        result.ShouldBe(_resolver);
        _resolver.IsRegistered<ITestService>().ShouldBeTrue();
    }

    [Fact]
    public void Register_WithConfigure_AppliesConfiguration()
    {
        var testClass = new TestClass("Test", "42");
        _resolver.RegisterSingleton(testClass, configure: test => test.Value = "45");
        var resolved = _resolver.Resolve<TestClass>();
        resolved.Value.ShouldBe("45");
    }

    [Fact]
    public void Register_WithName_RegistersSuccessfully()
    {
        var result = _resolver.Register<ITestService, TestService>("test");

        result.ShouldBe(_resolver);
        _resolver.IsRegistered<ITestService>("test").ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Register_WithNullOrEmptyName_ThrowsArgumentNullException(string? name)
    {
        var action = () => _resolver.Register<ITestService, TestService>(name!);

        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Register_WithoutOverwrite_DoesNotOverwriteExisting()
    {
        _resolver.Register<ITestService, TestService>();
        _resolver.Register<ITestService, AlternativeTestService>();

        var instance = _resolver.Resolve<ITestService>();
        instance.GetMessage().ShouldBe("TestService");
    }

    [Fact]
    public void Register_WithOverwrite_OverwritesExisting()
    {
        _resolver.Register<ITestService, TestService>();
        _resolver.Register<ITestService, AlternativeTestService>(true);

        var instance = _resolver.Resolve<ITestService>();
        instance.GetMessage().ShouldBe("AlternativeTestService");
    }

    [Fact]
    public void RegisterSingleton_ConcreteClass_RegistersSuccessfully()
    {
        var result = _resolver.RegisterSingleton<TestService>();

        result.ShouldBe(_resolver);
        _resolver.IsRegistered<TestService>().ShouldBeTrue();
    }

    [Fact]
    public void RegisterSingleton_NamedWithInstance_RegistersSuccessfully()
    {
        var instance = new TestService();
        var result = _resolver.RegisterSingleton<ITestService>("test", instance);

        result.ShouldBe(_resolver);
        _resolver.Resolve<ITestService>("test").ShouldBeSameAs(instance);
    }

    [Fact]
    public void RegisterSingleton_NamedWithNullInstance_ThrowsArgumentNullException()
    {
        var action = () => _resolver.RegisterSingleton<ITestService>("test", null!);

        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void RegisterSingleton_NamedWithNullOrEmptyName_ThrowsArgumentNullException(string? name)
    {
        var instance = new TestService();
        var action = () => _resolver.RegisterSingleton<ITestService>(name!, instance);

        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void RegisterSingleton_RegistersSuccessfully()
    {
        var result = _resolver.RegisterSingleton<ITestService, TestService>();

        result.ShouldBe(_resolver);
        _resolver.IsRegistered<ITestService>().ShouldBeTrue();
    }

    [Fact]
    public void RegisterSingleton_ReturnsSameInstance()
    {
        _resolver.RegisterSingleton<ITestService, TestService>();

        var instance1 = _resolver.Resolve<ITestService>();
        var instance2 = _resolver.Resolve<ITestService>();

        instance1.ShouldBeSameAs(instance2);
    }

    [Fact]
    public void RegisterSingleton_WithInstance_RegistersSuccessfully()
    {
        var instance = new TestService();
        var result = _resolver.RegisterSingleton<ITestService>(instance);

        result.ShouldBe(_resolver);
        _resolver.Resolve<ITestService>().ShouldBeSameAs(instance);
    }

    [Fact]
    public void RegisterSingleton_WithInstanceAndConfigure_AppliesConfiguration()
    {
        var instance = new TestService();
        var configured = false;
        _resolver.RegisterSingleton<ITestService>(instance, configure: _ => configured = true);

        configured.ShouldBeTrue();
    }

    [Fact]
    public void RegisterSingleton_WithNullInstance_ThrowsArgumentNullException()
    {
        var action = () => _resolver.RegisterSingleton<ITestService>(null!);
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Resolve_Named_ReturnsCorrectInstance()
    {
        _resolver.Register<ITestService, TestService>("service1");
        _resolver.Register<ITestService, AlternativeTestService>("service2");

        var instance1 = _resolver.Resolve<ITestService>("service1");
        var instance2 = _resolver.Resolve<ITestService>("service2");

        instance1.GetMessage().ShouldBe("TestService");
        instance2.GetMessage().ShouldBe("AlternativeTestService");
    }

    [Fact]
    public void Resolve_NamedUnregistered_ThrowsInvalidOperationException()
    {
        var action = () => _resolver.Resolve<ITestService>("nonexistent");

        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldStartWith("No named registration found for type");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Resolve_NamedWithNullOrEmptyName_ThrowsArgumentNullException(string? name)
    {
        var action = () => _resolver.Resolve<ITestService>(name!);

        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Resolve_RegisteredService_ReturnsInstance()
    {
        _resolver.Register<ITestService, TestService>();

        var instance = _resolver.Resolve<ITestService>();

        instance.ShouldNotBeNull();
        instance.ShouldBeOfType<TestService>();
    }

    [Fact]
    public void Resolve_TransientService_ReturnsNewInstanceEachTime()
    {
        _resolver.Register<ITestService, TestService>();

        var instance1 = _resolver.Resolve<ITestService>();
        var instance2 = _resolver.Resolve<ITestService>();

        instance1.ShouldNotBeSameAs(instance2);
    }

    [Fact]
    public void Resolve_UnregisteredService_ThrowsInvalidOperationException()
    {
        var action = () => _resolver.Resolve<ITestService>();

        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldStartWith("No registration found for type");
    }

    [Fact]
    public void Resolve_WithArrayDependency_InjectsAllServices()
    {
        _resolver.Register<TestService>();
        _resolver.Register<AlternativeTestService>();
        _resolver.Register<ServiceWithArrayDependency>();

        var instance = _resolver.Resolve<ServiceWithArrayDependency>();

        instance.Services.Length.ShouldBe(2);
        instance.Services.ShouldBeOfType<ITestService[]>();
    }

    [Fact]
    public void Resolve_WithCollectionDependency_InjectsAllServices()
    {
        _resolver.Register<TestService>();
        _resolver.Register<AlternativeTestService>();
        _resolver.Register<ServiceWithCollectionDependency>();

        var instance = _resolver.Resolve<ServiceWithCollectionDependency>();

        instance.Services.Count().ShouldBe(2);
    }

    [Fact]
    public void Resolve_WithDependencies_InjectsDependencies()
    {
        _resolver.Register<ITestService, TestService>();
        _resolver.Register<ITestRepository, TestRepository>();
        _resolver.Register<IComplexService, ComplexService>();

        var instance = _resolver.Resolve<IComplexService>();

        instance.Process().ShouldBe("TestService-TestRepository");
    }

    [Fact]
    public void Resolve_WithListDependency_InjectsAllServices()
    {
        _resolver.Register<TestService>();
        _resolver.Register<AlternativeTestService>();
        _resolver.Register<ServiceWithListDependency>();

        var instance = _resolver.Resolve<ServiceWithListDependency>();

        instance.Services.Count.ShouldBe(2);
        instance.Services.ShouldBeOfType<List<ITestService>>();
    }

    [Fact]
    public void SetCurrent_ReturnsResolver()
    {
        var result = _resolver.SetCurrent();

        result.ShouldBe(_resolver);
    }

    [Fact]
    public void WhereAssignableFrom_WithCompatibleServices_ReturnsServices()
    {
        _resolver.Register<TestService>();
        _resolver.Register<AlternativeTestService>();
        _resolver.Register<ITestRepository, TestRepository>();

        var services = _resolver.WhereAssignableFrom<ITestService>();

        services.Count.ShouldBe(2);
        services.ForAll(x => x.ShouldBeAssignableTo<ITestService>());
    }

    [Fact]
    public void WhereAssignableFrom_WithNamedServices_IncludesNamedServices()
    {
        _resolver.Register<TestService>();
        _resolver.Register<ITestService, AlternativeTestService>("named");

        var services = _resolver.WhereAssignableFrom<ITestService>();

        services.Count.ShouldBe(2);
    }

    [Fact]
    public void WhereAssignableFrom_WithoutCompatibleServices_ReturnsEmpty()
    {
        _resolver.Register<ITestRepository, TestRepository>();

        var services = _resolver.WhereAssignableFrom<ITestService>();

        services.ShouldBeEmpty();
    }
}
