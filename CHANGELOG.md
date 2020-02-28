# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com)
and this project adheres to [Semantic Versioning](http://semver.org).

## [Unreleased]
### Added
- Introduced all SignNow field types: Signature, Initial, Text, Dropdown, Checkbox, Radiobutton, Attachment, Hyperlink
- Added `Model.FieldJsonAttributes` which contains fields properties (e.g. name, label, X/Y coordinates, width, height)
- Introduced Interface `ISignNowFieldContent` which allows to retrieve field value for any of SignNow FieldContent object.
- Introduced Interface `ISignNowField` which allows to get the Type of the field from any of documents fields.
- `Model.SignNowDocument.GetFieldContent(FieldType)` which allows to retrieve field content for any of SignNow fields.
### Fixed
- Changed `expires_in` token value to token lifetime instead of timestamp

### Changed
- `Model.Field` now contains property `Model.FieldJsonAttributes`
- `Model.SignNowDocument` extended by added collections of various SignNow fields (Texts, Checkboxes, Hyperlinks... etc.)
- `Model.Signature` renamed to `Model.FieldContents.SigntureContent`


## [0.4.0-beta] - 2020-02-19
### Added
- Сode samples for [role-based invite][create role-based invite] and [freeform invite][create freeform invite] to README
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


## [0.3.0-beta] 2020-02-03
### Added
- IUserService.GetCurrentUserAsync method which allows developers to retrieve the User resource.
- IUserService.CreateInviteAsync method for creating freeform or role-based invites.
- IUserService.CancelInviteAsync method for canceling a freeform invite using the invite ID or canceling a role-based invite using the document ID.
- Model.FreeFormInvite which allows developers to create freeform invites to sign the document.
- Model.RoleBasedInvite which allows developers to create role-based invites to sign the document (with options on how to verify the signer's identity: by password protection, a phone call, or sms)

### Changed
- IDocumentService returns SignNowDocument model instead of DocumentResponse
- IDocumentService.GetDocumentAsync method can now be used to retrieve document roles, invite requests, timestamps, pages count, etc..


## [0.2.0-beta] 2019-12-18
### Added
- DocumentService.DownloadDocumentAsync method which allows to download a document by ID in either Collapsed/With History/Zipped mode

## Fixed
- "Argument passed in is not serializable" error on any SignNowException in .NET 4.5


## [0.1.0-beta] 2019-11-18
### Added
- OAuth2Service.RefreshTokenAsync method.
- OAuth2Service.ValidateTokenAsync method.


## [0.0.0-beta] - 2019-10-30
### Added
- Authorizaton token retrieval
- Document upload
- Document delete
- Create signing link (to the document that requires an e-signature)
- SignNowException implements AggregateException. If SignNow returns several errors in response, SignNowException contains all the errors as child exceptions.
- SignNowException.Data contains the integer value of a response status code instead of a string one.
- Implemented document ID parameter value validation in DocumentService.DeleteDocumentAsync method.


<!-- Aliases for URLs: please place here any long urls to keep clean markdown markup -->
[create role-based invite]: https://github.com/signnow/SignNow.NET/blob/develop/README.md#create-role-based-invite
[create freeform invite]: https://github.com/signnow/SignNow.NET/blob/develop/README.md#create-freeform-invite