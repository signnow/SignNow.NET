using SignNow.Net.Model;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Send a freeform or role-based invite
    /// </summary>
    public interface ISignInvite
    {
        /// <summary>
        /// The email of the sender`s.
        /// </summary>
        string Sender { get; }

        /// <summary>
        /// An email of signer`s that you would like to send the invite to
        /// </summary>
        string Recipient { get; }

        /// <summary>
        /// Returns JSON content for Invite
        /// </summary>
        /// <returns></returns>
        IContent InviteContent();
    }
}