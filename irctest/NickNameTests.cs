using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace irctest
{
    using irc.protocol;

    [TestClass]
    public class NickNameTests
    {
        [TestMethod]
        public void TestConstruction()
        {
            var testString = "leeter";
            var nickName = new NickName(testString);
            Assert.AreEqual(testString, nickName.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBadNick()
        {
            var testString = "-leeter";
            var nickName = new NickName(testString);
            Assert.AreEqual(testString, nickName.Value);
        }
    }
}
