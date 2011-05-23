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
        public void DefaultOnline()
        {
            var twitter = new GotConnection.Twitter(null);
            Assert.IsTrue(twitter.IsOnline);
        }

        [TestMethod]
        public void SpecifyOnline()
        {
            var twitter = new GotConnection.Twitter(new {online = true});
            Assert.IsTrue(twitter.IsOnline);
        }

        [TestMethod]
        public void SpecifyOffline()
        {
            var twitter = new GotConnection.Twitter(new {online = false});
            Assert.IsFalse(twitter.IsOnline);
        }

        [TestMethod]
        public void WorksOnline()
        {
            ITwitter twitter = ConnectTo.Twitter(null);
            var result = twitter.TimeLine("WestDiscGolf", 1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WorksOnlineXml()
        {
            ITwitter twitter = ConnectTo.Twitter(new {format = GotConnection.Twitter.Format.xml});
            var result = twitter.TimeLine("WestDiscGolf", 1);
            var doc = new XmlDocument();
            doc.LoadXml(result);
            Assert.AreEqual(1, doc.SelectNodes("//status").Count);
        }

        [TestMethod]
        public void TestOfflineJson()
        {
            ITwitter twitter = ConnectTo.Twitter(new {online = false});
            var result = twitter.TimeLine("WestDiscGolf", 2);
            Assert.IsNotNull(result);
        }
    }
}
