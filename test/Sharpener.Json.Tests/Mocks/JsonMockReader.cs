// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Json.Types.Interfaces;
using Sharpener.Tests.Common.Models;

namespace Sharpener.Json.Tests.Mocks;

public class JsonMockReader : IJsonReader
{
    public Func<string, Type, object> Read => (_, _) => new Item("other", "person");
}
