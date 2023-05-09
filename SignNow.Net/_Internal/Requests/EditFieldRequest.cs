using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class EditFieldRequest : JsonHttpContent
    {
        [JsonProperty("client_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ClientTime { get; set; } = DateTime.Now;

        [JsonProperty("fields")]
        public List<IFieldEditable> Fields { get; set; } = new List<IFieldEditable>();

        public EditFieldRequest(IFieldEditable field)
        {
            Fields.Add(field);
        }

        public EditFieldRequest(IEnumerable<IFieldEditable> fields)
        {
            Fields = (List<IFieldEditable>)fields;
        }
    }
}
