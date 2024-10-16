using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent Embedded signing invite params.
    /// </summary>
    public class EmbeddedInvite
    {
        private string email { get; set; }
        private uint signingOrder { get; set; }

        private string prefillSignatureName { get; set; }
        private bool forceNewSignature { get; set; }

        private string requiredPresetSignatureName { get; set; }

        /// <summary>
        /// Prefilled text in the Signature field, disabled for editing by signer.
        /// Cannot be used together with prefill_signature_name and/or force_new_signature.
        /// </summary>
        private bool isPrefilledSignatureName { get; set; }

        private bool isRequiredPresetEnabled { get; set; }


        /// <summary>
        /// Signer's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email
        {
            get { return email; }
            set { email = value.ValidateEmail(); }
        }

        /// <summary>
        /// Signer's role id in the document.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// The order of signing. Cannot be 0.
        /// </summary>
        [JsonProperty("order")]
        public uint SigningOrder
        {
            get { return signingOrder; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Value cannot be 0", nameof(SigningOrder));
                }

                signingOrder = value;
            }
        }

        /// <summary>
        /// Sets the language of the signing session for the signer.
        /// </summary>
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Lang Language { get; set; }

        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;

        /// <summary>
        /// Signer's first name.
        /// </summary>
        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Firstname { get; set; }

        /// <summary>
        /// Signer's last name.
        /// </summary>
        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Lastname { get; set; }

        /// <summary>
        /// Prefilled text in the Signature field.
        /// </summary>
        /// <exception cref="ArgumentException">String lenght cannot be greater than 255 characters</exception>
        /// <exception cref="ArgumentException">Cannot be used together with Required preset for Signature name</exception>
        [JsonProperty("prefill_signature_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefillSignatureName
        {
            get { return prefillSignatureName; }
            set
            {
                if (value.Length > 255)
                {
                    throw new ArgumentException(
                        "Prefilled text in the Signature field can be maximum 255 characters.",
                        nameof(PrefillSignatureName));
                }

                if (isRequiredPresetEnabled)
                {
                    throw new ArgumentException(
                        "Required preset for Signature name is set. Cannot be used together with",
                        nameof(PrefillSignatureName));
                }

                prefillSignatureName = value;
                isPrefilledSignatureName = true;
            }
        }

        /// <exception cref="ArgumentException">Cannot be used together with Required preset for Signature name</exception>
        [JsonProperty("force_new_signature", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolToIntJsonConverter))]
        public bool ForceNewSignature
        {
            get { return forceNewSignature; }
            set
            {
                if (isRequiredPresetEnabled)
                {
                    throw new ArgumentException(
                        "Required preset for Signature name is set. Cannot be used together with",
                        nameof(ForceNewSignature));
                }

                forceNewSignature = value;

                if (value)
                {
                    isPrefilledSignatureName = true;
                }
            }
        }

        /// <summary>
        /// Prefilled text in the Signature field, disabled for editing by signer.
        /// </summary>
        /// <exception cref="ArgumentException">Cannot be used together with prefill for Signature name</exception>
        [JsonProperty("required_preset_signature_name", NullValueHandling = NullValueHandling.Ignore)]
        public string RequiredPresetSignatureName
        {
            get { return requiredPresetSignatureName;  }
            set
            {
                if (isPrefilledSignatureName)
                {
                    throw new ArgumentException(
                        "Prefill for Signature name or Force new signature is set. Cannot be used together with",
                        nameof(RequiredPresetSignatureName));
                }

                requiredPresetSignatureName = value;
                isRequiredPresetEnabled = true;
            }
        }

        /// <summary>
        /// The link that opens after the signing session has been completed.
        /// </summary>
        [JsonProperty("redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri RedirectUrl { get; set; }

        /// <summary>
        /// The link that opens after the signing session has been declined by the signer.
        /// </summary>
        [JsonProperty("decline_redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri DeclineRedirectUrl { get; set; }

        /// <summary>
        /// Determines whether to open the redirect link in the new tab in the browser, or in the same tab after the signing session.
        /// Possible values: blank - opens the link in the new tab, self - opens the link in the same tab, default value.
        /// </summary>
        [JsonProperty("redirect_target", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public RedirectTarget RedirectTarget { get; set; }
    }
}
