using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GotConnection.Test
{
    /// <summary>
    /// Unit tests require an internet connection
    /// </summary>
    [TestClass]
    public class Twitter
    {
        [TestMethod]
        public void InDevelopment()
        {
            Assert.IsFalse(GotConnection.Twitter.InDevelopement);
        }

        [TestMethod]
        public void WorksOnline()
        {
            ITwitter twitter = ConnectTo.Twitter();
            var result = twitter.TimeLine("WestDiscGolf", 1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WorksOnlineOptionsOverload()
        {
            ITwitter twitter = ConnectTo.Twitter();
            var result = twitter.TimeLine("WestDiscGolf", new { count = 1 });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WorksOnlineXml()
        {
            ITwitter twitter = ConnectTo.Twitter();
            var result = twitter.TimeLine("WestDiscGolf", new { format = GotConnection.Twitter.Format.xml, count = 1 });
            var doc = new XmlDocument();
            doc.LoadXml(result);
            Assert.AreEqual(1, doc.SelectNodes("//status").Count);
        }
    }
}