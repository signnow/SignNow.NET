using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents optional properties for Signer.
    /// </summary>
    public class SignerOptions
    {
        private string AuthType { get; set; }

        private Role SignerRole { get; set; }

        /// <summary>
        /// Represents the email address of the signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Represents the name of the role the signer will be required to fulfill.
        /// </summary>
        [JsonProperty("role")]
        public string RoleName => SignerRole.Name;

        /// <summary>
        /// Leave empty state “” for a document with one role,
        /// or get all role IDs from Document and assign a signer to the specific `role_id`
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId => SignerRole.Id;

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
        public string AuthenticationType => AuthType;

        /// <summary>
        /// Password will be required from signers when they open the document.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; private set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; private set; }

        /// <summary>
        /// In how many days this invite expires.
        /// </summary>
        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// In how many days will another email be sent to remind of a signature invite.
        /// </summary>
        [JsonProperty("reminder",NullValueHandling = NullValueHandling.Ignore)]
        public int? RemindAfterDays { get; set; }

        public SignerOptions(string email, Role role)
        {
            Guard.ArgumentNotNull(role, nameof(role));
            Email = email;
            SignerRole = role;
        }

        /// <summary>
        /// Set <see cref="Password"/> which will be required from signers when they open the document.
        /// </summary>
        /// <param name="password">Password for signer to open the document.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetProtectionByPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                Password = password;
                Phone = null;
                AuthType = "password";
            }

            return this;
        }

        /// <summary>
        /// Set <see cref="Phone"/> number to authorize signer when they open the document via phone call.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetProtectionByPhoneCall(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                Password = null;
                Phone = phone;
                AuthType = "phone_call";
            }

            return this;
        }

        /// <summary>
        /// Set <see cref="Phone"/> number to authorize signer when they open the document via sms code.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        /// <returns><see cref="SignerOptions"/></returns>
        public SignerOptions SetProtectionBySms(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                Password = null;
                Phone = phone;
                AuthType = "sms";
            }

            return this;
        }

        /// <summary>
        /// Clear Signer access protection options.
        /// </summary>
        /// <returns></returns>
        public SignerOptions ClearSignerAccessProtection()
        {
            AuthType = null;
            Password = null;
            Phone = null;

            return this;
        }
    }
}
