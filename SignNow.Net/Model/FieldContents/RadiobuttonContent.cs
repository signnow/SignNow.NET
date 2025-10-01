using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Represents signNow field types: `Radiobutton`
    /// </summary>
    public class RadiobuttonContent : BaseContent
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
        public List<RadioContent> Radio { get; internal set; } = new List<RadioContent>();

        /// <summary>
        /// Returns Radiobutton content (actual state) as string.
        /// </summary>
        [JsonIgnore]
        public string Data => Radio.FirstOrDefault(itm => itm.Checked == true)?.Data;

        /// <summary>
        /// Returns Radiobutton content (actual state) as string.
        /// </summary>
        public override string ToString() => Data;

        /// <inheritdoc />
        public override object GetValue() => Data;
    }

    /// <summary>
    /// Represents signNow element for field types: `Radiobutton`
    /// </summary>
    public class RadioContent : ISignNowContent
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

        /// <inheritdoc />
        public object GetValue() => Data;
    }
}
