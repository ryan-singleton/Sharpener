namespace Sharpener.Injection.Tests.Models;

public class TestClass
{
    public TestClass(string? name, string value)
    {
        Name = name;
        Value = value;
    }

    public string? Name { get; set; }
    public string Value { get; set; }
}
