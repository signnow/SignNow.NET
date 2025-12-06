using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net._Internal.Helpers.Converters;

namespace SignNow.Net.Test.UnitTests.Helpers.Converters
{
    [TestClass]
    public class ObjectOrEmptyArrayConverterTests
    {
        // better split to 2 test classes
        [TestMethod]
        [DataRow("{'pr_1': 'propvalue1', 'pr_2': 'propvalue2'}", typeof(ModelA.ModelB))]
        [DataRow("{}", typeof(ModelA.ModelB))]
        [DataRow("[]", null)]
        [DataRow("null", null)]
        public void MyTestMethod(string modelB, Type expected)
        {
            var json = $@"
            {{
                'version': 2,
                'model_b': {modelB}
            }}";

            var obj = JsonConvert.DeserializeObject<ModelA>(json);

            Assert.AreEqual(expected, obj.Mb?.GetType());
        }
    }

    public class ModelA
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("model_b")]
        [JsonConverter(typeof(ObjectOrEmptyArrayConverter))]
        public ModelB Mb { get; set; }

        public class ModelB
        {
            [JsonProperty("pr_1")]
            public string Prop1 { get; set; }

            [JsonProperty("pr_2")]
            public string Prop2 { get; set; }
        }
    }
}
