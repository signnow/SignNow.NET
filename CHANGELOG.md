# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com)
and this project adheres to [Semantic Versioning](http://semver.org).

## [Unreleased] - TBD

## [1.0.0] - 2022-02-26
### Added
- `IDocumentService.PrefillTextFieldsAsync` that allows you to prefill document fields
- `IDocumentService.EditDocumentAsync` that allows you to add/change various fields
- `IFieldEditable` that represents editable Field attributes
- `Model.EditFields.TextField` that allows you to configure text field and add/change the document fields

### Changed
- `FieldJsonAttributes` added to `ISignNowFields` that allows you to get Field attributes

## [0.9.0] - 2021-07-07
### Added
- `IUserService.GetModifiedDocumentsAsync` that allows to get all modified documents for User
- `IUserService.GetUserDocumentsAsync` that allows to get all information of user's documents that were not modified yet
- `Interfaces.ISignNowContext` extended with `IFolderService` that allows you works with Folders
- `IFolderService.GetAllFoldersAsync` that allows you to get all user Folders with documents
- `IFolderService.GetFolderAsync` that allows you to get all details of a specific folder including all documents in that folder.
- `IFolderService.CreateFolderAsync` that allows you to create folder for a user.
- `IFolderService.DeleteFolderAsync` that allows you to deletes a folder.
- `IFolderService.RenameFolderAsync` that allows you to renames a folder.
- `IDocumentService.MoveDocumentAsync` that allows you to move the document to a specified folder.

## [0.8.0] - 2021-04-26
### Added
- `IDocumentService.CreateTemplateFromDocumentAsync` that allows to create template by flattening an existing document
- `IDocumentService.CreateDocumentFromTemplateAsync` that allows to create document from the template

## [0.7.0] - 2021-03-28
### Added
- `ISignInvite.CreateInviteAsync` that allows to create embedded signing invite for a document
- `ISignInvite.CancelEmbeddedInviteAsync` that allows to delete embedded signing invite for a document
- `ISignInvite.GenerateEmbeddedInviteLinkAsync` that allows to create a link for the embedded signing invite for a document

### Changed
- Changed JsonConverter for `Model.SignNowInvite` properties

## [0.6.0-beta] - 2020-11-17
### Added
- Carbon Copy for freeform invite and role-based invite [#106](https://github.com/signnow/SignNow.NET/issues/106)
- `Service.DocumentService.MergeDocumentsAsync` that allows to merge two or more documents into one document
- `Service.DocumentService.GetDocumentHistoryAsync` that allows to retrieve the history of the document actions
- `Service.DocumentService.CreateOneTimeDownloadLinkAsync` that allows to share document via one-time URL
- `Service.UserService.CreateUserAsync` that allows to create an account for user
- `Service.UserService.UpdateUserAsync` that allows to update user information i.e. first name, last name
- `Service.UserService.SendVerificationEmailAsync` that allows to sends verification email to a user
- `Service.UserService.SendPasswordResetLinkAsync` that allows to sends password reset link to a user

### Changed
- Increased timeout for Http Client
- Added Http method, Url and response time to error messages
- Added support for Basic Auth param to `Model.Token`


## [0.5.2-beta] - 2020-05-22
### Fixed
- Fixed `Models.FieldContents.RadiobuttonContent` converting error [#104](https://github.com/signnow/SignNow.NET/issues/104)

## [0.5.1-beta] - 2020-04-18
### Changed
- Upgraded netcore version from 2.x to 3.x for `SignNow.Net.Test`
- Added project `SignNow.Net.Examples` with code samples
- Added validation for all document identity

### Fixed
- Fixed `Field.Type` converting error [#96](https://github.com/signnow/SignNow.NET/issues/96)


## [0.5.0-beta] - 2020-03-06
### Added
- All SignNow field types: Signature, Initial, Text, Dropdown, Checkbox, Radiobutton, Attachment, Hyperlink
- `Internal.Model.FieldJsonAttributes` that contains fields properties such as name, label, X/Y coordinates, width, height, etc.
- The `ISignNowContent` interface that allows to retrieve value of any SignNow (Field) content object.
- The `ISignNowField` interface that allows to retrieve the field value using the `Type` and `ElementId` parameters of the field element 
- `Model.SignNowDocument.GetFieldContent(FieldType)` that allows to retrieve the field content of any SignNow field.

### Fixed
- Changed `expires_in` token value from a timestamp to token lifetime 

### Changed
- `Model.Field` now contains property `Model.FieldJsonAttributes`
- `Model.SignNowDocument` extended by added collections of various SignNow fields (Texts, Checkboxes, Hyperlinks... etc.)
- `Model.Signature` renamed to `Model.FieldContents.SignatureContent`


## [0.4.0-beta] - 2020-02-19
### Added
- Code samples for [role-based invite][create role-based invite] and [freeform invite][create freeform invite] to README
- Signature status property for the document - `Models.SignNowDocument.Status`; can be `NoInvite`, `Pending`, `Complete`
- Signature status for the specified signer of a Freeform invite - `Models.FreeformInvite.Status`
- Signature status for the specified signer of a field invite - `Models.FieldInvite.Status`
- `ISignNowInviteStatus` interface for role-based or freeform invites status retrieval
- `Model.DocumentStatus` with all statuses for the document
- `Model.InviteStatus` with all statuses for the Freeform and Field (role-based) invites

### Changed
- Removed redundant tests.
- Changed duplicated tests to parametrized tests.
- Replaced Json mocks with Bogus library for tests.


## [0.3.0-beta] - 2020-02-03
### Added
- IUserService.GetCurrentUserAsync method which allows developers to retrieve the User resource.
- IUserService.CreateInviteAsync method for creating freeform or role-based invites.
- IUserService.CancelInviteAsync method for canceling a freeform invite using the invite ID or canceling a role-based invite using the document ID.
- Model.FreeFormInvite which allows developers to create freeform invites to sign the document.
- Model.RoleBasedInvite which allows developers to create role-based invites to sign the document (with options on how to verify the signer's identity: by password protection, a phone call, or sms)

### Changed
- IDocumentService returns SignNowDocument model instead of DocumentResponse
- IDocumentService.GetDocumentAsync method can now be used to retrieve document roles, invite requests, timestamps, pages count, etc..


## [0.2.0-beta] - 2019-12-18
### Added
- DocumentService.DownloadDocumentAsync method which allows to download a document by ID in either Collapsed/With History/Zipped mode

## Fixed
- "Argument passed in is not serializable" error on any SignNowException in .NET 4.5


## [0.1.0-beta] - 2019-11-18
### Added
- OAuth2Service.RefreshTokenAsync method.
- OAuth2Service.ValidateTokenAsync method.


## [0.0.0-beta] - 2019-10-30
### Added
- Authorization token retrieval
- Document upload
- Document delete
- Create signing link (to the document that requires an e-signature)
- SignNowException implements AggregateException. If SignNow returns several errors in response, SignNowException contains all the errors as child exceptions.
- SignNowException.Data contains the integer value of a response status code instead of a string one.
- Implemented document ID parameter value validation in DocumentService.DeleteDocumentAsync method.


<!-- Aliases for URLs: please place here any long urls to keep clean markdown markup -->
[create role-based invite]: https://github.com/signnow/SignNow.NET/blob/develop/README.md#create-role-based-invite
[create freeform invite]: https://github.com/signnow/SignNow.NET/blob/develop/README.md#create-freeform-invite


[Unreleased]: https://github.com/signnow/SignNow.NET/compare/0.9.0...HEAD
[0.9.0]: https://github.com/signnow/SignNow.NET/compare/0.8.0...0.9.0
[0.8.0]: https://github.com/signnow/SignNow.NET/compare/0.7.0...0.8.0
[0.7.0]: https://github.com/signnow/SignNow.NET/compare/0.6.0-beta...0.7.0
[0.6.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.5.2-beta...0.6.0-beta
[0.5.2-beta]: https://github.com/signnow/SignNow.NET/compare/0.5.1-beta...0.5.2-beta
[0.5.1-beta]: https://github.com/signnow/SignNow.NET/compare/0.5.0-beta...0.5.1-beta
[0.5.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.4.0-beta...0.5.0-beta
[0.4.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.3.0-beta...0.4.0-beta
[0.3.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.2.0-beta...0.3.0-beta
[0.2.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.1.0-beta...0.2.0-beta
[0.1.0-beta]: https://github.com/signnow/SignNow.NET/compare/0.0.0-beta...0.1.0-beta
