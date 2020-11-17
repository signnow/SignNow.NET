using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent a user resource
    /// </summary>
    public class User
    {
        /// <summary>
        /// User unique id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// User is active or not.
        /// </summary>
        [JsonProperty("active")]
        [JsonConverter(typeof(StringToBoolJsonConverter))]
        public bool Active { get; set; }

        /// <summary>
        /// User is verified or not.
        /// </summary>
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        /// <summary>
        /// User is logged or not.
        /// </summary>
        [JsonProperty("is_logged_in")]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// User first name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        [JsonProperty("primary_email")]
        public string Email { get; set; }

        /// <summary>
        /// Timestamp User was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// User billing period data
        /// </summary>
        [JsonProperty("billing_period")]
        public UserBilling BillingPeriod { get; internal set; }

        /// <summary>
        /// Companies which user belongs to.
        /// </summary>
        [JsonProperty("companies")]
        public IReadOnlyCollection<Company> Companies { get; set; } = new List<Company>();

        /// <summary>
        /// Documents used by User per month.
        /// </summary>
        [JsonProperty("monthly_document_count")]
        public int MonthlyDocumentCount { get; internal set; }

        /// <summary>
        /// The number of documents used by User for the entire time.
        /// </summary>
        [JsonProperty("lifetime_document_count")]
        public int LifetimeDocumentCount { get; internal set; }
    }
}
