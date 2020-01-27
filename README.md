# SignNow.NET

[![Build status](https://github.com/signnow/SignNow.NET/workflows/Build%20and%20Test/badge.svg "Build status")](https://github.com/signnow/SignNow.NET/actions?query=workflow%3A%22Build+and+Test%22) [![codecov](https://codecov.io/gh/signnow/SignNow.NET/branch/develop/graph/badge.svg "Code coverage report")](https://codecov.io/gh/signnow/SignNow.NET) [![NuGet](https://img.shields.io/nuget/v/SignNow.Net.svg?style=flat-square "NuGet package latest SDK version")](https://www.nuget.org/packages/SignNow.Net) [![NuGet Downloads](https://img.shields.io/nuget/dt/SignNow.Net.svg?style=flat-square)](https://www.nuget.org/packages/SignNow.Net "NuGet Downloads") [![License](https://img.shields.io/github/license/signnow/SignNow.NET?style=flat-square "SignNow .Net SDK License")](LICENSE)

## About SignNow

SignNow.Net is the official .NET 4.5+ and .NET Standard class library for the SignNow API. SignNow allows you to embed legally-binding e-signatures into your app, CRM or cloud storage. Send documents for signature directly from your website. Invite multiple signers to finalize contracts. Track status of your requests and download signed copies automatically.

Get your account at <https://www.signnow.com/developers>

## Contents

1.  [Get started](#get-started)
2.  [Platform dependencies](#platform-dependencies)
3.  [Installation](#installation)
4.  [Documentation](#documentation)
5.  [Features](#features)
    -   [Authorization](#authorization)
    -   [Upload a document to SignNow](#upload-document)
    -   [Download a document from SignNow](#download-document)
    -   [Create a single-use link to the document for signature](#create-signing-link)
    -   [Create a freeform invite to the document for signature](#create-freeform-invite)
    -   [Create a role-based invite to the document for signature](#create-role-based-invite)
6.  [Contribution guidelines](#contribution-guidelines)
    -   [XML doc generation](#xml-doc-generation)
    -   [Important notes](#important-notes)
7.  [License](#license)

## <a name="get-started"></a>Get started

To start using the API  you will need an API key. You can get one here <https://www.signnow.com/api>. For a full list of accepted parameters, refer to the SignNow REST Endpoints API guide: <https://docs.signnow.com/reference>.

#### API and Application

| Resources    | Sandbox                        | Production                |
| ------------ | ------------------------------ | ------------------------- |
| API:         | api-eval.signnow.com:443       | api.signnow.com:443       |
| Application: | <https://app-eval.signnow.com> | <https://app.signnow.com> |
| Entry page:  | <https://eval.signnow.com>     |                           |

## <a name="platform-dependencies"></a>Platform Dependencies

#### Windows

-   .Net Framework 4.5 or newer version should be installed in your system, or
-   .Net Core 2.0 and newer

#### MacOS and Linux

-   .Net Core 2.0 and newer

## <a name="installation"></a>Installation

### Install .Net package

```bash
dotnet add package SignNow.Net --version <package-version>
```

### Install using Package manager

```powershell
Install-Package SignNow.Net -Version <package-version>
```

## <a name="documentation"></a>Documentation

Read about the available SignNow features in [SignNow API Docs](https://docs.signnow.com).

## <a name="features"></a>Features

### <a name="authorization"></a> Authorization

Get your access token via OAuth 2.0 service.

```csharp
string clientId = "0fa****-EXAMPLE_CLIENT_ID-****13";
string clientSecret = "0fb**-EXAMPLE_CLIENT_SECRET-**13";

string userLogin = "signnow_dotnet_sdk@example.com";
string userPassword = "example-user-password";

var oauth = new OAuth2Service(clientId, clientSecret);

var token = oauth.GetTokenAsync(userLogin, userPassword, Scope.All).Result;
```

### <a name="upload-document"></a> Upload a document to SignNow

All the features in SignNow require a `document_id`. Once you upload a document to SignNow, you get the `document_id` from a successful response.

```csharp
string documentId = default;

var pdfFilePath = "./file-path/document-example.pdf";
var pdfFileName = "document-example.pdf";

// using token from the Authorization step
var SignNow = new SignNowContext(token);

using (var fileStream = File.OpenRead(pdfFilePath))
{
    var uploadResponse = SignNow.Documents.UploadDocumentAsync(fileStream, pdfFileName).Result;

    documentId = uploadResponse.Id;
}
```

### <a name="download-document"></a> Download a document from SignNow

Choose the type of download for your document:

-   `PdfOriginal` - download a document in a state it's been when uploaded to SignNow, before any changes
-   `PdfCollapsed` - download a document in PDF file format
-   `ZipCollapsed` - download a document in ZIP archive
-   `PdfWithHistory` - download a document with its history, a full log of changes on a separate page.

```csharp
// using `documentId` from the Upload document step
var downloadPdf = SignNow.Documents.DownloadDocumentAsync(documentId, DownloadType.PdfCollapsed).Result;

using (FileStream output = new FileStream(@"./outputDir/" + downloadPdf.Filename, FileMode.Create))
{
  downloadPdf.Document.CopyTo(output);
}

Console.WriteLine("Downloaded successful: " + downloadPdf.Filename);
```

### <a name="create-signing-link"></a> Create a single-use link to the document for signature

You generate a link for sharing the document to be signed. It's called signing link in SignNow. Having followed the link, signers can click anywhere on the document to sign it.

Steps:

▶ Upload a document or Get the ID of the document you’d like to have signed

▶ Send a signing link

```csharp
// using `documentId` from the Upload document step
var signingLinks = SignNow.Documents.CreateSigningLinkAsync(documentId).Result;

Console.WriteLine("Authorize and Sign the Document" + signingLinks.Url);
Console.WriteLine("Sign the Document" + signingLinks.AnonymousUrl);
```

### <a name="create-freeform-invite"></a> Create a freeform invite to the document for signature

You can generate signer personalized email message with link for sharing the document to be signed. Signers can click anywhere on a document to add their signature. It's called freeform invite in SignNow.
> Remember: freeform invites only work for documents with no fillable fields.

```csharp
var invite = new FreeFormInvite("signer@signnow.com");

// using `documentId` from the Upload document step
// creating Invite request
var inviteResponse = SignNow.Invites.CreateInviteAsync(documentId, invite).Result;

// Check if invite was successful
var inviteId = inviteResponse.Id; // "a09b26feeba7ce70228afe6290f4445700b6f349"
```

### <a name="create-role-based-invite"></a> Create a role-based invite to the document for signature

You can generate signer personalized email messages with link for sharing the document to be signed. It's called role-based invite in SignNow. You can add more roles either in SignNow web app while editing the fields, or with `ISignInvite` interface from SDK while specifying parameters of the `SignerOptions` object.

```csharp
// For example, let use document with only two fillable fields
var pdfFilePath = "./file-path/document-with-fields.pdf";

// Upload document with fillable fields and extract fields
using (var fileStream = File.OpenRead(pdfFilePath))
{
    var DocumentId = SignNow.Documents.UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithFields.pdf").Result.Id;
}

// Get the SignNow document instance by uploaded DocumentId
var document = SignNow.Documents.GetDocumentAsync(DocumentId).Result;

// Create role-based invite using document with fillable fields
var roleBasedInvite = new RoleBasedInvite(document);

// Get all document signer roles
var roles = roleBasedInvite.DocumentRoles();

// Creates options for signers
var signer1 = new SignerOptions("signer1@signnow.com", roles.First());
var signer2 = new SignerOptions("signer2@signnow.com", roles.Last())
    {
        ExpirationDays = 15,
        RemindAfterDays = 7
    }
    .SetAuthenticationByPassword("***secret***");

// Attach signers to existing roles in the document
roleBasedInvite.AddRoleBasedInvite(signer1);
roleBasedInvite.AddRoleBasedInvite(signer2);

// Send invite request for sharing the document to be signed
var inviteResponse = SignNow.Invites.CreateInviteAsync(DocumentId, invite).Result;

// Finaly - check if document has invite request
var documentUpdated = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
var fieldInvites = documentUpdated.FieldInvites.First();

// Get field invite request status for the first signer
var status = fieldInvites.Status.ToString(); // "Pending"
var roleName = fieldInvites.RoleName; // "Signer 1"
var signer1email = fieldInvites.Email; // "signer1@signnow.com"
```

## <a name="contribution-guidelines"></a>Contribution guidelines

### <a name="xml-doc-generation"></a>XML doc generation

For XML documentation generation, install InheritDocTool (the project build will fail without it):

```bash
dotnet tool install -g InheritDocTool
```

More about the InheritDoc [here](https://www.inheritdoc.io)

### <a name="important-notes"></a>Important notes

Thanks to all contributors who got interested in this project. We're excited to hear from you. Here are some tips to make our collaboration meaningful and bring its best results to life:

-   We accept pull requests from the community. Please, send your pull requests to the **DEVELOP branch** which is the consolidated work-in-progress branch. We don't accept requests to the Master branch.

-   Please, check in with the documentation first before you open a new Issue.

-   When suggesting new functionality, give as many details as possible. Add a test or code example if you can.

-   When reporting a bug, please, provide full system information. If possible, add a test that helps us reproduce the bug. It will speed up the fix significantly.

## <a name="license"></a>License

This SDK is distributed under the MIT License, see [LICENSE](https://github.com/signnow/SignNow.NET/blob/develop/LICENSE) for more information.

#### API Contact Information

If you have questions about the SignNow API, please visit <https://docs.signnow.com/reference> or email api@signnow.com.<br>

**Support**: To contact SignNow support, please email support@signnow.com or api@signnow.com.<br>

**Sales**: For pricing information, please call (800) 831-2050, email sales@signnow.com or visit <https://www.signnow.com/contact>.
