// The Sharpener project licenses this file to you under the MIT license.

using Fizz.Buzz;
using Foo.Bar;
using NamedResources;
using Shouldly;
using Xunit;

namespace Sharpener.NamedResources.Tests;

public class GeneratorTests
{
    [Fact]
    public void GenerationSucceeds()
    {
        var constFunc = () =>
        {
            _ = Ingredients.Butter;
            _ = Food.RibeyeSteak;
        };

        var typeFunc = () =>
        {
            _ = new Butter();
            _ = new RibeyeSteak();
        };
        constFunc.ShouldNotThrow();
        typeFunc.ShouldNotThrow();
    }

    [Fact]
    public void ValuesAreCorrect()
    {
        Ingredients.Butter.ShouldBe("Unsalted Butter");
        new Butter().Name.ShouldBe("Unsalted Butter");

        Food.RibeyeSteak.ShouldBe("Ribeye Steak");
        new RibeyeSteak().Name.ShouldBe("Ribeye Steak");
    }

    [Fact]
    public void NamespacesAreCorrect()
    {
        typeof(Butter).Namespace.ShouldBe("NamedResources");
        typeof(RibeyeSteak).Namespace.ShouldBe("Foo.Bar");

        typeof(Ingredients).Namespace.ShouldBe("NamedResources");
        typeof(Food).Namespace.ShouldBe("Fizz.Buzz");
    }
}
