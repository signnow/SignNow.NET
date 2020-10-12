using System;
using System.Globalization;
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
    }

    /// <summary>
    /// User billing period data
    /// </summary>
    public class UserBilling
    {
        [JsonProperty("start_date")]
        internal string Start => StartDate.Date.ToString("d", DateTimeFormatInfo.InvariantInfo);

        [JsonProperty("end_date")]
        internal string End => EndDate.Date.ToString("d", DateTimeFormatInfo.InvariantInfo);

        /// <summary>
        /// User billing period start date.
        /// </summary>
        [JsonProperty("start_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// User billing period end date.
        /// </summary>
        [JsonProperty("end_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime EndDate { get; set; }
    }
}
