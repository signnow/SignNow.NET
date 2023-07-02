using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.ComplexTags
{
    public class RadioButtonTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override FieldType Type { get; protected set; } = FieldType.RadioButton;

        /// <summary>
        /// Radiobutton name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Position of the field by X axis
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Position of the field by Y axis
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("radio")]
        public List<RadioGroup> Options { get; set; } = new List<RadioGroup>();

        public RadioButtonTag(string groupName)
        {
            Name = groupName;
        }

        public void AddOption(string value, int xPos, int yPos, int isChecked = 0)
        {
            var option = new RadioGroup
            {
                PageNumber = this.PageNumber,
                X = this.X,
                Y = this.Y,
                Width = Math.Min(this.Width, this.Height),
                Height = Math.Min(this.Width, this.Height),
                Value = value,
                isChecked = isChecked,
                XOffset = xPos,
                YOffset = yPos

            };

            Options.Add(option);
        }
    }

    public class RadioGroup
    {
        [JsonProperty("page_number")]
        public int PageNumber { get; internal set; }

        [JsonProperty("x")]
        public int X { get; internal set; }

        [JsonProperty("y")]
        public int Y { get; internal set; }

        [JsonProperty("width")]
        public int Width { get; internal set; }

        [JsonProperty("height")]
        public int Height { get; internal set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("checked")]
        public int isChecked { get; set; }

        [JsonProperty("x-offset")]
        public int XOffset { get; set; }

        [JsonProperty("y-offset")]
        public int YOffset { get; set; }
    }
}
