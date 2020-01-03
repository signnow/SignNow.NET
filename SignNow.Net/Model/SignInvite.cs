using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public abstract class SignInvite
    {
        /// <summary>
        /// The subject of the email.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The message body of the email invite.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        internal SignInvite(string message, string subject)
        {
            Message = message;
            Subject = subject;
        }
    }

    /// <summary>
    /// Freeform invite - an invitation to sign a document which doesn't contain any fillable fields.
    /// </summary>
    public sealed class FreeFormInvite : SignInvite
    {
        /// <summary>
        /// An email of signer`s that you would like to send the invite to
        /// </summary>
        [JsonProperty("to")]
        public string Recipient { get; private set; }

        /// <param name="to">The email of the invitee</param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        public FreeFormInvite(string to, string message = null, string subject = null) : base(message, subject)
        {
            Recipient = to;
        }
    }
}
