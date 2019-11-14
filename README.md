# SignNow.NET

## About SignNow

SignNow.Net is the official .NET 4.5+ class library for the SignNow API. SignNow allows you to embed legally-binding e-signatures into your app, CRM or cloud storage. Send documents for signature directly from your website. Invite multiple signers to finalize contracts. Track status of your requests and download signed copies automatically.

Get your account at https://www.signnow.com/developers


## IMPORTANT NOTES
**Note for Pull Requests (PRs)**: We accept pull requests from the community. Please, send your pull requests to the **DEVELOP branch** which is the consolidated work-in-progress branch. We don't accept requests to the Master branch.


## Get started
  * Get a SignNow account [here](https://www.signnow.com/developers) or [here](https://www.signnow.com/)
  * SignNow uses Authorization: Basic <credentials>, so before you start, have your client ID and secret base-64 encoded
  * For XML documentation generation you have to install InheritDocTool (the project build will fail without it):

    ```
    dotnet tool install -g InheritDocTool
    ```

More about the InheritDoc [here](https://www.inheritdoc.io)

#### Windows
  * .Net Framework 4.5 or newer version should be installed in your system, or
  * .Net Standard 1.2 or 2.0, or
  * .Net Core 2.2 and newer

#### MacOS and Linux
  * .Net Standard 1.2 or 2.0, or
  * .Net Core 2.2 and newer

#### /root folder

  * To your `/root` folder, add a config file for every environment you'd like to work with. 
  Name it `api.signnow.<environment>.json` where `<environment>` equals `test`, or `prod`, or any environment name you use. 
  
    For example: `api.signnow.test.json`

  * Add your SignNow login and password to that file in json format
    ```
    {
        'login':'name123'
        'password':'pass123'
    }
    ```






