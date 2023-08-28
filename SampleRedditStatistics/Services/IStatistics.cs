using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleRedditStatistics.Services
{
    /// <summary>
    /// Statistics interface
    /// </summary>
    public interface IStatistics
    {
        /// <summary>
        /// Gets the posts with more upvotes for the given subreddit
        /// </summary>
        /// <param name="subreddit">music</param>
        /// <returns></returns>
        Task<IList<string>> GetPostsWithMostUpVotes(string subreddit);

        /// <summary>
        /// Gets the users with more posts for the given subreddit
        /// </summary>
        /// <param name="subreddit">music</param>
        /// <returns></returns>
        Task<IList<string>> GetUsersWithMostPosts(string subreddit);
    }
}
