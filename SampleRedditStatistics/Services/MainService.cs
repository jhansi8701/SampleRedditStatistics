using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;
using Timer = System.Timers.Timer;

namespace SampleRedditStatistics.Services
{
    public class MainService
    {
        private readonly IStatistics _statistics = null;
        private readonly ILogger<MainService> _logger = null;

        private const double INTERVAL = 5000;


        private bool isServiceRunning = false;

        private Timer timer = null;

        private string subreddit = null;

      
        public MainService(IStatistics statistics, ILogger<MainService> logger)
        {
            _statistics = statistics;
            _logger = logger;
            /* Time needed to to constantly poll the reddit api. We could windows service or hangfire too. */
             /* As the requirement says needs to log the statistics to to terminal I have chosen console appplication */
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = INTERVAL;
        }

        public void StartService(string subreddit)
        {
            timer.Enabled = true;
            this.subreddit = subreddit;
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            /* IsServiceRunning parameter is required as this event is multi threaded . */
            /* To avoid concurrency issues and to achieve async mechanism we need this parameter */

            if (isServiceRunning)
            {
                return;
            }
            else
            {
                try
                {
                    isServiceRunning = true;
                    IList<string> posts = await _statistics.GetPostsWithMostUpVotes(subreddit);

                    _logger.LogInformation("Posts with Most upvotes: ");

                    foreach (string post in posts)
                    {
                        Console.WriteLine(post);
                    }

                    _logger.LogInformation("=====================================================");

                    IList<string> users = await _statistics.GetUsersWithMostPosts(subreddit);
                    _logger.LogInformation("Users with Most Posts: ");

                    foreach (string user in users)
                    {
                        Console.WriteLine(user);
                    }
                    _logger.LogInformation("=====================================================");

                    Thread.Sleep(2000);
                    isServiceRunning = false;
                }
                catch (Exception ex)
                {
                    isServiceRunning = false;
                    _logger.LogError(ex, ex.StackTrace);
                }
            }
        }

    }
}
