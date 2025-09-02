// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;
using Sharpener.Revit.Commands;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Sharpener.Revit.Attributes;

/// <summary>
///     It is highly common to write Revit <see cref="IExternalCommand" /> implementations that perform a try catch based
///     upon user cancellation and then unhandled exception logic where an action can be taken and then a failure result is
///     returned. This can be applied automatically by using this attribute along with
///     <see cref="TryableExternalCommand" />. See that class documentation for more information.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TryableCommandAttribute : Attribute
{
    /// <summary>
    ///     Gets or sets the message displayed to the user when the operation is canceled.
    ///     This is used by the <see cref="TryableExternalCommand" /> class to provide feedback
    ///     when a cancellation takes place during command execution. The default value is "Operation canceled by user."
    /// </summary>
    public string CancelledMessage { get; set; } = "Operation cancelled by user.";

    /// <summary>
    ///     Gets or sets a value indicating whether errors should be suppressed during command execution.
    ///     When set to true, the <see cref="TryableExternalCommand" /> will not invoke the error handling logic
    ///     defined by the <see cref="OnError" /> action in the event of an exception,
    ///     and it will directly return a failure result.
    ///     This property allows finer control over how unhandled exceptions are managed during command execution.
    ///     The default value is false.
    /// </summary>
    public bool SuppressOnError { get; set; }

    internal Action<Exception> OnError { get; set; } = exception =>
    {
        var message = $"Error: {exception.Message}";
        TaskDialog.Show("Error", message);
    };
}
