using Newtonsoft.Json;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Model;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents optional properties for Signer.
    /// </summary>
    public class SignerOptions
    {
        /// <summary>
        /// <see cref="SignerAuthorization"/> options for Role-based Invite.
        /// </summary>
        [JsonIgnore]
        public SignerAuthorization SignerAuth { get; set; }

        [JsonIgnore]
        public Role SignerRole { get; }

        /// <summary>
        /// Represents the email address of the signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Represents the name of the role the signer will be required to fulfill.
        /// </summary>
        [JsonProperty("role")]
        private string RoleName => SignerRole.Name;

        /// <summary>
        /// Leave empty state “” for a document with one role,
        /// or get all role IDs from Document and assign a signer to the specific `role_id`
        /// </summary>
        [JsonProperty("role_id")]
        private string RoleId => SignerRole.Id;

        /// <summary>
        /// The order of signing, or Signing step. When there are multiple signers,
        /// preset who signs the document first, who’s in the second group etc.
        /// Each group - one signing step. For a freeform invite this equals “1”
        /// </summary>
        [JsonProperty("order")]
        public int SigningOrder => SignerRole.SigningOrder;

        /// <summary>
        /// Authentication type for case, when password used to open the Document.
        /// </summary>
        [JsonProperty("authentication_type", NullValueHandling = NullValueHandling.Ignore)]
        private string AuthenticationType => SignerAuth?.AuthenticationType;

        /// <summary>
        /// Password will be required from signers when they open the document.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        private string Password => SignerAuth?.Password;

        /// <summary>
        /// Phone number to authorize signers when they open the document via phone call or sms code.
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        private string Phone => SignerAuth?.Phone;

        /// <summary>
        /// In how many days this invite expires.
        /// </summary>
        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// In how many days will another email be sent to remind of a signature invite.
        /// </summary>
        [JsonProperty("reminder", NullValueHandling = NullValueHandling.Ignore)]
        public int? RemindAfterDays { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignerOptions"/> class.
        /// </summary>
        /// <param name="email"><see cref="Email"/> address of the signer.</param>
        /// <param name="role"><see cref="Role"/> of the signer which will be required to fulfill.</param>
        /// <exception cref="System.ArgumentNullException">if <see cref="Role"/> is null.</exception>
        /// <exception cref="System.ArgumentException">if email is not valid.</exception>
        public SignerOptions(string email, Role role)
        {
            Guard.ArgumentNotNull(role, nameof(role));
            Email = email.ValidateEmail();
            SignerRole = role;
        }

        /// <summary>
        /// Set <see cref="Password"/> which will be required from signers when they open the document.
        /// </summary>
        /// <param name="password">Password for signer to open the document.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetAuthenticationByPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return this;

            SignerAuth = new PasswordAuthorization(password);

            return this;
        }

        /// <summary>
        /// Set <see cref="Phone"/> number to authorize signer when they open the document via phone call.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetAuthenticationByPhoneCall(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return this;

            SignerAuth = new PhoneCallAuthorization(phone);

            return this;
        }

        /// <summary>
        /// Set <see cref="Phone"/> number to authorize signer when they open the document via sms code.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetAuthenticationBySms(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return this;

            SignerAuth = new SmsAuthorization(phone);

            return this;
        }

        /// <summary>
        /// Clear Signer authentication options.
        /// </summary>
        /// <returns></returns>
        public SignerOptions ClearSignerAuthentication()
        {
            SignerAuth = null;

            return this;
        }
    }
}
