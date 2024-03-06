# AttributeSourceGenerator

A simple attribute-based Roslyn incremental source generator base class for .NET.

[![main](https://img.shields.io/github/actions/workflow/status/jscarle/AttributeSourceGenerator/main.yml?logo=github)](https://github.com/jscarle/AttributeSourceGenerator)
[![nuget](https://img.shields.io/nuget/v/AttributeSourceGenerator)](https://www.nuget.org/packages/AttributeSourceGenerator)
[![downloads](https://img.shields.io/nuget/dt/AttributeSourceGenerator)](https://www.nuget.org/packages/AttributeSourceGenerator)

### Example generator

```csharp
using AttributeSourceGenerator;
using Microsoft.CodeAnalysis;

namespace SourceGenerators;

[Generator]
public sealed class IdentifierSourceGenerator : AttributeIncrementalGeneratorBase
{
    public IdentifierSourceGenerator()
        : base(() => new AttributeIncrementalGeneratorConfiguration()
        {
            MarkerAttributeName = MarkerAttributeName,
            MarkerAttributeSource = MarkerAttributeSource,
            SymbolFilter = FilterType.Struct,
            SourceGenerator = GenerateIdentifier
        })
    {
    }

    private const string MarkerAttributeNamespace = "Domain.Common.Attributes";

    private const string MarkerAttributeName = $"{MarkerAttributeNamespace}.GeneratedIdentifierAttribute`1";

    private static source MarkerAttributeSource = new Source("GeneratedIdentifierAttribute`1", $$"""
        namespace {{MarkerAttributeNamespace}};
        
        [AttributeUsage(AttributeTargets.Struct)]
        public sealed class GeneratedIdentifierAttribute<TIdentifier> : Attribute;                                                               
        """;

    private static IEnumerable<Source> GenerateIdentifier(Symbol symbol)
    {
        return [new Source(symbol.Name, $$"""
            // <auto-generated/>
            
            #nullable enable
            
            namespace {{symbol.Namespace}};
            
            partial struct {{symbol.Name}} : IEquatable<{{symbol.Name}}>, IComparable<{{symbol.Name}}>, IComparable
            {
              // Implementation details
            }
            """)];
    }
}
```

### Typical .csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

	<PropertyGroup>
		<TargetFramework>netstandard2.0</TargetFramework>
		<Nullable>enable</Nullable>
		<ImplicitUsings>true</ImplicitUsings>
		<LangVersion>latest</LangVersion>
		<AnalysisLevel>latest-All</AnalysisLevel>
		<EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>
		<CodeAnalysisTreatWarningsAsErrors>true</CodeAnalysisTreatWarningsAsErrors>
		<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
	</PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.3.1" PrivateAssets="all" />
		<PackageReference Include="AttributeSourceGenerator" Version="8.0.2" PrivateAssets="all" GeneratePathProperty="true" />
	</ItemGroup>

	<PropertyGroup>
		<IsRoslynComponent>true</IsRoslynComponent>
		<IsPublishable>false</IsPublishable>
		<IsPackable>true</IsPackable>
		<IncludeBuildOutput>false</IncludeBuildOutput>
		<NoWarn>$(NoWarn);NU5128</NoWarn>
		<GetTargetPathDependsOn>$(GetTargetPathDependsOn);GetDependencyTargetPaths</GetTargetPathDependsOn>
	</PropertyGroup>

	<ItemGroup>
		<None Include="$(PkgAttributeSourceGenerator)/lib/netstandard2.0/AttributeSourceGenerator.dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
		<None Include="$(OutputPath)/$(AssemblyName).dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
	</ItemGroup>

	<Target Name="GetDependencyTargetPaths">
		<ItemGroup>
			<TargetPathWithTargetPlatformMoniker Include="$(PkgAttributeSourceGenerator)/lib/netstandard2.0/AttributeSourceGenerator.dll" IncludeRuntimeDependency="false" />
			<TargetPathWithTargetPlatformMoniker Include="$(MSBuildThisFileDirectory)bin/$(Configuration)/$(TargetFramework)/$(AssemblyName).dll" IncludeRuntimeDependency="false" />
		</ItemGroup>
	</Target>

</Project>
```
