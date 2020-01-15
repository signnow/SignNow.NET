using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using Method = System.Net.Http.HttpMethod;

namespace UnitTests
{
    [TestClass]
    public class RequestOptionsTest
    {
        private JsonHttpContent content;
        private Uri requestUrl;
        private Token token;

        [TestInitialize]
        public void Setup()
        {
            content = new JsonHttpContent(new { document_id = "test" });
            requestUrl = new Uri($"https://signnow.com");
            token = new Token
            {
                AccessToken = "12345",
                RefreshToken = "12345",
                Scope = Scope.All.ToString()
            };
        }

        [TestMethod]
        public void ShouldProperCreateGetRequest()
        {
            var options = new GetHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.IsNull(options.Content);
            Assert.AreEqual(Method.Get, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
        }

        [TestMethod]
        public void ShouldProperCreatePostRequest()
        {
            var options = new PostHttpRequestOptions
            {
                Content = content,
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.AreEqual(Method.Post, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
            Assert.AreEqual(content, options.Content);
        }

        [TestMethod]
        public void ShouldProperCreatePutRequest()
        {
            var options = new PutHttpRequestOptions
            {
                Content = content,
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.AreEqual(Method.Put, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
            Assert.AreEqual(content, options.Content);
        }

        [TestMethod]
        public void ShouldProperCreateDeleteRequest()
        {
            var options = new DeleteHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.IsNull(options.Content);
            Assert.AreEqual(Method.Delete, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
        }

        [TestMethod]
        public void ShouldProperCreateHeadRequest()
        {
            var options = new HeadHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.IsNull(options.Content);
            Assert.AreEqual(Method.Head, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
        }

        [TestMethod]
        public void ShouldProperCreateOptionsRequest()
        {
            var options = new OptionsHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = token
            };

            Assert.IsNull(options.Content);
            Assert.AreEqual(Method.Options, options.HttpMethod);
            Assert.AreEqual(requestUrl, options.RequestUrl);
        }
    }
}
