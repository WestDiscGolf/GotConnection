using System.Web.Mvc;

namespace SampleMvcSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetTweets()
        {
            GotConnection.ITwitter twitter = GotConnection.ConnectTo.Twitter();
            var result = twitter.TimeLine("WestDiscGolf", new { count = 5 });

            return new ContentResult { Content = result, ContentType = "application/json" };            
        }
    }
}
