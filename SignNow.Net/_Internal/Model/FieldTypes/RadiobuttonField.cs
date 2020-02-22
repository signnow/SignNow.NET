using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Radiobutton`
    /// </summary>
    internal class RadiobuttonField : BaseField
    {
        /// <summary>
        /// Radiobutton name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Timestamp radiobutton was created.
        /// </summary>
        [JsonProperty("server_created_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// X coordinate (in pixels) of radiobutton field.
        /// <para>Coordinate starts from top left corner of the document Mediabox</para>
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Y coordinate (in pixels) of radiobutton field.
        /// <para>Coordinate starts from top left corner of the document Mediabox</para>
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        /// <summary>
        /// List of Radio elements.
        /// </summary>
        [JsonProperty("radio")]
        public List<RadioField> Radio { get; set; } = new List<RadioField>();

        /// <summary>
        /// Returns Radiobutton content (actual state) as string.
        /// </summary>
        [JsonIgnore]
        public string Data => Radio.Find(itm => itm.Checked == true).Data;

        /// <summary>
        /// Returns Radiobutton content (actual state) as string.
        /// </summary>
        public override string ToString() => Data;
    }

    /// <summary>
    /// Represents SignNow element for field types: `Radiobutton`
    /// </summary>
    internal class RadioField
    {
        /// <summary>
        /// Identity of Radio field.
        /// </summary>
        [JsonProperty("radio_id")]
        public string Id { get; set; }

        /// <summary>
        /// The page number of the document.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// X coordinate (in pixels) of radio field.
        /// <para>Coordinate starts from top left corner of the document Mediabox</para>
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Y coordinate (in pixels) of radio field.
        /// <para>Coordinate starts from top left corner of the document Mediabox</para>
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        /// <summary>
        /// Width (in pixels) of radio field.
        /// <para>Starts from top left corner of the element frame.</para>
        /// </summary>
        [JsonProperty("width")]
        public decimal Width { get; set; }

        /// <summary>
        /// Height (in pixels) of radio field.
        /// <para>Starts from top left corner of the element frame.</para>
        /// </summary>
        [JsonProperty("height")]
        public decimal Height { get; set; }

        /// <summary>
        /// Timestamp radio element was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// State of radio element (checked or not).
        /// </summary>
        [JsonProperty("checked")]
        [JsonConverter(typeof(StringToBoolJsonConverter))]
        public bool Checked { get; set; }

        /// <summary>
        /// The radio element value.
        /// </summary>
        [JsonProperty("value")]
        public string Data { get; set; }
    }
}
