// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Tests.Mocks;

using Sharpener.Tests.Common.Models;
using Types.Interfaces;

public class JsonMockReader : IJsonReader
{
    public Func<string, Type, object> Read => (_, _) => new Item("other", "person");
}
