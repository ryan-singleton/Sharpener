using Sharpener.Tests.Models;
using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Tests.Mocks;

public class MockFrom : IJsonDeserializer
{
    public Func<string, Type, object> Deserialize => (_, _) => new Item("other", "person");
}
