namespace Sharpener.Delegates;

/// <summary>
/// A storage place for named delegates that are kept for specific contexts.
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public class DelegateStore<TDelegate> where TDelegate : Delegate
{
    private const string _defaultName = "default";
    private readonly IDictionary<string, TDelegate> _namedDelegates;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="defaultLogic">The default logic of this store.</param>
    public DelegateStore(TDelegate defaultLogic)
    {
        _namedDelegates = new Dictionary<string, TDelegate>();
        SetDefault(defaultLogic);
    }

    /// <summary>
    /// Sets the default logic of the store. If the logic parameter is null, this will throw a null argument exception, as that cannot be null.
    /// </summary>
    /// <param name="logic"></param>
    public void SetDefault(TDelegate logic)
    {
        if (logic is null)
        {
            throw new ArgumentNullException("Default logic cannot be null");
        }
        SetNamed(_defaultName, logic);
    }

    /// <summary>
    /// Gets a named function from the store. Returns null if not found.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TDelegate? GetNamed(string name) => _namedDelegates.ContainsKey(name) ? _namedDelegates[name] : null;

    /// <summary>
    /// Gets the default function of the store. Will not be null as setting it to null is not allowed.
    /// </summary>
    /// <returns></returns>
    public TDelegate Default => GetNamed(_defaultName)!;

    /// <summary>
    /// Sets or creates a named delegate.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="logic">The logic of the delegate.</param>
    public void SetNamed(string name, TDelegate logic)
    {
        if (_namedDelegates.ContainsKey(name))
        {
            _namedDelegates[name] = logic;
            return;
        }

        _namedDelegates.Add(name, logic);
    }
}
