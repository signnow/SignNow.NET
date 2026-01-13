using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.QueryBuilders;

namespace UnitTests.Models
{
    [TestClass]
    public class FilterBuilderBaseTest
    {
        /// <summary>
        /// Implementation of FilterBuilderBase for testing protected methods.
        /// </summary>
        private class FilterBuilderTestClass : FilterBuilderBase
        {
            public string And(params Func<FilterBuilderTestClass, string>[] filterBuilder)
                => FilterBuilderBase.And(filterBuilder);

            public string Or(params Func<FilterBuilderTestClass, string>[] filterBuilder)
                => FilterBuilderBase.Or(filterBuilder);

            public new string Filter(string param, string operation, string value)
                => FilterBuilderBase.Filter(param, operation, value);

            public new string Filter(string param, string operation, string[] values, bool quoteValues = false)
                => FilterBuilderBase.Filter(param, operation, values, quoteValues);

            public new string[] EnumToStringValues<T>(T[] enums) where T : Enum
                => FilterBuilderBase.EnumToStringValues(enums);
        }

        [TestMethod]
        public void And_WithSingleFilter_ReturnsFilterDirectly()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"filter_type\", \"value\": \"value\"}}",
                new FilterBuilderTestClass().And(fb => fb.Filter("param", "filter_type", "value"))
            );
        }

        [TestMethod]
        public void And_WithMultipleFilters_ReturnsCombinedAndCondition()
        {
            Assert.AreEqual(
                "{\"_AND\": [{\"param1\":{\"type\": \"eq\", \"value\": \"v1\"}},{\"param2\":{\"type\": \"in\", \"value\": \"v2\"}}]}",
                new FilterBuilderTestClass().And(
                    fb => fb.Filter("param1", "eq", "v1"),
                    fb => fb.Filter("param2", "in", "v2")
                )
            );
        }

        [TestMethod]
        public void And_WithNullFilters_IgnoreNulls()
        {
            Assert.AreEqual(
                "{\"_AND\": [{\"param1\":{\"type\": \"eq\", \"value\": \"v1\"}},{\"param2\":{\"type\": \"in\", \"value\": \"v2\"}}]}",
                new FilterBuilderTestClass().And(
                    fb => fb.Filter("param1", "eq", "v1"),
                    null,
                    fb => fb.Filter("param2", "in", "v2")
                )
            );
        }

        [TestMethod]
        public void And_WithNullArray_ThrowsArgumentException()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new FilterBuilderTestClass().And(null)
            );

            StringAssert.Contains(exception.Message, "At least one filter must be provided for AND operation");
        }

        [TestMethod]
        public void And_WithEmptyArray_ThrowsArgumentException()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new FilterBuilderTestClass().And()
            );

            StringAssert.Contains(exception.Message, "At least one filter must be provided for AND operation");
        }

        [TestMethod]
        public void Or_WithSingleFilter_ReturnsFilterDirectly()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"filter_type\", \"value\": \"value\"}}",
                new FilterBuilderTestClass().Or(fb => fb.Filter("param", "filter_type", "value"))
            );
        }

        [TestMethod]
        public void Or_WithMultipleFilters_ReturnsCombinedOrCondition()
        {
            Assert.AreEqual(
                "{\"_OR\": [{\"param1\":{\"type\": \"eq\", \"value\": \"v1\"}},{\"param2\":{\"type\": \"in\", \"value\": \"v2\"}}]}",
                new FilterBuilderTestClass().Or(
                    fb => fb.Filter("param1", "eq", "v1"),
                    fb => fb.Filter("param2", "in", "v2")
                )
            );
        }

        [TestMethod]
        public void Or_WithNullFilters_IgnoreNulls()
        {
            Assert.AreEqual(
                "{\"_OR\": [{\"param1\":{\"type\": \"eq\", \"value\": \"v1\"}},{\"param2\":{\"type\": \"in\", \"value\": \"v2\"}}]}",
                new FilterBuilderTestClass().Or(
                    fb => fb.Filter("param1", "eq", "v1"),
                    null,
                    fb => fb.Filter("param2", "in", "v2")
                )
            );
        }

        [TestMethod]
        public void Or_WithNullArray_ThrowsArgumentException()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new FilterBuilderTestClass().Or(null)
            );

            StringAssert.Contains(exception.Message, "At least one filter must be provided for OR operation");
        }

        [TestMethod]
        public void Or_WithEmptyArray_ThrowsArgumentException()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new FilterBuilderTestClass().Or()
            );

            StringAssert.Contains(exception.Message, "At least one filter must be provided for OR operation");
        }

        [TestMethod]
        public void Filter_WithStringValue_ReturnsCorrectFormat()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"like\", \"value\": \"test value\"}}",
                new FilterBuilderTestClass().Filter("param", "like", "test value")
            );
        }

        [TestMethod]
        public void Filter_WithNullParam_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new FilterBuilderTestClass().Filter(null, "=", "value")
            );

            StringAssert.Contains(exception.Message, "Value cannot be null");
            Assert.AreEqual("param", exception.ParamName);
        }

        [TestMethod]
        public void Filter_WithNullOperation_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new FilterBuilderTestClass().Filter("param", null, "value")
            );

            StringAssert.Contains(exception.Message, "Value cannot be null");
            Assert.AreEqual("operation", exception.ParamName);
        }

        [TestMethod]
        public void Filter_WithNullValue_HandlesNullValue()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"=\", \"value\": \"\"}}",
                new FilterBuilderTestClass().Filter("param", "=", null)
            );
        }

        [TestMethod]
        public void Filter_WithQuotedArray_ReturnsCorrectFormat()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"in\", \"value\": [\"v1\",\"v2\",\"v3\"]}}",
                new FilterBuilderTestClass().Filter("param", "in", new[] { "v1", "v2", "v3" }, quoteValues: true)
            );
        }

        [TestMethod]
        public void Filter_WithStringArrayQuotingDisabled_ReturnsCorrectFormat()
        {
            Assert.AreEqual(
                "{\"param\":{\"type\": \"in\", \"value\": [v1,v2,v3]}}",
                new FilterBuilderTestClass().Filter("param", "in", new[] { "v1", "v2", "v3" }, quoteValues: false)
            );
        }

        [TestMethod]
        public void FilterArray_WithNullParam_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new FilterBuilderTestClass().Filter(null, "=", new[] { "value" })
            );

            StringAssert.Contains(exception.Message, "Value cannot be null");
            Assert.AreEqual("param", exception.ParamName);
        }

        [TestMethod]
        public void FilterArray_WithNullOperation_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new FilterBuilderTestClass().Filter("param", null, new[] { "value" })
            );

            StringAssert.Contains(exception.Message, "Value cannot be null");
            Assert.AreEqual("operation", exception.ParamName);
        }

        [TestMethod]
        public void FilterArray_WithNullValue_HandlesNullValue()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new FilterBuilderTestClass().Filter("param", "eq", null, quoteValues: true)
            );

            StringAssert.Contains(exception.Message, "Value cannot be null");
            Assert.AreEqual("values", exception.ParamName);
        }

        [TestMethod]
        public void EnumToStringValues_WithEnumMemberAttribute_ReturnsCorrectStringValues()
        {
            CollectionAssert.AreEqual(
                new[] { "document.complete", "document.fieldinvite.delete", "document_group.invite.resend" },
                new FilterBuilderTestClass().EnumToStringValues(
                    new[] { EventType.DocumentComplete, EventType.DocumentFieldInviteDelete, EventType.DocumentGroupInviteResend }
                )
            );
        }
        
    }
}
