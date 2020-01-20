using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
    [TestClass]
    public class ErrorResponseTest
    {
        [TestMethod]
        public void ShouldProcessErrorMessage()
        {
            var json = @"{
                'error': 'context of error'
            }";

            var json404 = @"{
                '404': 'context of error404'
            }";

            var jsonError = @"{
                'errors': [
                    {
                        'message': 'context of single errors item',
                        'code': 127
                    }
                ]
            }";

            var jsonErrors = @"{
                'errors': [
                    {
                        'code': 41,
                        'message': 'context of first errors item'
                    },
                    {
                        'code': 42,
                        'message': 'context of second errors item'
                    }
                ]
            }";

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(json);
            var errorResponse404 = JsonConvert.DeserializeObject<ErrorResponse>(json404);
            var errorResponseError = JsonConvert.DeserializeObject<ErrorResponse>(jsonError);
            var errorResponseErrors = JsonConvert.DeserializeObject<ErrorResponse>(jsonErrors);

            Assert.AreEqual("context of error", errorResponse.GetErrorMessage());
            Assert.AreEqual("context of error404", errorResponse404.GetErrorMessage());
            Assert.AreEqual("context of single errors item", errorResponseError.GetErrorMessage());
            Assert.AreEqual(
                $"context of first errors item{Environment.NewLine}context of second errors item{Environment.NewLine}",
                errorResponseErrors.GetErrorMessage()
                );
        }
    }
}
