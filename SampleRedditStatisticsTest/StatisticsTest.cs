using Moq;
using Newtonsoft.Json.Linq;
using SampleRedditStatistics.Services;

namespace SampleRedditStatisticsTest
{
    public class StatisticsTest
    {       

        private readonly Mock<IStatistics> _moqStatisticsService;

        public StatisticsTest()
        {
            _moqStatisticsService = new Mock<IStatistics>();           
        }

        [Theory]
        [InlineData("abcxyz")]
        public void GetSuccessNoPosts(string value)
        {
            IList<string> posts = new List<string>();
          _moqStatisticsService.Setup(x => x.GetPostsWithMostUpVotes(value))
                .ReturnsAsync(posts);

            Assert.Empty(posts);

        }

        [Theory]
        [InlineData("abcxyz")]
        public void GetSuccessNoUsers(string value)
        {
            IList<string> users= new List<string>();
            _moqStatisticsService.Setup(x => x.GetUsersWithMostPosts(value))
                  .ReturnsAsync(users);

            Assert.Empty(users);

        }

        [Fact]
        public void GetExceptionForPostsWithMostUpVotes() {
            ArgumentNullException exception = new ArgumentNullException("Invalid null arugument");
            _moqStatisticsService.Setup(x => x.GetPostsWithMostUpVotes(null)).ThrowsAsync(exception);

            Assert.Equal(exception.ParamName, "Invalid null arugument");

        }

        [Fact]
        public void GetExceptionForUsersWithMostPosts()
        {
            ArgumentNullException exception = new ArgumentNullException("Invalid null arugument");
            _moqStatisticsService.Setup(x => x.GetUsersWithMostPosts(null)).ThrowsAsync(exception);

            Assert.Equal(exception.ParamName, "Invalid null arugument");

        }
    }
}