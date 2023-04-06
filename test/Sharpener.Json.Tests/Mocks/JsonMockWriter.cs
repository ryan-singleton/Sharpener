// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Tests.Mocks;

using Types.Interfaces;

public class JsonMockWriter : IJsonWriter
{
    public Func<object, string> Write => _ => "stuff";
}
