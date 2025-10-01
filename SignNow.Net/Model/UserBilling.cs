using System;
using System.Globalization;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
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
