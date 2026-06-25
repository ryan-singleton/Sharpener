// The Sharpener project licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

// ReSharper disable ReplaceSubstringWithRangeIndexer
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable UseCollectionExpression

namespace Sharpener.NamedResources.Generators;

[Generator(LanguageNames.CSharp)]
internal sealed class EnumDrivenGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat FullyQualifiedFormat =
        SymbolDisplayFormat.FullyQualifiedFormat;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var enumProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is EnumDeclarationSyntax enumDeclaration
                                    && HasNamedResourceAttribute(enumDeclaration),
                static (context, cancellationToken) => GetEnumData(context, cancellationToken))
            .Where(static data => data is not null);

        context.RegisterSourceOutput(
            enumProvider,
            static (context, enumData) =>
            {
                if (enumData is not null)
                {
                    WriteFileContent(context, enumData);
                }
            });
    }

    private static bool HasNamedResourceAttribute(EnumDeclarationSyntax enumDeclaration)
    {
        return enumDeclaration.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .Any(IsNamedResourceAttribute);
    }

    private static bool IsNamedResourceAttribute(AttributeSyntax attribute)
    {
        return attribute.Name.ToString() is Strings.Domain or Strings.DomainAttribute;
    }

    private static EnumData? GetEnumData(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var enumDeclaration = (EnumDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;

        var enumSymbol = semanticModel.GetDeclaredSymbol(enumDeclaration, cancellationToken);

        if (enumSymbol is null)
        {
            return null;
        }

        var attribute = enumDeclaration.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .FirstOrDefault(IsNamedResourceAttribute);

        if (attribute is null)
        {
            return null;
        }

        var interfaceSymbol = GetInterfaceSymbol(semanticModel, attribute, cancellationToken);

        if (interfaceSymbol is null)
        {
            return null;
        }

        var attributeOptions = GetAttributeOptions(
            semanticModel,
            attribute,
            interfaceSymbol,
            enumSymbol,
            cancellationToken);

        var members = enumDeclaration.Members
            .Select(memberDeclaration => GetEnumMemberData(
                semanticModel,
                memberDeclaration,
                cancellationToken))
            .ToImmutableArray();

        return new EnumData(
            enumSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : enumSymbol.ContainingNamespace.ToDisplayString(),
            enumSymbol.Name,
            enumSymbol.ToDisplayString(FullyQualifiedFormat),
            interfaceSymbol.Name,
            interfaceSymbol.ToDisplayString(FullyQualifiedFormat),
            attributeOptions.ConstantsClassName,
            attributeOptions.NamedResourceNamespace,
            attributeOptions.ConstantsClassNamespace,
            attributeOptions.ConstantsClassSummary,
            members);
    }

    private static INamedTypeSymbol? GetInterfaceSymbol(
        SemanticModel semanticModel,
        AttributeSyntax attribute,
        CancellationToken cancellationToken)
    {
        var typeofExpression = attribute.ArgumentList?.Arguments
            .Select(argument => argument.Expression)
            .OfType<TypeOfExpressionSyntax>()
            .FirstOrDefault();

        if (typeofExpression is null)
        {
            return null;
        }

        return semanticModel.GetTypeInfo(typeofExpression.Type, cancellationToken).Type as INamedTypeSymbol;
    }

    private static AttributeOptions GetAttributeOptions(
        SemanticModel semanticModel,
        AttributeSyntax attribute,
        INamedTypeSymbol interfaceSymbol,
        INamedTypeSymbol enumSymbol,
        CancellationToken cancellationToken)
    {
        var args = attribute.ArgumentList?.Arguments ?? default;
        var attributeConstructor = semanticModel.GetSymbolInfo(attribute, cancellationToken).Symbol as IMethodSymbol;

        var defaultConstantsClassName = GetDefaultConstantsClassName(interfaceSymbol.Name);
        var defaultConstantsClassSummary =
            $"An automatically generated class with constant string values that correlate to the values found in the <see cref=\"{GetXmlDocTypeName(enumSymbol)}\"/> enum.";

        return new AttributeOptions(
            GetStringArg(semanticModel, args, attributeConstructor, "constantsClassName", cancellationToken)
            ?? defaultConstantsClassName,
            GetStringArg(semanticModel, args, attributeConstructor, "namedResourceNamespace", cancellationToken)
            ?? Strings.DefaultNamedResourcesNamespace,
            GetStringArg(semanticModel, args, attributeConstructor, "constantsClassNamespace", cancellationToken)
            ?? Strings.DefaultNamedResourcesNamespace,
            GetStringArg(semanticModel, args, attributeConstructor, "constantsClassSummary", cancellationToken)
            ?? defaultConstantsClassSummary);
    }

    private static string GetDefaultConstantsClassName(string interfaceName)
    {
        var className = interfaceName.EndsWith("Name", StringComparison.Ordinal)
            ? interfaceName.Substring(0, interfaceName.Length - "Name".Length)
            : interfaceName;

        className = $"{className}s";

        return StartsWithTwoUpper(className)
            ? className.Substring(1)
            : className;
    }

    private static string GetXmlDocTypeName(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.ToDisplayString(FullyQualifiedFormat).Replace("global::", string.Empty);
    }

    private static string? GetStringArg(
        SemanticModel semanticModel,
        SeparatedSyntaxList<AttributeArgumentSyntax> args,
        IMethodSymbol? attributeConstructor,
        string paramName,
        CancellationToken cancellationToken)
    {
        foreach (var arg in args)
        {
            if (arg.NameColon?.Name.Identifier.ValueText == paramName)
            {
                return GetConstantStringValue(semanticModel, arg, cancellationToken);
            }
        }

        foreach (var arg in args)
        {
            if (arg.NameEquals?.Name.Identifier.ValueText == paramName)
            {
                return GetConstantStringValue(semanticModel, arg, cancellationToken);
            }
        }

        if (attributeConstructor is null)
        {
            return null;
        }

        var parameterIndex = GetParameterIndex(attributeConstructor, paramName);

        if (parameterIndex < 0)
        {
            return null;
        }

        var positionalIndex = 0;

        foreach (var arg in args)
        {
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

    private static int GetParameterIndex(IMethodSymbol attributeConstructor, string paramName)
    {
        for (var i = 0; i < attributeConstructor.Parameters.Length; i++)
        {
            if (attributeConstructor.Parameters[i].Name == paramName)
            {
                return i;
            }
        }

        return -1;
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

    private static EnumMemberData GetEnumMemberData(
        SemanticModel semanticModel,
        EnumMemberDeclarationSyntax memberDeclaration,
        CancellationToken cancellationToken)
    {
        var memberName = memberDeclaration.Identifier.ValueText;

        return new EnumMemberData(
            memberName,
            GetDescriptionValue(semanticModel, memberDeclaration, memberName, cancellationToken),
            GetSummaryText(memberDeclaration));
    }

    private static string GetDescriptionValue(
        SemanticModel semanticModel,
        EnumMemberDeclarationSyntax memberDeclaration,
        string fallbackValue,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in
                 memberDeclaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
        {
            var attributeName = attribute.Name.ToString();

            if (!attributeName.EndsWith("Description", StringComparison.Ordinal)
                && !attributeName.EndsWith("DescriptionAttribute", StringComparison.Ordinal))
            {
                continue;
            }

            var argument = attribute.ArgumentList?.Arguments.FirstOrDefault();

            if (argument is null)
            {
                return fallbackValue;
            }

            var constantValue = semanticModel.GetConstantValue(argument.Expression, cancellationToken);

            return constantValue is { HasValue: true, Value: string description }
                ? description
                : fallbackValue;
        }

        return fallbackValue;
    }

    private static void WriteFileContent(SourceProductionContext context, EnumData enumData)
    {
        WriteSource(
            context,
            GetHintName(enumData, $"{enumData.InterfaceName}s"),
            GenerateNamedResourceStructs(enumData));

        WriteSource(
            context,
            GetHintName(enumData, enumData.ConstantsClassName),
            GenerateConstantStrings(enumData));
    }

    private static void WriteSource(
        SourceProductionContext context,
        string hintName,
        string source)
    {
        context.AddSource(hintName, SourceText.From(source, Encoding.UTF8));
    }

    private static string GetHintName(EnumData enumData, string generatedTypeName)
    {
        var prefix = string.IsNullOrWhiteSpace(enumData.EnumNamespace)
            ? enumData.EnumName
            : $"{enumData.EnumNamespace}.{enumData.EnumName}";

        return $"{prefix}.{generatedTypeName}.g.cs";
    }

    private static string GenerateNamedResourceStructs(EnumData enumData)
    {
        var members = string.Concat(enumData.Members.Select(member => GenerateNamedResourceStruct(enumData, member)));

        return $"""
                // <auto-generated />
                #nullable enable

                using System.CodeDom.Compiler;
                using Sharpener.Extensions;

                namespace {enumData.NamedResourceNamespace};

                {members}
                """;
    }

    private static string GenerateNamedResourceStruct(EnumData enumData, EnumMemberData member)
    {
        return $$"""
                 {{FormatXmlSummary(CreateSummaryText(member))}}
                 [GeneratedCode("Sharpener.NamedResources.Generators", "{{Strings.Version}}")]
                 public readonly struct {{member.Name}}() : {{enumData.InterfaceFullName}}
                 {
                     /// <inheritdoc />
                     public string Name { get; } = {{enumData.EnumFullName}}.{{member.Name}}.ToDescriptionValue()!;
                 }

                 """;
    }

    private static string GenerateConstantStrings(EnumData enumData)
    {
        var members = string.Concat(enumData.Members.Select(member => GenerateConstantString(enumData, member)));

        return $$"""
                 // <auto-generated />
                 #nullable enable

                 using System.CodeDom.Compiler;
                 using Sharpener.Extensions;

                 namespace {{enumData.ConstantsClassNamespace}};

                 /// <summary>
                 /// {{enumData.ConstantsClassSummary}}
                 /// </summary>
                 [GeneratedCode("Sharpener.NamedResources.Generators", "{{Strings.Version}}")]
                 public static class {{enumData.ConstantsClassName}}
                 {
                 {{members}}}

                 """;
    }

    private static string GenerateConstantString(EnumData enumData, EnumMemberData member)
    {
        return $"""
                    {FormatXmlSummary(CreateSummaryText(member), "    ")}
                    public static readonly string {member.Name} = {enumData.EnumFullName}.{member.Name}.ToDescriptionValue()!;

                """;
    }

    private static string FormatXmlSummary(string text, string indent = "")
    {
        var builder = new StringBuilder();

        builder.Append(indent);
        builder.AppendLine("/// <summary>");

        foreach (var line in SplitLines(text))
        {
            builder.Append(indent);
            builder.Append("///     ");
            builder.AppendLine(line);
        }

        builder.Append(indent);
        builder.Append("/// </summary>");

        return builder.ToString();
    }

    private static IEnumerable<string> SplitLines(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            yield return string.Empty;
            yield break;
        }

        foreach (var line in text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
        {
            yield return line;
        }
    }

    private static string GetSummaryText(EnumMemberDeclarationSyntax memberDeclaration)
    {
        foreach (var trivia in memberDeclaration.GetLeadingTrivia())
        {
            if (!trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
            {
                continue;
            }

            return ExtractSummaryText(trivia.ToFullString());
        }

        return string.Empty;
    }

    private static string ExtractSummaryText(string triviaText)
    {
        var lines = triviaText
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
            .Select(line => line.Trim())
            .Select(line => line.StartsWith("///", StringComparison.Ordinal)
                ? line.Substring(3).Trim()
                : line)
            .ToArray();

        var builder = new StringBuilder();
        var insideSummary = false;

        foreach (var line in lines)
        {
            if (!insideSummary)
            {
                var startIndex = line.IndexOf("<summary>", StringComparison.Ordinal);
                if (startIndex < 0)
                {
                    continue;
                }

                insideSummary = true;

                var content = line.Substring(startIndex + "<summary>".Length);

                var endIndex = content.IndexOf("</summary>", StringComparison.Ordinal);
                if (endIndex >= 0)
                {
                    var inline = content.Substring(0, endIndex).Trim();
                    if (!string.IsNullOrWhiteSpace(inline))
                    {
                        builder.AppendLine(inline);
                    }

                    break;
                }

                if (!string.IsNullOrWhiteSpace(content))
                {
                    builder.AppendLine(content.Trim());
                }
            }
            else
            {
                var endIndex = line.IndexOf("</summary>", StringComparison.Ordinal);
                if (endIndex >= 0)
                {
                    var content = line.Substring(0, endIndex).Trim();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        builder.AppendLine(content);
                    }

                    break;
                }

                if (!string.IsNullOrWhiteSpace(line))
                {
                    builder.AppendLine(line);
                }
            }
        }

        return builder.ToString().TrimEnd();
    }

    private static string CreateSummaryText(EnumMemberData member)
    {
        if (!string.IsNullOrWhiteSpace(member.Summary))
        {
            return member.Summary;
        }

        return !string.IsNullOrWhiteSpace(member.Description)
            ? $"Represents the {member.Description} named resource."
            : "Represents a named resource.";
    }

    private static bool StartsWithTwoUpper(string input)
    {
        return input.Length >= 2
               && char.IsUpper(input[0])
               && char.IsUpper(input[1]);
    }

    private sealed record AttributeOptions(
        string ConstantsClassName,
        string NamedResourceNamespace,
        string ConstantsClassNamespace,
        string ConstantsClassSummary);

    private sealed record EnumData(
        string EnumNamespace,
        string EnumName,
        string EnumFullName,
        string InterfaceName,
        string InterfaceFullName,
        string ConstantsClassName,
        string NamedResourceNamespace,
        string ConstantsClassNamespace,
        string ConstantsClassSummary,
        ImmutableArray<EnumMemberData> Members);

    private sealed record EnumMemberData(
        string Name,
        string Description,
        string Summary);
}
