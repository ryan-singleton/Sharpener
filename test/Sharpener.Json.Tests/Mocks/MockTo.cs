// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Tests.Mocks;

public class MockTo : IJsonSerializer
{
    public Func<object, string> Serialize => _ => "stuff";
}
