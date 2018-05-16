using TMDbLib.Objects.TvShows;
using TMDbLibTests.Core2.Helpers;
using TMDbLibTests.Core2.JsonHelpers;
using Xunit;

namespace TMDbLibTests.Core2
{
    public class ClientNetworkTests : TestBase
    {
        [Fact]
        public void TestNetworkGetById()
        {
            Network network = Config.Client.GetNetworkAsync(IdHelper.Hbo).Result;

            Assert.NotNull(network);
            Assert.Equal("HBO", network.Name);
            Assert.Equal(IdHelper.Hbo, network.Id);
        }
    }
}
