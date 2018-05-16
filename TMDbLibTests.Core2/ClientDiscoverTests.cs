﻿using System;
using System.Linq;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Core2.Helpers;
using TMDbLibTests.Core2.JsonHelpers;
using Xunit;

namespace TMDbLibTests.Core2
{
    public class ClientDiscoverTests : TestBase
    {
        [Fact]
        public void TestDiscoverTvShowsNoParams()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.DiscoverTvShowsAsync().Query(i).Result);

            SearchContainer<SearchTv> result = Config.Client.DiscoverTvShowsAsync().Query().Result;

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public void TestDiscoverTvShows()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            DiscoverTv query = Config.Client.DiscoverTvShowsAsync()
                    .WhereVoteCountIsAtLeast(100)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }

        [Fact]
        public void TestDiscoverMoviesNoParams()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.DiscoverMoviesAsync().Query(i).Result);

            SearchContainer<SearchMovie> result = Config.Client.DiscoverMoviesAsync().Query().Result;

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public void TestDiscoverMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            DiscoverMovie query = Config.Client.DiscoverMoviesAsync()
                    .WhereVoteCountIsAtLeast(1000)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }

        [Fact]
        public void TestDiscoverMoviesRegion()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            DiscoverMovie query = Config.Client.DiscoverMoviesAsync().WhereReleaseDateIsInRegion("BR").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }

        [Fact]
        public void TestDiscoverMoviesLanguage()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            DiscoverMovie query = Config.Client.DiscoverMoviesAsync().WhereLanguageIs("da-DK").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            Assert.Equal("Deadpool 2", query.Query(0).Result.Results[11].Title);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }
    }
}