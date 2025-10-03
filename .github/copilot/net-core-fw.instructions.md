---
description: 'Guidance for working with .NET Framework and .NET Core projects. Includes project structure, C# language version, NuGet management, and best practices.'
applyTo: '**/*.csproj, **/*.cs'
---

# .NET Framework/Core Development

## Build and Compilation Requirements

- Always use `dotnet build` to build the solution or projects instead of `msbuild /t:rebuild`

## .NET SDK project Structure for .NET Framework and .NET Core compatibility

.NET Framework projects use the legacy project format, which differs significantly from modern SDK-style projects.
This project - is SDK-style and supports multi-targeting for .NET Framework and .NET Core.

When working with such projects, consider the following structure:

- **Target Framework**: Uses `<TargetFramework>` instead of `<TargetFrameworkVersion>`
  - Example: `<TargetFrameworks>netstandard2.0;netstandard2.1;net462</TargetFrameworks>`
- **Build Configuration**: Contains explicit `<PropertyGroup>` sections for common groups of configurations
- **Package References**: Uses `<PackageReference>` instead of `packages.config` for NuGet dependencies
- **Conditional Compilation**: Uses `#if NETFRAMEWORK` and `#if NETSTANDARD` for framework-specific code
- **Conditional Package References**: Uses `Condition` attributes on `<PackageReference>` to include/exclude packages based on the target framework
  - Example:
    ```xml
    <ItemGroup Condition="'$(TargetFramework)' == 'net462'">
      <PackageReference Include="SomeLegacyPackage" Version="1.0.0" />
      <Reference Include="System.Net.Http" />
    </ItemGroup>
    ```
- **Assembly Info**: Uses `<GenerateAssemblyInfo>` to control automatic generation of assembly attributes
  - Example: `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>`
- **Common Properties**: Centralizes common properties like `Company`, `Authors`, `Version` and Nuget package properties in a single `<PropertyGroup>` should be defined once for all target frameworks in `SignNow.props` file. This file should be included in all projects using `<Import Project="..\SignNow.props" />`.

## NuGet Package Management

- Installing and updating NuGet packages in .NET Framework projects is a complex task requiring coordinated changes to multiple files. Therefore, **do not attempt to install or update NuGet packages** in this project.
- Instead, if changes to NuGet references are required, ask the user to install or update NuGet packages using the Visual Studio NuGet Package Manager or Visual Studio package manager console.
- When recommending NuGet packages, ensure they are compatible with .NET Framework or .NET Standard 2.0 (not only .NET Core or .NET 5+).

## Environment Considerations (Cross-platform environment)

- Use Unix-style paths with forward slashes (e.g., `/path/to/file.cs`) and ensure compatibility with both Windows and Unix-like systems.
- Use `dotnet` CLI commands for building, testing, and managing projects instead of Windows
- Avoid Windows-specific commands and tools (e.g., PowerShell scripts, batch files)
- Use Unix-appropriate commands when suggesting terminal operations
- Consider Unix-specific behaviors when working with file system operations

## Common .NET Framework/Core agreements and Best Practices

### Async/Await Patterns
- **ConfigureAwait(false)**: Always use `ConfigureAwait(false)` in library code to avoid deadlocks:
  ```csharp
  var result = await SomeAsyncMethod().ConfigureAwait(false);
  ```
- **Avoid sync-over-async**: Don't use `.Result` or `.Wait()` or `.GetAwaiter().GetResult()`. These sync-over-async patterns can lead to deadlocks and poor performance. Always use `await` for asynchronous calls.

### DateTime Handling

- **Use DateTimeOffset for timestamps**: Prefer `DateTimeOffset` over `DateTime` for absolute time points
- **Specify DateTimeKind**: When using `DateTime`, always specify `DateTimeKind.Utc`
- **Culture-aware formatting**: Use `CultureInfo.InvariantCulture` for serialization/parsing

### Memory Management

- **Dispose pattern**: Implement `IDisposable` properly for unmanaged resources
- **Using statements**: Always wrap `IDisposable` objects in using statements
- **Avoid large object heap**: Keep objects under 85KB to avoid LOH allocation