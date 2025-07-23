using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace UnitTests.Requests
{
    [TestClass]
    public class PagePaginationOptionsTest
    {
        [TestMethod]
        public void CreateProperQueryString()
        {
            var noOptions = new PagePaginationOptions();
            var pageOption = new PagePaginationOptions { Page = 1 };
            var perPageOption = new PagePaginationOptions { PerPage = 10 };
            var fullOption = new PagePaginationOptions { Page = 2, PerPage = 20 };

            Assert.AreEqual(String.Empty, noOptions.ToQueryString());
            Assert.AreEqual("page=1", pageOption.ToQueryString());
            Assert.AreEqual("per_page=10", perPageOption.ToQueryString());
            Assert.AreEqual("page=2&per_page=20", fullOption.ToQueryString());
        }
    }
}
