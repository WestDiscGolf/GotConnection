using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;

namespace GotConnection
{
    public class Twitter : ITwitter
    {
        // valid options to be passed in which can be overridden
        private const string FormatKey = "format";
        private const string Count = "count";
        private const string NativeRetweets = "include_rts";

        /// <summary>
        /// Defaults to not in development
        /// </summary>
        public static bool InDevelopement
        {
            get
            {
                var response = ConfigurationManager.AppSettings["GotConnection.Twitter.InDevelopment"];
                bool result;
                if (bool.TryParse(response, out result))
                {
                    return result;
                }
                return false;
            }
        }

        /// <summary>
        /// Enum format the same as specified in the Twitter api documentation
        /// </summary>
        public enum Format
        {
            json,
            xml,
            rss,
            atom
        }

        private Options _options;

        /// <summary>
        /// Setup the defaults, adding in if they've not been specified
        /// </summary>
        /// <param name="options"></param>
        private void Extend(object options)
        {
            // load the passed in options
            _options = new Options(options);

            // default json format
            _options.SetDefault(FormatKey, Format.json);
            
            // default return 5
            _options.SetDefault(Count, 5);

            // include native retweets
            _options.SetDefault(NativeRetweets, true);
        }

        /// <summary>
        /// Gets the timeline for the specified username and most recent tweets as defined.
        /// </summary>
        /// <param name="username">Twitter username the timeline is required for</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public string TimeLine(string username, object options = null)
        {
            Extend(options);

            string timelineUrl = string.Format("http://api.twitter.com/1/statuses/user_timeline.{0}?screen_name={1}", _options[FormatKey], username);

            if (_options.Contains(Count))
            {
                timelineUrl = string.Format("{0}&count={1}", timelineUrl, _options[Count]);
            }

            if (_options.Contains(NativeRetweets))
            {
                timelineUrl = string.Format("{0}&include_rts={1}", timelineUrl, _options[NativeRetweets]);
            }

            var result = string.Empty;
            var webRequest = WebRequest.Create(timelineUrl);
            webRequest.Timeout = 2000;

            try
            {
                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var receiveStream = response.GetResponseStream();
                        if (receiveStream != null)
                        {
                            var stream = new StreamReader(receiveStream);
                            result = stream.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException)
            {
                if (InDevelopement)
                {
                    result = InDevelopmentTimeline(username);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Unplugged response. Returns some dummy test data depending on the format specified
        /// </summary>
        /// <param name="username"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private string InDevelopmentTimeline(string username)
        {
            var result = string.Empty;

            switch ((Format)_options[FormatKey])
            {
                case Format.json:
                    List<string> status = new List<string>();
                    for (int index = 0; index < (int)_options[Count]; index++)
                    {
                        status.Add(Status);
                    }
                    result = string.Format("[{0}]", string.Join(",", status)); // outter array
                    break;

                default:
                    throw new NotImplementedException("Only offline json is supported at the moment");
            }

            return result;
        }

        // This is not nice and will be addressing this in future releases.
        private string Status = "{\"in_reply_to_status_id_str\": null,\"text\": \"This is a test tweet, nothing to see here, move along!\",\"contributors\": null,\"place\": null,\"retweeted\": false,\"in_reply_to_user_id\": null,\"in_reply_to_user_id_str\": null,\"coordinates\": null,\"geo\": null,\"truncated\": false,\"source\": \"web\",\"created_at\": \"Sat May 21 17:42:32 +0000 2011\",\"in_reply_to_status_id\": null,\"id_str\": \"71994297272111104\",\"favorited\": false,\"user\": {\"default_profile_image\": false,\"contributors_enabled\": false,\"lang\": \"en\",\"show_all_inline_media\": false,\"verified\": false,\"geo_enabled\": false,\"profile_link_color\": \"009999\",\"description\": \"Husband, Developer, Disc Golfer\",\"location\": \"Mars\",\"profile_sidebar_border_color\": \"eeeeee\",\"followers_count\": 74,\"follow_request_sent\": null,\"time_zone\": \"London\",\"friends_count\": 80,\"url\": \"http://westdiscgolf.blogspot.com\",\"is_translator\": false,\"statuses_count\": 2835,\"profile_use_background_image\": true,\"favourites_count\": 0,\"following\": null,\"notifications\": null,\"created_at\": \"Sat Nov 07 17:14:23 +0000 2009\",\"profile_background_color\": \"131516\",\"profile_background_image_url\": \"http://a1.twimg.com/images/themes/theme14/bg.gif\",\"listed_count\": 4,\"protected\": false,\"profile_image_url\": \"http://a2.twimg.com/profile_images/514658470/me_and_oakleys_normal.jpg\",\"id_str\": \"88226617\",\"profile_text_color\": \"333333\",\"name\": \"Adam Storr\",\"profile_sidebar_fill_color\": \"efefef\",\"screen_name\": \"WestDiscGolf\",\"id\": 88226617,\"default_profile\": false,\"profile_background_tile\": true,\"utc_offset\": 0},\"retweet_count\": 0,\"id\": 71994297272111104,\"in_reply_to_screen_name\": null}";
    }
}