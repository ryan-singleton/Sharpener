using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Tests.Mocks;

public class MockTo : IJsonSerializer
{
    public Func<object, string> Serialize => _ => "stuff";
}
