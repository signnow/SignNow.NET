# SignNow.NET

[![Build status](https://github.com/signnow/SignNow.NET/workflows/Build%20and%20Test/badge.svg "Build status")](https://github.com/signnow/SignNow.NET/actions?query=workflow%3A%22Build+and+Test%22) [![codecov](https://codecov.io/gh/signnow/SignNow.NET/branch/develop/graph/badge.svg "Code coverage report")](https://codecov.io/gh/signnow/SignNow.NET) [![NuGet](https://img.shields.io/nuget/v/SignNow.Net.svg?style=flat-square "NuGet package latest SDK version")](https://www.nuget.org/packages/SignNow.Net) [![NuGet Downloads](https://img.shields.io/nuget/dt/SignNow.Net.svg?style=flat-square)](https://www.nuget.org/packages/SignNow.Net "NuGet Downloads") [![License](https://img.shields.io/github/license/signnow/SignNow.NET?style=flat-square "SignNow .Net SDK License")](LICENSE)

## About SignNow

SignNow.Net is the official .NET 4.5+ class library for the SignNow API. SignNow allows you to embed legally-binding e-signatures into your app, CRM or cloud storage. Send documents for signature directly from your website. Invite multiple signers to finalize contracts. Track status of your requests and download signed copies automatically.

Get your account at https://www.signnow.com/developers

## Contents
1. [Get started](#get-started)
2. [Platform dependencies](#platform-dependencies)
3. [XML doc generation](#xml-doc-generation)
4. [Installation](#installation)
5. [Documentation](#documentation)
6. [Features](#features)
    * [Authorization](#authorize)
    * [Upload a document to SignNow](#upload-document)
    * [Download a document from SignNow](#download-document)
    * [Create a single-use link to the document for signature](#create-signing-link)
7. [Important notes](#important-notes)
8. [License](#license)


## <a name="get-started"></a>Get started
  * Get a SignNow account [here](https://www.signnow.com/developers) 
  * SignNow uses Authorization: Basic <`credentials`>. To get your access token, have your client ID and secret base-64 encoded first


## <a name="platform-dependencies"></a>Platform Dependencies
#### Windows
  * .Net Framework 4.5 or newer version should be installed in your system, or
  * .Net Standard 1.2 or 2.0, or
  * .Net Core 2.2 and newer

#### MacOS and Linux
  * .Net Core 2.2 and newer


## <a name="xml-doc-generation"></a>XML doc generation

For XML documentation generation, install InheritDocTool (the project build will fail without it):

```bash
dotnet tool install -g InheritDocTool
```

More about the InheritDoc [here](https://www.inheritdoc.io)


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

Read about the available SignNow features in [SignNow API Docs](https://github.com/signnow/SignNow.NET.wiki.git) .


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
}
```

### <a name="upload-document"></a> Upload a document to SignNow

All the features in SignNow require a `document_id`. Once you upload a document to SignNow, you get the `document_id` from a successful response.
```csharp
string documentId = default;

var pdfFilePath = "./file-path/document-example.pdf";
var pdfFileName = "document-example.pdf";

/// using token from the Authorization step
var documentService = new DocumentService(token);

using (var fileStream = File.OpenRead(pdfFilePath))
{
    var uploadResponse = documentService.UploadDocumentAsync(fileStream, pdfFileName).Result;

    documentId = uploadResponse.Id;
}
```

### <a name="download-document"></a> Download a document from SignNow

Choose the type of download for your document:
* `PdfOriginal` - download a document in a state it's been when uploaded to SignNow, before any changes
* `PdfCollapsed` - download a document in PDF file format
* `ZipCollapsed` - download a document in ZIP archive
* `PdfWithHistory` - download a document with its history, a full log of changes on a separate page.

```csharp
/// using `documentId` from the Upload document step

var downloadPdf = documentService.DownloadDocumentAsync(documentId, downloadType.PdfCollapsed).Result;

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
/// using `documentId` from the Upload document step

var signingLinks = documentService.CreateSigningLinkAsync(documentId).Result;

Console.WriteLine("Authorize and Sign the Document" + signingLinks.Url);
Console.WriteLine("Sign the Document" + signingLinks.AnonymousUrl);
```

## <a name="important-notes"></a>Important notes

Thanks to all contributors who got interested in this project. We're excited to hear from you. Here are some tips to make our collaboration meaningful and bring its best results to life:

* We accept pull requests from the community. Please, send your pull requests to the **DEVELOP branch** which is the consolidated work-in-progress branch. We don't accept requests to the Master branch.

* Please, check in with the documentation first before you open a new Issue.

* When suggesting new functionality, give as many details as possible. Add a test or code example if you can.

* When reporting a bug, please, provide full system information. If possible, add a test that helps us reproduce the bug. It will speed up the fix significantly.


## <a name="license"></a>License

This SDK is distributed under the MIT License,  see [LICENSE](https://github.com/signnow/SignNow.NET/blob/develop/LICENSE) for more information.