using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GotConnection.Test
{
    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void ConstructorEmpty()
        {
            var options = new Options();
            Assert.IsNotNull(options);
            Assert.AreEqual(0, options.Count);
        }

        [TestMethod]
        public void ConstructorDictionary()
        {
            var options = new Options(new Dictionary<string, object>());
            Assert.IsNotNull(options);
            Assert.AreEqual(0, options.Count);
        }

        [TestMethod]
        public void ConstructorObject()
        {
            var options = new Options(new { });
            Assert.IsNotNull(options);
            Assert.AreEqual(0, options.Count);
        }

        [TestMethod]
        public void ConstructWithValueDictionary()
        {
            var testData = new Dictionary<string, object> {{"MyTest", "result"}};

            var options = new Options(testData);
            Assert.AreEqual(1, options.Count);
            Assert.AreEqual("result", options["MyTest"]);
        }

        [TestMethod]
        public void ConstructWithValueObject()
        {
            var options = new Options(new {MyTest = "result"});
            Assert.AreEqual(1, options.Count);
            Assert.AreEqual("result", options["MyTest"]);
        }

        [TestMethod]
        public void AddValue()
        {
            var options = new Options();
            options.Add("MyTest", "result");
            Assert.AreEqual(1, options.Count);
            Assert.AreEqual("result", options["MyTest"]);
        }
    }
}