---
applyTo: "**/*.cs"
description: "Guidelines for building application business logic, based on SignNow API specification."
---

# SignNow .NET SDK Development Guidelines

This document outlines specific of SignNow API to follow coding standards and best practices for the SignNow .NET SDK project.

## General Code Rules

- Use existing SDK patterns and extension methods
- Trust the infrastructure layer (SignNowClient) to handle transport concerns
- Keep service layer focused on business logic and request preparation
- Maintain clear separation between layers


## Key Development Patterns
- **Consistent Pattern**: Follow the same pattern throughout the entire codebase
- **Keep it readable**: Code should be clean and easy to read
- **Fail Fast Pattern**: Let exceptions occur early rather than hiding them
- **Resource Cleanup**: Always clean up test resources (documents, files, etc.). Use `DisposableDocumentId` variable for documents from `AuthorizedApiTestBase` class for this purpose. Extend this pattern to other resource types as needed.
- **Avoid Fully Qualified Names (FQNs)** in code when namespace can be imported
- **ALWAYS** add `using` statements for commonly used namespaces instead of FQCN
- **DRY Principle**: Don't duplicate functionality that already exists in base classes
- **Meaningful Naming**: Use descriptive names that explain business operations, not implementation details
- **Service-oriented**: Clean separation via interfaces (`IDocumentService`, etc.)
- **WebClientBase inheritance**: All services inherit common HTTP infrastructure
- **Token management**: Bearer/Basic tokens set per request (`Token.TokenType = TokenType.Bearer`) before request options
- **Validation extensions**: ID/email validation in `SignNow.Net.Internal.Extensions.ValidatorExtensions`

### SDK Infrastructure Discovery Rules
- **BEFORE** implementing any helper class, converter, or utility:
  1. Check `SignNow.Net._Internal.Helpers/` for existing implementations
  2. Check `SignNow.Net/_Internal/Helpers/Converters/` for existing JSON converters for data type serialization
  3. Check `SignNow.Net._Internal.Extensions/` for existing extension methods  
  4. Check base classes in `Service/` folder for existing patterns
- **Standard JSON Converters**: Use `Newtonsoft.Json.Converters.StringEnumConverter` for enum-to-string conversion
- **REUSE** existing SDK patterns and infrastructure whenever possible
- **EXTEND** existing classes/converters rather than creating duplicates

### Service Interface Rules
- **NEVER** add exception documentation (`/// <exception>`) to interface methods
- Interfaces should only document behavior, not implementation details
- Exception handling is implementation-specific and belongs in concrete classes
- Keep interface documentation focused on method purpose and parameters only

### Service Implementation Rules
- **ALWAYS** use `/// <inheritdoc />` for interface implementation
- **ADD** XML exception documentation (`/// <exception>`) only for exceptions thrown in THIS implementation
- **USE** existing extension methods for validation (e.g., `ValidateId()`) instead of custom validation blocks
- **NEVER** wrap `SignNowClient.RequestAsync()` calls in try/catch - SignNowClient handles all API errors internally
- **USE** `RequestOptions` pattern with SignNowClient for all API calls
- Let SignNowClient's `ProcessErrorResponse` handle API error mapping to Sign`NowException

### Service Implementation Pattern
```csharp
public class YourService : WebClientBase, IYourService
{
    public YourService(Uri baseApiUrl, Token token, ISignNowClient signNowClient = null)
        : base(baseApiUrl, token, signNowClient) { }

    public async Task<ResponseType> MethodAsync(string id, CancellationToken cancellationToken = default)
    {
        Token.TokenType = TokenType.Bearer; // Always set token type per request
        var requestOptions = new GetHttpRequestOptions
        {
            RequestUrl = new Uri(ApiBaseUrl, $"/endpoint/{id.ValidateId()}"), // Always validate IDs
            Token = Token
        };
        
        return await SignNowClient
            .RequestAsync<ResponseType>(requestOptions, cancellationToken)
            .ConfigureAwait(false); // Always use ConfigureAwait(false)
    }
}
```

### Validation Rules
- Use existing extension methods from `ValidatorExtensions`
- **ALWAYS** use extension method validation inline within the request URI or assignment: `documentId.ValidateId()`
- **NEVER** use separate validation code blocks or assign validation results to variables
- **NEVER** call validation methods on separate lines before usage
- Extension method validation should be fluent and integrated into the natural flow of code
- The `ValidateId()` and `ValidateEmail()` extension methods returns the validated value, allowing inline usage
- **Proper Guard Usage**: Use `Guard` class funcionality instead of manual null checking. Extend `Guard` class in `SignNow.Net/_Internal/Helpers/Guard.cs` for custom validations as needed

### Model Property Validation Rules
- **ADD** validation in property setters when API has specific value constraints
- Use `ArgumentOutOfRangeException` for numeric range validations
- Document valid ranges in XML comments with `<remarks>` sections
- Validate immediately when property is set, not during API calls

### Empty Response APIs
For endpoints returning empty bodies (202/204 status codes), return `Task` instead of custom response objects:

```csharp
// ✅ Correct
public async Task YourActionAsync(string id, CancellationToken cancellationToken = default)
{
    await SignNowClient.RequestAsync(requestOptions, cancellationToken).ConfigureAwait(false);
}
```

### SignNow Response Formats
All SignNow API Responses use JSON as the content type. The API has two versions: v1 and v2.
If an endpoint URL starts with v2, it belongs to version 2; otherwise, it is version 1.

Version 2 endpoints follow a strict response structure:

```json
{
  "data": {},
  "meta": {}
}
```

