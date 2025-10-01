using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.ComplexTags;

namespace SignNow.Net.Model
{
    public class ComplexTextTags : ITextTags
    {
        [JsonIgnore]
        public List<ComplexTag> Properties { get; set; } = new List<ComplexTag>();

        public ComplexTextTags() { }

        public ComplexTextTags(ComplexTag tag)
        {
            Properties.Add(tag);
        }

        public string ToStringContent()
        {
            return JsonConvert.SerializeObject(Properties);
        }
    }
}
