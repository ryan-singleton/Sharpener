using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Tests.Mocks;

public class MockTo : IJsonSerializer
{
    public Func<object, string> Serialize => _ => "stuff";
}
