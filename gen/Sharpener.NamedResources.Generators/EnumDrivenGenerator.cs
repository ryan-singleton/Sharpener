// The Sharpener project licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Sharpener.NamedResources.Generators;

[Generator(LanguageNames.CSharp)]
internal class EnumDrivenGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var enumProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (node, _) => node is EnumDeclarationSyntax e
                             && e.AttributeLists
                                 .SelectMany(al => al.Attributes)
                                 .Any(a => a.Name.ToString() is Strings.Domain or Strings.DomainAttribute),
                static (context, cancellationToken) => GetEnumData(context, cancellationToken))
            .Where(static data => data is not null);

        context.RegisterSourceOutput(
            enumProvider,
            (ctx, enumData) =>
            {
                if (enumData is not null)
                {
                    WriteFileContent(ctx, enumData);
                }
            });
    }

    private static EnumData? GetEnumData(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var enumDeclaration = (EnumDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;

        var enumSymbol = ModelExtensions.GetDeclaredSymbol(semanticModel, enumDeclaration, cancellationToken);

        if (enumSymbol is null)
        {
            return null;
        }

        var enumNamespace = enumSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : enumSymbol.ContainingNamespace.ToDisplayString();

        var members = new List<EnumMemberData>();

        foreach (var memberDeclaration in enumDeclaration.Members)
        {
            var memberName = memberDeclaration.Identifier.ValueText;
            var descriptionValue = GetDescriptionValue(semanticModel, memberDeclaration, memberName, cancellationToken);
            var summary = GetSummaryText(memberDeclaration);
            members.Add(new EnumMemberData(memberName, descriptionValue, summary));
        }

        var attr = enumDeclaration.AttributeLists
            .SelectMany(al => al.Attributes)
            .First(a => a.Name.ToString() is Strings.Domain or  Strings.DomainAttribute);

        // All four named args are optional — read them if present, otherwise fall back to defaults
        var args = attr.ArgumentList?.Arguments ?? default;

        var attrCtor = semanticModel.GetSymbolInfo(attr, cancellationToken).Symbol as IMethodSymbol;

        var interfaceName = string.Empty;
        if (attr.ArgumentList!.Arguments[0].Expression is TypeOfExpressionSyntax typeofExpr)
        {
            interfaceName = typeofExpr.Type.ToString();
        }

        // Derive the default constants class name from the interface name the same way
        // the attribute constructor does at runtime: strip trailing "Name", then leading "I"
        var defaultConstantsClassName = interfaceName
            .TrimEnd("Name".ToCharArray());
        defaultConstantsClassName = $"{defaultConstantsClassName}s";

        if (StartsWithTwoUpper(defaultConstantsClassName))
        {
            defaultConstantsClassName = defaultConstantsClassName.Substring(1);
        }


        var constantsClassName = GetStringArg(semanticModel, args, attrCtor, "constantsClassName", cancellationToken)
                                 ?? defaultConstantsClassName;


        var namedResourceNamespace = GetStringArg(semanticModel, args, attrCtor, "namedResourceNamespace", cancellationToken)
                                     ?? Strings.DefaultNamedResourcesNamespace;

        var constantsClassNamespace =
            GetStringArg(semanticModel, args, attrCtor, "constantsClassNamespace", cancellationToken)
            ?? Strings.DefaultNamedResourcesNamespace;

        var constantsClassSummary =
            GetStringArg(semanticModel, args, attrCtor, "constantsClassSummary", cancellationToken)
            ?? $"An automatically generated class with constant string values that correlate to the values found in the {interfaceName} class.";


            // ReSharper disable once UseCollectionExpression
        return new EnumData( enumNamespace, enumSymbol.Name, new List<EnumMemberData>(members).ToImmutableArray(), interfaceName, constantsClassName, namedResourceNamespace, constantsClassNamespace, constantsClassSummary);
    }

   private static string? GetStringArg(
    SemanticModel semanticModel,
    SeparatedSyntaxList<AttributeArgumentSyntax> args,
    IMethodSymbol? attributeConstructor,
    string paramName,
    CancellationToken cancellationToken)
{
    // Handles constructor named arguments:
    // [Domain(typeof(IFooName), constantsClassName: "FooNames")]
    foreach (var arg in args)
    {
        if (arg.NameColon?.Name.Identifier.ValueText != paramName)
        {
            continue;
        }

        return GetConstantStringValue(semanticModel, arg, cancellationToken);
    }

    // Handles attribute property / field assignments:
    // [Domain(typeof(IFooName), ConstantsClassName = "FooNames")]
    //
    // You may not need this today, but it costs almost nothing and prevents
    // confusion if you later move options to settable attribute properties.
    foreach (var arg in args)
    {
        if (arg.NameEquals?.Name.Identifier.ValueText != paramName)
        {
            continue;
        }

        return GetConstantStringValue(semanticModel, arg, cancellationToken);
    }

    // Handles positional constructor arguments:
    // [Domain(typeof(IFooName), "FooNames")]
    if (attributeConstructor is null)
    {
        return null;
    }

    var parameterIndex = -1;

    for (var i = 0; i < attributeConstructor.Parameters.Length; i++)
    {
        if (attributeConstructor.Parameters[i].Name == paramName)
        {
            parameterIndex = i;
            break;
        }
    }

    if (parameterIndex < 0)
    {
        return null;
    }

    var positionalIndex = 0;

    foreach (var arg in args)
    {
        // Not positional.
        if (arg.NameColon is not null || arg.NameEquals is not null)
        {
            continue;
        }

        if (positionalIndex == parameterIndex)
        {
            return GetConstantStringValue(semanticModel, arg, cancellationToken);
        }

        positionalIndex++;
    }

    return null;
}

private static string? GetConstantStringValue(
    SemanticModel semanticModel,
    AttributeArgumentSyntax argument,
    CancellationToken cancellationToken)
{
    var constantValue = semanticModel.GetConstantValue(argument.Expression, cancellationToken);

    return constantValue is { HasValue: true, Value: string value }
        ? value
        : null;
}

    private static string GetDescriptionValue( SemanticModel semanticModel, EnumMemberDeclarationSyntax memberDeclaration, string fallbackValue, CancellationToken cancellationToken)
    {
        foreach (var attributeList in memberDeclaration.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var attributeName = attribute.Name.ToString();

                if (!attributeName.EndsWith("Description", StringComparison.Ordinal) &&
                    !attributeName.EndsWith("DescriptionAttribute", StringComparison.Ordinal))
                {
                    continue;
                }

                var argument = attribute.ArgumentList?.Arguments.FirstOrDefault();

                if (argument is null)
                {
                    return fallbackValue;
                }

                var constantValue = semanticModel.GetConstantValue(argument.Expression, cancellationToken);

                return constantValue is { HasValue: true, Value: string description } ? description : fallbackValue;
            }
        }

        return fallbackValue;
    }

    private static void WriteFileContent(SourceProductionContext context, EnumData enumData)
    {
        EmitNamedResourceStructs(context, enumData);
        EmitConstantStrings(context, enumData);
    }

    private static void EmitNamedResourceStructs(SourceProductionContext context, EnumData enumData)
    {
        var builder = new StringBuilder();

        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("#nullable enable");
        builder.AppendLine();
        builder.AppendLine($"using {enumData.Namespace};");
        builder.AppendLine("using Sharpener.Extensions;");
        builder.AppendLine();
        builder.AppendLine($"namespace {enumData.NamedResourceNamespace};");
        builder.AppendLine();

        foreach (var member in enumData.Members)
        {
            var summaryText = CreateSummaryText(member);
            builder.AppendLine("/// <summary>");
            foreach (var line in summaryText.Split(["\r\n", "\n"], StringSplitOptions.None))
            {
                builder.Append("///     ");
                builder.AppendLine(line);
            }

            builder.AppendLine("/// </summary>");
            builder.Append("public readonly struct ");
            builder.Append(member.Name);
            builder.AppendLine($"() : {enumData.InterfaceName}");
            builder.AppendLine("{");
            builder.AppendLine("    /// <inheritdoc />");
            builder.Append("    public string Name { get; } = ");
            builder.Append(enumData.EnumName);
            builder.Append('.');
            builder.Append(member.Name);
            builder.AppendLine(".ToDescriptionValue()!;");
            builder.AppendLine("}");
            builder.AppendLine();
        }

        var csharpFileName = $"{enumData.InterfaceName}s.g.cs";
        context.AddSource(csharpFileName, SourceText.From(builder.ToString(), Encoding.UTF8));
    }

    private static void EmitConstantStrings(SourceProductionContext context, EnumData enumData)
    {
        var builder = new StringBuilder();

        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("#nullable enable");
        builder.AppendLine();
        builder.AppendLine($"using {enumData.Namespace};");
        builder.AppendLine("using Sharpener.Extensions;");
        builder.AppendLine();
        builder.AppendLine($"namespace {enumData.ConstantsClassNamespace};");
        builder.AppendLine();
        builder.AppendLine("/// <summary>");
        builder.AppendLine($"/// {enumData.ConstantsClassSummary}");
        builder.AppendLine("/// </summary>");
        builder.AppendLine($"public static class {enumData.ConstantsClassName}");
        builder.AppendLine("{");

        foreach (var member in enumData.Members)
        {
            var summaryText = CreateSummaryText(member);
            builder.AppendLine("    /// <summary>");
            foreach (var line in summaryText.Split(["\r\n", "\n"], StringSplitOptions.None))
            {
                builder.Append("    ///     ");
                builder.AppendLine(line);
            }

            builder.AppendLine("    /// </summary>");
            builder.AppendLine(
                $"    public static readonly string {member.Name} = {enumData.EnumName}.{member.Name}.ToDescriptionValue()!;");
            builder.AppendLine();
        }

        builder.AppendLine("}");

        var csharpFileName = $"{enumData.ConstantsClassName}.g.cs";
        context.AddSource(csharpFileName, SourceText.From(builder.ToString(), Encoding.UTF8));
    }

    private static string GetSummaryText(EnumMemberDeclarationSyntax memberDeclaration)
    {
        var triviaList = memberDeclaration.GetLeadingTrivia();

        foreach (var trivia in triviaList)
        {
            if (!trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
            {
                continue;
            }

            var triviaText = trivia.ToFullString();

                    // ReSharper disable once ReplaceSubstringWithRangeIndexer
            var lines = triviaText
                .Split(["\r\n", "\n"], StringSplitOptions.None)
                .Select(line => line.Trim())
                .Select(line => line.StartsWith("///", StringComparison.Ordinal)
                    ? line.Substring(3).Trim()
                    : line)
                .ToArray();

            var summaryLines = new List<string>();
            var insideSummary = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("<summary>", StringComparison.Ordinal))
                {
                    insideSummary = true;

                    // ReSharper disable once ReplaceSubstringWithRangeIndexer
                    var remaining = line.Substring("<summary>".Length).Trim();

                    if (!string.IsNullOrWhiteSpace(remaining))
                    {
                        summaryLines.Add(remaining);
                    }

                    continue;
                }

                if (line.StartsWith("</summary>", StringComparison.Ordinal))
                {
                    break;
                }

                if (insideSummary)
                {
                    summaryLines.Add(line);
                }
            }

            return string.Join(@"\r\n", summaryLines.Where(line => !string.IsNullOrWhiteSpace(line)));
        }

        return string.Empty;
    }

    private static string CreateSummaryText(EnumMemberData member)
    {
        if (!string.IsNullOrWhiteSpace(member.Summary))
        {
            return member.Summary;
        }

        return !string.IsNullOrWhiteSpace(member.Description) ? $"Represents the {member.Description} named resource." : "Represents a named resource.";
    }

    public static bool StartsWithTwoUpper(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2)
            return false;

        return char.IsUpper(input[0]) && char.IsUpper(input[1]);
    }
}

internal sealed class EnumData(
    string ns,
    string enumName,
    ImmutableArray<EnumMemberData> members,
    string interfaceName,
    string constantsClassName,
    string namedResourceNamespace,
    string constantsClassNamespace,
    string constantsClassSummary)
{
    public string Namespace { get; } = ns;
    public string EnumName { get; } = enumName;
    public ImmutableArray<EnumMemberData> Members { get; } = members;
    public string InterfaceName { get; } = interfaceName;
    public string ConstantsClassName { get; } = constantsClassName;
    public string NamedResourceNamespace { get; } = namedResourceNamespace;
    public string ConstantsClassNamespace { get; } = constantsClassNamespace;
    public string ConstantsClassSummary { get; } = constantsClassSummary;
}

internal sealed class EnumMemberData(string name, string description, string summary)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Summary { get; } = summary;
}
