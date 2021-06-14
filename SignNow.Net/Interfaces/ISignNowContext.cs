namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface that manages other SignNow interfaces: IDocumentService, IUserService, ISignInvite, IFolderService.
    /// <para>It contains:</para>
    /// <list type="bullet">
    /// <item>
    ///     <description><see cref="IDocumentService"/> interface for any operations with a Document in SignNow;
    ///         can be used to create, download, retrieve, delete a document etc.</description>
    /// </item>
    /// <item>
    ///     <description><see cref="IUserService"/> interface for any operations with a User in SignNow;
    ///         i.e. create a user, authenticate a user, retrieve user's documents etc.</description>
    /// </item>
    /// <item>
    ///     <description><see cref="ISignInvite"/> interface for any operations with an Invite in SignNow:
    ///         creating or canceling the invite to sign a document, checking status of the invite, etc.</description>
    /// </item>
    /// <item>
    ///     <description><see cref="IFolderService"/> interface for any operations with a Folders in SignNow:
    ///         can be used to create, view, rename or delete a folders.</description>
    /// </item>
    /// </list>
    /// </summary>
    public interface ISignNowContext
    {
        /// <inheritdoc cref="IDocumentService"/>
        IDocumentService Documents { get; }

        /// <inheritdoc cref="IUserService"/>
        IUserService Users { get; }

        /// <inheritdoc cref="ISignInvite"/>
        ISignInvite Invites { get; }

        /// <inheritdoc cref="IFolderService"/>
        IFolderService Folders { get; }
    }
}
