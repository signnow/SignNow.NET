using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;

namespace UnitTests.Extensions
{
    [TestClass]
    public class ScopeExtensionsTest
    {
        [TestMethod]
        public void ShouldProperConvertScopeToString()
        {
            Assert.AreEqual("*", Scope.All.AsString());
            Assert.AreEqual("user", Scope.User.AsString());
        }

        [TestMethod]
        public void ThrowsExceptionForWrongScopeConverting()
        {
            Assert.ThrowsException<NotImplementedException>(
                () =>
                {
                    var userScope = Scope.User;
                    var brokenScope = ++userScope;
                    brokenScope.AsString();
                });
        }
    }
}
