// The Sharpener project licenses this file to you under the MIT license.

using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Sharpener.Revit.Attributes;
using UiResult = Autodesk.Revit.UI.Result;

namespace Sharpener.Revit.Commands;

/// <summary>
///     It is highly common to write Revit <see cref="IExternalCommand" /> implementations that perform a try catch based
///     upon user cancellation and then unhandled exception logic where an action can be taken and then a failure result is
///     returned. Deriving from this class along with <see cref="TryableCommandAttribute" /> decoration and implementing
///     <see cref="TryExecute" /> allows that boilerplate logic to be applied automatically so you can focus on the command
///     implementation otherwise.
/// </summary>
public abstract class TryableExternalCommand : IExternalCommand
{
    /// <inheritdoc />
    public UiResult Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var attribute = GetType().GetCustomAttribute<TryableCommandAttribute>() ?? new TryableCommandAttribute();

        try
        {
            return TryExecute(commandData, ref message, elements);
        }
        catch (OperationCanceledException)
        {
            message = attribute.CancelledMessage;
            return UiResult.Cancelled;
        }
        catch (Exception ex)
        {
            if (!attribute.SuppressOnError)
            {
                attribute.OnError(ex);
            }

            return UiResult.Failed;
        }
    }

    /// <inheritdoc cref="Execute" />
    protected abstract UiResult TryExecute(ExternalCommandData commandData, ref string message, ElementSet elements);
}
