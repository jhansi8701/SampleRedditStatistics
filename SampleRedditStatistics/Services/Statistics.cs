using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SampleRedditStatistics.Services
{
    public class Statistics : IStatistics
    {
        private HttpClient client = null;
        public Statistics()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://oauth.reddit.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Please replace the bearer token ABCXYZ with the actual token I have written in the note pad that sent to recruiter. 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ABCXYZ");
            client.DefaultRequestHeaders.Add("User-Agent", Uri.EscapeDataString("appname:platform:version (by /u/jhansim)"));
        }

        public async Task<IList<string>> GetPostsWithMostUpVotes(string subreddit)
        {
            if (string.IsNullOrEmpty(subreddit))
            {
                throw new ArgumentNullException("Invalid null arugument");
            }
            string apiPath = $"/r/{subreddit}/top?limit=5";
            IList<string> result = new List<string>();

            var jsonResult = await client.GetStringAsync(apiPath);
            JObject parsedObj = JObject.Parse(jsonResult);

            if(parsedObj == null)
            {
                return result;
            }

            JArray  nestedArrayObjs = parsedObj["data"]["children"].ToObject<JArray>();
                       

            foreach (var obj in nestedArrayObjs)
            {
                string value = obj.SelectToken("data.title")?.Value<string>();

                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }

            }

            return result;
        }

        public async Task<IList<string>> GetUsersWithMostPosts(string subreddit)
        {
            if (string.IsNullOrEmpty(subreddit))
            {
                throw new ArgumentNullException("Invalid null arugument");
            }

            string apiPath = $"/r/{subreddit}?limit=25";

            var jsonResult = await client.GetStringAsync(apiPath);
            JObject parsedObj = JObject.Parse(jsonResult);

            if(parsedObj == null)
            {
                return new List<string>();
            }

            JArray nestedArrayObjs = parsedObj["data"]["children"].ToObject<JArray>();

            IList<JObject> intermedicateResult = new List<JObject>();

            foreach (var obj in nestedArrayObjs)
            {
                JObject valueObj = obj.SelectToken("data").Value<JObject>();

                if (valueObj != null)
                {
                    intermedicateResult.Add(valueObj);
                }

            }

            IList<string> result = intermedicateResult.GroupBy(r => new { User = r["author"] }).Select(group => new
            {
                User = group.Key.User,
                Count = group.Count()
            }).OrderByDescending(o => o.Count).Take(5).Select(c => c.User.Value<string>()).ToList();


            return result;
        }
    }
}
