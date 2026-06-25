// The Sharpener project licenses this file to you under the MIT license.

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

        Food.RibeyeSteak.ShouldBe("Ribeye Steaks");
        new RibeyeSteak().Name.ShouldBe("Ribeye Steaks");
    }
}
