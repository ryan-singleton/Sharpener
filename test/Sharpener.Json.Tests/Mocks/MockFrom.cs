// The Sharpener project and Facefire license this file to you under the MIT license.

using Sharpener.Json.Types.Interfaces;
using Sharpener.Tests.Common.Models;

namespace Sharpener.Json.Tests.Mocks;

public class MockFrom : IJsonDeserializer
{
    public Func<string, Type, object> Deserialize => (_, _) => new Item("other", "person");
}
