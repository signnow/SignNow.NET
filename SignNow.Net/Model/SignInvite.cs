using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public abstract class SignInvite
    {
        /// <summary>
        /// The subject of the email.
        /// <remarks>
        ///     If <see cref="Subject"/> is null - default subject will be used:
        ///     `sender.email@signnow.com` Needs Your Signature
        /// </remarks>
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The message body of the email invite.
        /// <remarks>
        ///     If <see cref="Message"/> is null - default message will be used:
        ///     `sender.email@signnow.com` invited you to sign `DocumentName`
        /// </remarks>
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        internal SignInvite()
        {
        }
    }

    /// <summary>
    /// Freeform invite - an invitation to sign a document which doesn't contain any fillable fields.
    /// </summary>
    public sealed class FreeFormInvite : SignInvite
    {
        /// <summary>
        /// An email of signer`s that you would like to send the invite to.
        /// </summary>
        [JsonProperty("to")]
        public string Recipient { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="FreeFormInvite"/>
        /// </summary>
        /// <param name="to">The email of the invitee.</param>
        public FreeFormInvite(string to)
        {
            Recipient = to;
        }
    }
}
