# SignNow.NET

[![NuGet](https://img.shields.io/nuget/v/SignNow.Net.svg?style=flat-square)](https://www.nuget.org/packages/SignNow.Net) [![License](https://img.shields.io/github/license/signnow/SignNow.NET?style=flat-square)](LICENSE)

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
    * [Send a freeform invite to sign a document](#freeform-invite)
7. [Important notes](#important-notes)
8. [License](#license)


## <a name="get-started"></a>Get started
  * Get a SignNow account [here](https://www.signnow.com/developers) 
  * SignNow uses Authorization: Basic <credentials>, so to get your access token, have your client ID and secret base-64 encoded first


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
using System;
using System.Threading.Tasks;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace AuthorizationExample
{
    public static class OAuthServiceExample
    {
        public static async Task<Token> GetAccessToken()
        {
            Uri ApiBaseUrl = new Uri("https://api.signnow.com");
            
            /// Register your account first
            /// <see cref="https://www.signnow.com/developers">
            string clientId = "0fa****-EXAMPLE_CLIENT_ID-****13";
            string clientSecret = "0fb**-EXAMPLE_CLIENT_SECRET-**13";

            string userLogin = "signnow_dotnet_sdk@example.com";
            string userPassword = "example-user-password";
            
            var oauth = new OAuth2Service(ApiBaseUrl, clientId, clientSecret);

            return await oauth.GetTokenAsync(userLogin, userPassword, Scope.All).ConfigureAwait(false);
        }
    }
}
```

### <a name="upload-document"></a> Upload a document to SignNow

All the features in SignNow require a `document_id`. Once you upload a document to SignNow, you get the `document_id` from a successful response.
```csharp
/// First step: Authorize
```

### <a name="download-document"></a> Download a document from SignNow

Choose the type of download for your document:
* `PdfOriginal` - download a document in a state it's been when uploaded to SignNow, before any changes
* `PdfCollapsed` - download a document in PDF file format
* `ZipCollapsed` - download a document in ZIP archive
* `PdfWithHistory` - download a document with its history, a full log of changes on a separate page.

```csharp
///
```

### <a name="freeform-invite"></a> Send a freeform invite to sign a document 

**Freeform invite** - an invitation to sign a document which doesn’t contain any fillable fields.

Signers can click anywhere on a document to add their signature.

Steps

▶ Upload a document or Get the ID of the document you’d like to have signed

▶ Send a freeform invite


```csharp
///
```


## <a name="important-notes"></a>Important notes

Thanks to all contributors who got interested in this project. We're excited to hear from you. Here are some tips to make our collaboration meaningful and bring its best results to life:

* We accept pull requests from the community. Please, send your pull requests to the **DEVELOP branch** which is the consolidated work-in-progress branch. We don't accept requests to the Master branch.

* Please, check in with the documentation first before you open a new Issue.

* When suggesting new functionality, give as many details as possible. Add a test or code example if you can.

* When reporting a bug, please, provide full system information. If possible, add a test that helps us reproduce the bug. It will speed up the fix significantly.


## <a name="license"></a>License

This SDK is distributed under the MIT License,  see [LICENSE.txt](https://github.com/signnow/SignNow.NET/blob/develop/LICENSE) for more information.