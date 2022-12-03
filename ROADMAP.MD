## Roadmap

### Autowatch

I'm looking to create `Stopwatch` wrapper that implements `IDisposable`. It would have the benefit of starting on instantiation (unless an optional parameter is set to false) and stopping and eventually getting collected once the using statement scope is exited. Not that one could not stop it themselves.

### Span

More `Span<T>` experimentation for performance improvements.

### Newtonsoft JSON

Will create a `Sharpener.Newtonsoft` package with some extensions to call that will rewire the `SerializationSettings` class to automatically use Newtonsoft instead. But this specific repo is meant to be third party package agnostic as possible.