- The data field is required.
- If the response returns a single entity, `data` is an object and the `meta` field is omitted.
- If the response returns a collection of entities, `data` is an array and the `meta` field contains pagination details


### Method parameters & Request/Response Classes
- **Minimum parameters** for public methods - use request classes for complex operations, limit to 2-4 parameters max
- **Avoid FQNs in methods**: import class namespaces and avoid fully qualified class names in methods. Prefer short names for better readability
- **Request class naming**: do not use HTTP method names (e.g., `Get`, `Post`) in class names; use business operation names instead (e.g., `CreateDocumentRequest`)
- **Response class naming**: use business operation names (e.g., `DocumentResponse`, `UserResponse`); avoid generic names like `ApiResponse`
- **ALWAYS** use `= default` for optional reference type parameters instead of `= null`. This provides type-safe defaults and clearer intent.

### Property Assignment Rules
- **ALWAYS** set all required properties BEFORE using objects in complex operations
- **NEVER** modify object properties after they've been used in method calls or assignments
- Group all property assignments together at the beginning of method for clarity

### JSON Converter Discovery and Reuse Rules
- **ALWAYS** check existing converters in `SignNow.Net/Internal/Helpers/` before creating new ones
- **USE** existing SDK converters for common data types:
  - `UriConverter` for Uri properties (already exists in project)
  - `StringToEnumConverter` for enum to string conversions
  - Check `Internal/Helpers/` folder for all available converters
- **NEVER** create custom converters for data types that already have converters in the SDK
- **ADD** `[JsonConverter(typeof(ExistingConverter))]` attribute to properties using existing converters
- **CREATE** custom converters only when existing ones don't meet specific requirements and lace any new converters in the correct `_Internal/Helpers/` folder with proper namespace `SignNow.Net.Internal.Helpers.Converters`

### JSON Serialization Rules
- **ALWAYS** use `Newtonsoft.Json` attributes for JSON serialization (`[JsonProperty]`, `[JsonIgnore]`, etc.)
- **NEVER** use `[DataContract]` or `[DataMember]` attributes - these are for WCF/XML serialization
- All model classes should use `[JsonProperty(PropertyName = "api_field_name")]` for API field mapping
- **Enum Properties**: 
  - **USE** `StringEnumConverter` from Newtonsoft.Json: `[JsonConverter(typeof(StringEnumConverter))]`
  - Use `[EnumMember(Value = "api_value")]` on enum members for custom API string values
  - **NEVER** rely on default enum serialization (numeric or name-based)
- **URI Properties**: 
  - **ALWAYS** use `Uri` type for URL properties (never `string`)
  - **USE** existing `StringToUriJsonConverter`: `[JsonConverter(typeof(StringToUriJsonConverter))]`
  - **NEVER** create custom URI converters - use existing infrastructure

### URL and URI Handling Rules
- **URLs and URIs**: Always use `Uri` type for URL properties in models, never `string`
- **JSON Serialization**: Use `[JsonConverter(typeof(UriConverter))]` for proper JSON serialization of `Uri` properties
- **Nullable URLs**: Use `Uri?` for optional URL properties
- **Validation**: `Uri` type provides built-in validation and parsing capabilities


## Testing Guidelines
- **Faker pattern**: Every model needs a corresponding `ModelNameFaker : Faker<ModelName>` class
- **No trivial tests**: Don't test basic getters/setters or Faker functionality
- **Use TestUtils**: `TestUtils.DeserializeFromJson<T>()` and `TestUtils.SerializeToJsonFormatted()` for JSON deserialization/serialization purposes
- **Unit tests**: Use Mocked `ISignNowClient` from `SignNowTestBase.SignNowClientMock()`, verify correct endpoint calls
- Use meaningful test scenarios that validate actual SDK behavior
- **Tests Inheritance**: Use `SignNowTestBase` (for unit tests) or `AuthorizedApiTestBase` (for acceptance or feature tests)

### Unit Test Rules
- **ONE** test per feature - test only the service method behavior, not infrastructure
- **NO** tests for argument validation - these are covered by validator tests
- **NO** tests for null/empty parameters - validation is handled by extension methods
- **NO** tests for API error scenarios - this is SignNowClient's responsibility
- **NO** "Arrange/Act/Assert" comments in test code - the code structure should be self-evident
- Focus on testing successful execution path and proper method invocation only

### Acceptance Test Rules
- **FULL END-TO-END** scenarios only - create prerequisites, execute feature, verify results
- For delete operations: CREATE → VERIFY EXISTS → DELETE → VERIFY DELETED
- **NO** exception handling in acceptance tests - let tests fail fast on errors
- Test **ONE main feature** per test method
- Use real API calls, not mocks


## Examples Usage Rules
- **SIMPLEST** possible implementation for SDK customers
- **NO** comprehensive try/catch blocks - examples should show happy path
- **INCLUDE** all prerequisites explicitly in the example (document creation, etc.)
- Each example must be completely self-contained and runnable
- Avoid overengineering - customers need simple, clear examples


## Refactoring
- Refactor only when necessary to improve code quality or add features.
- Refactor class names to reflect business logic.
- Aviod code duplication by leveraging base classes and shared utilities.
- Detect and refactor Model classes that uses the same properties into a common base class.


## Changelog Rules
- **ADD** a changelog entry for every new feature, bug fix, or breaking change
- **Customer-Focused Changelog**: Focus on valuable functionality rather than implementation details
- **Simplified Descriptions**: Remove unnecessary technical details about tests and examples
- **Developer-Relevant Info**: Mention the new converters, tools, helpers and other common and reusable components as it's a reusable tool for developers