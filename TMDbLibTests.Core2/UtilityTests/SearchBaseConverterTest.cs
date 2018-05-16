using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.Core2.Helpers;
using TMDbLibTests.Core2.JsonHelpers;
using Xunit;

namespace TMDbLibTests.Core2.UtilityTests
{
    public class SearchBaseConverterTest : TestBase
    {
        [Fact]
        public void SearchBaseConverter_Movie()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchMovie original = new SearchMovie();
            original.OriginalTitle = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchMovie result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchMovie;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        }

        [Fact]
        public void SearchBaseConverter_Tv()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchTv original = new SearchTv();
            original.OriginalName = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchTv result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchTv;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalName, result.OriginalName);
        }

        [Fact]
        public void SearchBaseConverter_Person()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchPerson original = new SearchPerson();
            original.Name = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchPerson result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchPerson;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.Name, result.Name);
        }

        /// <summary>
        /// Tests the SearchBaseConverter
        /// </summary>
        [Fact]
        public void TestSearchBaseConverter()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchMultiAsync("Rock", i).Sync());
            SearchContainer<SearchBase> result = Config.Client.SearchMultiAsync("Rock").Sync();

            var totalResults = result.Results;

            for (int i = 1; i <= result.TotalPages; i++)
            {
                SearchContainer<SearchBase> resultI = Config.Client.SearchMultiAsync("Rock", i).Sync();
                totalResults.AddRange(resultI.Results);
            }

            Assert.NotNull(totalResults);

            Assert.Contains(totalResults, item => item.MediaType == MediaType.Tv && item is SearchTv);
            Assert.Contains(totalResults, item => item.MediaType == MediaType.Movie && item is SearchMovie);
            Assert.Contains(totalResults, item => item.MediaType == MediaType.Person && item is SearchPerson);
        }
    }
}