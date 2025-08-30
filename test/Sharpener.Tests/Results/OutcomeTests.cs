// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Results;

namespace Sharpener.Tests.Results;

public class OutcomeTests
{
    [Fact]
    public void Error_WhenError_ReturnsError()
    {
        var error = new Error("Test error");
        Outcome<int> outcome = error;

        outcome.Error.ShouldBe(error);
    }

    [Fact]
    public void Error_WhenSuccess_ThrowsInvalidOperationException()
    {
        Outcome<int> outcome = 42;
        var action = () => { _ = outcome.Error; };
        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe("Outcome is in success state");
    }

    [Theory]
    [InlineData("Error message")]
    [InlineData("Another error")]
    public void ImplicitConversionFromError_CreatesErrorResult(string errorMessage)
    {
        Outcome<int> outcome = new Error(errorMessage);

        outcome.IsError.ShouldBeTrue();
        outcome.IsSuccess.ShouldBeFalse();
        outcome.Error.ErrorMessage.ShouldBe(errorMessage);
    }

    [Theory]
    [InlineData(42)]
    [InlineData("test")]
    [InlineData(true)]
    public void ImplicitConversionFromSuccess_CreatesSuccessOutcome<T>(T value)
    {
        Outcome<T> outcome = value;

        outcome.IsSuccess.ShouldBeTrue();
        outcome.IsError.ShouldBeFalse();
        outcome.Value.ShouldBe(value);
    }

    [Fact]
    public void ImplicitConversionToSuccess_ReturnsValue()
    {
        Outcome<int> outcome = 42;
        int value = outcome;

        value.ShouldBe(42);
    }

    [Fact]
    public void ImplicitConversionToSuccess_WhenError_ThrowsInvalidOperationException()
    {
        Outcome<int> outcome = new Error("Test error");

        var action = () =>
        {
            int _ = outcome;
        };

        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe("Outcome is in error state");
    }

    [Fact]
    public void Match_VoidVersion_WithError_CallsOnError()
    {
        Outcome<int> outcome = new Error("Test error");
        var successCalled = false;
        var errorCalled = false;

        outcome.Match(
            _ => successCalled = true,
            _ => errorCalled = true
        );

        successCalled.ShouldBeFalse();
        errorCalled.ShouldBeTrue();
    }

    [Fact]
    public void Match_VoidVersion_WithSuccess_CallsOnSuccess()
    {
        Outcome<int> outcome = 42;
        var successCalled = false;
        var errorCalled = false;

        outcome.Match(
            _ => successCalled = true,
            _ => errorCalled = true
        );

        successCalled.ShouldBeTrue();
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public void Match_WithError_CallsOnError()
    {
        Outcome<int> outcome = new Error("Test error");

        var output = outcome.Match(
            value => $"Success: {value}",
            error => $"Error: {error}"
        );

        output.ShouldBe("Error: Test error");
    }

    [Fact]
    public void Match_WithSuccess_CallsOnSuccess()
    {
        Outcome<int> outcome = 42;

        var output = outcome.Match(
            value => $"Success: {value}",
            error => $"Error: {error}"
        );

        output.ShouldBe("Success: 42");
    }

    [Fact]
    public void Value_WhenError_ThrowsInvalidOperationException()
    {
        Outcome<int> outcome = new Error("Test error");

        var action = () => { _ = outcome.Value; };

        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe("Outcome is in error state");
    }

    [Fact]
    public void Value_WhenSuccess_ReturnsValue()
    {
        Outcome<int> outcome = 42;

        outcome.Value.ShouldBe(42);
    }
}
