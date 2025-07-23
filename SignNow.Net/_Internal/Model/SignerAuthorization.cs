namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Represents Signer Authorization options for Role-based Invite.
    /// </summary>
    public abstract class SignerAuthorization
    {
        /// <summary>
        /// Authentication type for case, when password, phone call or sms code used to open the Document.
        /// </summary>
        public abstract string AuthenticationType { get; }

        /// <summary>
        /// Password will be required from signers when they open the document.
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// Phone number to authorize signers when they open the document via phone call or sms code.
        /// </summary>
        public string Phone { get; protected set; }

        internal SignerAuthorization() {}
    }

    /// <summary>
    /// Represents password authorization for signers when they open the document.
    /// </summary>
    internal sealed class PasswordAuthorization : SignerAuthorization
    {
        /// <inheritdoc cref="SignerAuthorization"/>
        public override string AuthenticationType => "password";

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordAuthorization"/> class.
        /// </summary>
        /// <param name="password">Password for signer to open the document.</param>
        public PasswordAuthorization(string password)
        {
            Password = password;
        }
    }

    /// <summary>
    /// Represents phone call authorization for signers when they open the document.
    /// </summary>
    internal sealed class PhoneCallAuthorization : SignerAuthorization
    {
        /// <inheritdoc cref="SignerAuthorization"/>
        public override string AuthenticationType => "phone_call";

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneCallAuthorization"/> class.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        public PhoneCallAuthorization(string phone)
        {
            Phone = phone;
        }
    }

    /// <summary>
    /// Represents sms code authorization for signers when they open the document.
    /// </summary>
    internal sealed class SmsAuthorization : SignerAuthorization
    {
        /// <inheritdoc cref="SignerAuthorization"/>
        public override string AuthenticationType => "sms";

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsAuthorization"/> class.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        public SmsAuthorization(string phone)
        {
            Phone = phone;
        }
    }
}
