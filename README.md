# SignNow.NET

[![Build status][actions build badge]][actions build link]
[![Codecov][codecov badge]][codecov link]
[![Codacy][codacy badge]][codacy link]
[![NuGet Version][nuget badge]][nuget link]
[![NuGet Downloads][nuget downloads badge]][nuget downloads link]
[![License][license badge]](LICENSE)

## About SignNow

SignNow.Net is the official .NET 4.5+ and .NET Standard class library for the SignNow API. SignNow allows you to embed legally-binding e-signatures into your app, CRM or cloud storage. Send documents for signature directly from your website. Invite multiple signers to finalize contracts. Track status of your requests and download signed copies automatically.

Get your account at <https://www.signnow.com/developers>

## <a name="table-of-contents"></a>Table of Contents

1. [Get started](#get-started)
2. [Platform dependencies](#platform-dependencies)
3. [Installation](#installation)
4. [Documentation](#documentation)
5. [Features](#features)
    - [Authorization](#authorization)
        - [Request Access Token](#get-token)
        - [Verify Access Token][verify_access_token example]
        - [Refresh Access Token][refresh_access_token example]
    - [User](#user)
        - [Creates an account for a user](#create-user)
        - [Retrieve User Information][get_user_info example]
        - [Sends verification email to a user][send_verification example]
        - [Change User details][update_user example]
        - [Sends password reset link via email][reset_password example]
    - [Document](#document)
        - [Upload a document to SignNow](#upload-document)
        - [Upload a document & Extract Fields][upload_doc_extract example]
        - [Download a document from SignNow](#download-document)
        - [Retrieve a document resource][get_document example]
        - [Merge two or more SignNow documents into one](#merge-documents)
        - [Create a signing link to the document for signature](#create-signing-link)
        - [Create a freeform invite to the document for signature](#create-freeform-invite)
        - [Create a role-based invite to the document for signature](#create-role-based-invite)
        - [Create embedded signing invite to the document for signature](#create-embedded-invite)
        - [Create a one-time link to download the document as a PDF](#share-document-via-link)
        - [Get the history of a document](#document-history)
        - [Check the status of the document][check_sign_status example]
        - [Create a template by flattening an existing document][create_template example]
        - [Create a document from the template][create_document example]
6. [Contribution guidelines](#contribution-guidelines)
    - [XML doc generation](#xml-doc-generation)
    - [Important notes](#important-notes)
7. [License](#license)

## <a name="get-started"></a>Get started

To start using the API  you will need an API key. You can get one here <https://www.signnow.com/api>. For a full list of accepted parameters, refer to the SignNow REST Endpoints API guide: [SignNow API Reference][api reference link].

#### API and Application

| Resources    | Sandbox                        | Production                |
| ------------ | ------------------------------ | ------------------------- |
| API:         | api-eval.signnow.com:443       | api.signnow.com:443       |
| Application: | <https://app-eval.signnow.com> | <https://app.signnow.com> |
| Entry page:  | <https://eval.signnow.com>     |                           |

## <a name="platform-dependencies"></a>Platform Dependencies

#### Windows

- .Net Framework 4.5 or newer version should be installed in your system, or
- .Net Core 3.0 and newer

#### MacOS and Linux

- .Net Core 3.0 and newer

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

Read about the available SignNow features in [SignNow API Docs][api docs link].

## <a name="features"></a>Features

## <a name="authorization"></a>Authorization

#### <a name="get-token"></a>Request Access Token

Get your access token via OAuth 2.0 service.

```csharp
public static class AuthenticationExamples
{
    /// <summary>
    /// An example of obtaining an access token via OAuth 2.0 service.
    /// </summary>
    /// <param name="apiBase">SignNow API base URL. Sandbox: "https://api-eval.signnow.com", Production: "https://api.signnow.com"</param>
    /// <param name="clientInfo"><see cref="CredentialModel"/> with Application Client ID and Client Secret</param>
    /// <param name="userCredentials"><see cref="CredentialModel"/> with User email and User password</param>
    public static async Task<Token> RequestAccessToken(Uri apiBase, CredentialModel clientInfo, CredentialModel userCredentials)
    {
        Uri apiBaseUrl = apiBase;

        string clientId = clientInfo.Login;
        string clientSecret = clientInfo.Password;

        string userLogin = userCredentials.Login;
        string userPassword = userCredentials.Password;

        var oauth = new OAuth2Service(apiBaseUrl, clientId, clientSecret);

        return await oauth.GetTokenAsync(userLogin, userPassword, Scope.All)
            .ConfigureAwait(false);
    }
}
```

More examples: [Request Access token][request_access_token example], [Verify Access Token][verify_access_token example], [Refresh Access Token][refresh_access_token example]

## <a name="user"></a>User

### <a name="create-user"></a>Creates an account for a user

By default verification email is not sent to newly created User.
To send it - use `IUserService.SendVerificationEmailAsync(string email)`

```csharp
public static partial class UserExamples
{
    /// <summary>
    /// Creates an account for a user example
    /// </summary>
    /// <param name="firstname">User firstname</param>
    /// <param name="lastname">User lastname</param>
    /// <param name="email">User email</param>
    /// <param name="password">User password</param>
    /// <param name="token">Access token</param>
    /// <returns>
    /// Response with: User identity, email
    /// </returns>
    public static async Task<UserCreateResponse> CreateSignNowUser(string firstname, string lastname, string email, string password, Token token)
    {
        var signNowContext = new SignNowContext(token);

        var userRequest = new CreateUserOptions
        {
            Email = email,
            FirstName = firstname,
            LastName = lastname,
            Password = password
        };

        return await signNowContext.Users
            .CreateUserAsync(userRequest)
            .ConfigureAwait(false);
    }
}
```

More examples: [Create User][create_user example], [Retrieve User information][get_user_info example], [Sends verification email to a user][send_verification example], [Change User details][update_user example], [Sends password reset link via email][reset_password example]

## <a name="document"></a>Document

### <a name="upload-document"></a>Upload a document to SignNow

All the features in SignNow require a `document_id`. Once you upload a document to SignNow, you get the `document_id` from a successful response.

```csharp
public static class DocumentExamples
{
    /// <summary>
    /// Uploads a PDF document to SignNow and returns SignNowDocument object.
    /// </summary>
    /// <param name="pdfFilePath">Full qualified path to your PDF file.</param>
    /// <param name="token">Access token</param>
    public static async Task<SignNowDocument> UploadDocument(string pdfFilePath, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);
        var pdfFileName = "document-example.pdf";
        
        await using var fileStream = File.OpenRead(pdfFilePath);
        
        // Upload the document
        var uploadResponse = signNowContext.Documents
            .UploadDocumentAsync(fileStream, pdfFileName).Result;

        // Gets document ID from successful response
        var documentId = uploadResponse.Id;

        return await signNowContext.Documents.GetDocumentAsync(documentId);
    }
}
```

More examples: [Upload document][upload_document example], [Upload document with field extract][upload_doc_extract example]

### <a name="download-document"></a>Download a document from SignNow

Choose the type of download for your document:

- `PdfOriginal` - download a document in a state it's been when uploaded to SignNow, before any changes
- `PdfCollapsed` - download a document in PDF file format
- `ZipCollapsed` - download a document in ZIP archive
- `PdfWithHistory` - download a document with its history, a full log of changes on a separate page.

```csharp
public static class DocumentExamples
{
    /// <summary>
    /// Downloads signed document.
    /// <see cref="DownloadDocumentResponse"/> contains: file name, file length, Stream content
    /// </summary>
    /// <param name="documentId">ID of signed document</param>
    /// <param name="token">Access token</param>
    public static async Task<DownloadDocumentResponse> DownloadDocument(string documentId, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);
        
        // using `documentId` from the Upload document step
        return await signNowContext.Documents
                        .DownloadDocumentAsync(documentId, DownloadType.PdfCollapsed)
                        .ConfigureAwait(false);
    }
}
```

More examples: [Download signed document][download_signed_doc example]

### <a name="merge-documents"></a>Merge two or more SignNow documents into one

Merges two or more SignNow documents into one single PDF file.

Steps:

▶ Upload documents or Get document IDs of the documents you’d like to merge

▶ Merge the documents

Merged document will be saved in PDF file format.

```csharp
public static partial class DocumentExamples
{
    /// <summary>
    /// Merge two documents into one final document
    /// </summary>
    /// <param name="documentName">New Document name with extension</param>
    /// <param name="documentsList">List of the documents to be merged</param>
    /// <param name="token">Access token</param>
    /// <returns>
    /// <see cref="DownloadDocumentResponse"/> contains: file name, file length, Stream content
    /// </returns>
    public static async Task<DownloadDocumentResponse>
        MergeTwoDocuments(string documentName, IEnumerable<SignNowDocument> documentsList, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        return await signNowContext.Documents
            .MergeDocumentsAsync(documentName, documentsList)
            .ConfigureAwait(false);
    }
}
```

More examples: [Merge document][merge_documents example]

### <a name="create-signing-link"></a>Create a signing link to the document for signature

Signing link - a single-use link to a document that requires a signature. When the document is signed (or the signer declines to sign it), the link is no longer valid.
Having followed the link, signers can click anywhere on the document to sign it.

Steps:

▶ Upload a document or Get the ID of the document you’d like to have signed

▶ Send a signing link

```csharp
public static partial class DocumentExamples
{
    /// <summary>
    /// Create a signing link to the document for signature.
    /// </summary>
    /// <param name="documentId">Identity of the document you’d like to have signed</param>
    /// <param name="token">Access token</param>
    /// <returns>
    /// Response with:
    /// <para> <see cref="SigningLinkResponse.Url"/>
    /// to sign the document via web browser using SignNow credentials. </para>
    /// <para> <see cref="SigningLinkResponse.AnonymousUrl"/>
    /// to sign the document via web browser without SignNow credentials. </para>
    /// </returns>
    public static async Task<SigningLinkResponse>
        CreateSigningLinkToTheDocument(string documentId, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        // using `documentId` from the Upload document step
        return await signNowContext.Documents
            .CreateSigningLinkAsync(documentId).ConfigureAwait(false);
    }
}
```

More examples: [Create signing link][create_sign_lnk example], [Check signing status][check_sign_status example], [Create a template by flattening an existing document ][create_template example], [Create a document from the template][create_document example]

### <a name="create-freeform-invite"></a>Create a freeform invite to the document for signature

*Freeform invite* - an invitation to sign a document which doesn’t contain any fillable fields.

Simply upload a document and send it for signature right away. No need for adding fields and configuring roles.
Just add the signer's email address and customize the message in your email.
The document will be available for signing via a button in the email.
Clicking the button opens a document in SignNow editor. Signers can click anywhere on a document to add their signature.

Remember: if your document contains even one fillable field, you have to create a role-based invite to get it signed.

```csharp
public static partial class InviteExamples
{
    /// <summary>
    /// Create a freeform invite to the document for signature.
    /// </summary>
    /// <param name="document">SignNow document you’d like to have signed</param>
    /// <param name="email">The email of the invitee.</param>
    /// <param name="token">Access token</param>
    /// <returns>
    /// <see cref="InviteResponse"/> which contains an Identity of invite request.
    /// </returns>
    public static async Task<InviteResponse>
        CreateFreeformInviteToSignTheDocument(SignNowDocument document, string email, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        // Create freeform invite
        var invite = new FreeFormSignInvite(email)
        {
            Message = $"{email} invited you to sign the document {document.Name}",
            Subject = "The subject of the Email"
        };

        // Creating Invite request
        return await signNowContext.Invites.CreateInviteAsync(document.Id, invite).ConfigureAwait(false);
    }
}
```

More examples: [Create freeform invite][create_ff_invite example]

### <a name="create-role-based-invite"></a>Create a role-based invite to the document for signature

*Role-based invite* - an invitation to sign a document which contains at least one fillable field assigned to one role.

Role-based invites allow you to build e-signature workflows. The document can be signed by multiple signers: each with individual access settings, all arranged in a specific order.

Upload a document or create one from a template.

The document will be available for signing via a button in the email. You can customize email messages for every signer.
Clicking the button opens a document in SignNow editor. Signers can sign only the fields designated for their role.

You can add more roles either in SignNow web app while editing the fields, or with `ISignInvite` interface from SDK while specifying parameters of the `SignerOptions` object.

```csharp
public static partial class InviteExamples
{
    /// <summary>
    /// Create a role-based invite to the document for signature.
    /// </summary>
    /// <param name="document">SignNow document with fields you’d like to have signed</param>
    /// <param name="email">The email of the invitee.</param>
    /// <param name="token">Access token</param>
    /// <returns><see cref="InviteResponse"/> without any Identity of invite request.</returns>
    public static async Task<InviteResponse>
        CreateRoleBasedInviteToSignTheDocument(SignNowDocument document, string email, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        // Create role-based invite
        var invite = new RoleBasedInvite(document)
        {
            Message = $"{email} invited you to sign the document {document.Name}",
            Subject = "The subject of the Email"
        };

        // Creates options for signers
        var signer = new SignerOptions(email, invite.DocumentRoles().First())
            {
                ExpirationDays = 15,
                RemindAfterDays = 7,
            }
            .SetAuthenticationByPassword("***PASSWORD_TO_OPEN_THE_DOCUMENT***");

        // Attach signer to existing roles in the document
        invite.AddRoleBasedInvite(signer);

        // Creating Invite request
        return await signNowContext.Invites.CreateInviteAsync(document.Id, invite).ConfigureAwait(false);
    }
}
```

More examples: [Create role-based invite][create_rb_invite example]

### <a name="create-embedded-invite"></a>Create embedded signing invite to the document for signature

```csharp
public static partial class InviteExamples
{
    /// <summary>
    /// Create an embedded signing invite to the document for signature.
    /// </summary>
    /// <param name="document">SignNow document you’d like to have signed</param>
    /// <param name="email">The email of the invitee.</param>
    /// <param name="token">Access token</param>
    /// <returns>
    /// <see cref="EmbeddedInviteResponse"/> which contains an invite data.
    /// </returns>
    public static async Task<EmbeddedInviteResponse>
        CreateEmbeddedSigningInviteToSignTheDocument(SignNowDocument document, string email, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        // create embedded signing invite
        var invite = new EmbeddedSigningInvite(document);
        invite.AddEmbeddedSigningInvite(
            new EmbeddedInvite
            {
                Email = email,
                RoleId = document.Roles[0].Id,
                SigningOrder = 1
            });

        return await signNowContext.Invites.CreateInviteAsync(document.Id, invite)
            .ConfigureAwait(false);
    }
}
```

More examples: [Generate embedded signing link][generate_embedded_link example], [Cancel embedded signing invite][cancel_embedded_invite example]

### <a name="share-document-via-link"></a>Create a one-time link to download the document as a PDF

```csharp
public static partial class DocumentExamples
{
    /// <summary>
    /// Create a one-time use URL for anyone to download the document as a PDF.
    /// </summary>
    /// <param name="documentId">Identity of the document</param>
    /// <param name="token">Access token</param>
    /// <returns><see cref="DownloadLinkResponse"/></returns>
    public static async Task<DownloadLinkResponse>
        CreateOneTimeLinkToDownloadTheDocument(string documentId, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        return await signNowContext.Documents
            .CreateOneTimeDownloadLinkAsync(documentId)
            .ConfigureAwait(false);
    }
}
```

More examples: [Create a One-time Use Download URL][create_one_time_link example]

### <a name="document-history"></a>Get the history of a document

```csharp
public static partial class DocumentExamples
{
    /// <summary>
    /// Retrieve the history of a document.
    /// </summary>
    /// <param name="documentId">Identity of the document</param>
    /// <param name="token">Access token</param>
    /// <returns><see cref="DocumentHistoryResponse"/></returns>
    public static async Task<IReadOnlyList<DocumentHistoryResponse>>
        GetTheDocumentHistory(string documentId, Token token)
    {
        // using token from the Authorization step
        var signNowContext = new SignNowContext(token);

        return await signNowContext.Documents
            .GetDocumentHistoryAsync(documentId)
            .ConfigureAwait(false);
    }
}
```

More examples: [Get document history][document_history example]


## <a name="contribution-guidelines"></a>Contribution guidelines

### <a name="xml-doc-generation"></a>XML doc generation

For XML documentation generation, install InheritDocTool:

```bash
dotnet tool install -g InheritDocTool
```

More about the InheritDoc [here](https://www.inheritdoc.io)

### <a name="important-notes"></a>Important notes

Thanks to all contributors who got interested in this project. We're excited to hear from you. Here are some tips to make our collaboration meaningful and bring its best results to life:

- We accept pull requests from the community. Please, send your pull requests to the **DEVELOP branch** which is the consolidated work-in-progress branch. We don't accept requests to the Master branch.

- Please, check in with the documentation first before you open a new Issue.

- When suggesting new functionality, give as many details as possible. Add a test or code example if you can.

- When reporting a bug, please, provide full system information. If possible, add a test that helps us reproduce the bug. It will speed up the fix significantly.

## <a name="license"></a>License

This SDK is distributed under the MIT License, see [LICENSE][license link] for more information.

#### API Contact Information

If you have questions about the SignNow API, please visit [SignNow API Reference][api reference link] or email api@signnow.com.<br>

**Support**: To contact SignNow support, please email support@signnow.com or api@signnow.com.<br>

**Sales**: For pricing information, please call (800) 831-2050, email sales@signnow.com or visit <https://www.signnow.com/contact>.

<!-- Aliases for URLs: please place here any long urls to keep clean markdown markup -->
[actions build badge]: https://github.com/signnow/SignNow.NET/workflows/Build%20and%20Test/badge.svg "Build status"
[actions build link]: https://github.com/signnow/SignNow.NET/actions?query=workflow%3A%22Build+and+Test%22
[codecov badge]: https://codecov.io/gh/signnow/SignNow.NET/branch/develop/graph/badge.svg "Code coverage report"
[codecov link]: https://codecov.io/gh/signnow/SignNow.NET
[codacy badge]: https://api.codacy.com/project/badge/Grade/1aea9e4b60eb4b6a8c458e16fc8bdb24 "Codacy Repository certification"
[codacy link]: https://app.codacy.com/manual/AlexNDRmac/SignNow.NET?utm_source=github.com&utm_medium=referral&utm_content=signnow/SignNow.NET&utm_campaign=Badge_Grade_Dashboard
[nuget badge]: https://img.shields.io/nuget/v/SignNow.Net.svg?style=flat-square "NuGet package latest SDK version"
[nuget link]: https://www.nuget.org/packages/SignNow.Net
[nuget downloads badge]: https://img.shields.io/nuget/dt/SignNow.Net.svg?style=flat-square
[nuget downloads link]: https://www.nuget.org/packages/SignNow.Net "NuGet Downloads"
[license badge]: https://img.shields.io/github/license/signnow/SignNow.NET?style=flat-square "SignNow .Net SDK License"
[license link]: https://github.com/signnow/SignNow.NET/blob/develop/LICENSE
[api docs link]: https://docs.signnow.com
[api reference link]: https://docs.signnow.com/sn/ref

<!-- All examples URLs should be there -->
<!-- Authorization -->
[request_access_token example]:     https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Authentication/RequestAccessToken.cs#L16
[verify_access_token example]:      https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Authentication/RequestAccessToken.cs#39
[refresh_access_token example]:     https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Authentication/RequestAccessToken.cs#54

<!-- Users -->
[create_user example]:              https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Users/CreateSignNowUser.cs
[get_user_info example]:            https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Users/CreateSignNowUser.cs#42
[send_verification example]:        https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Users/SendVerificationEmailToUser.cs
[update_user example]:              https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Users/ChangeUserDetails.cs#18
[reset_password example]:           https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Users/ChangeUserDetails.cs#40

<!-- Documents -->
[upload_document example]:          https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/UploadDocument.cs#33
[upload_doc_extract example]:       https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/UploadDocument.cs#14
[download_signed_doc example]:      https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/DownloadSignedDocument.cs
[get_document example]:             https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/UploadDocument.cs#48
[merge_documents example]:          https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/MergeTwoDocuments.cs
[create_sign_lnk example]:          https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/CreateSigningLinkToTheDocument.cs
[create_ff_invite example]:         https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Invites/CreateFreeformInviteToSignTheDocument.cs
[create_rb_invite example]:         https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Invites/CreateRoleBasedInviteToSignTheDocument.cs
[generate_embedded_link example]:   https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Invites/GenerateLinkForEmbeddedInvite.cs
[cancel_embedded_invite example]:   https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Invites/CancelEmbeddedInvite.cs
[check_sign_status example]:        https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/CheckTheStatusOfTheDocument.cs
[create_one_time_link example]:     https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/CreateOneTimeLinkToDownloadTheDocument.cs
[document_history example]:         https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/GetTheDocumentHistory.cs
[create_template example]:          https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/CreateTemplateFromTheDocument.cs
[create_document example]:          https://github.com/signnow/SignNow.NET/blob/develop/SignNow.Net.Examples/Documents/CreateDocumentFromTheTemplate.cs
