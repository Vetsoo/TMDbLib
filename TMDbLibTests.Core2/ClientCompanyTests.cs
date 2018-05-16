﻿using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Core2.Helpers;
using TMDbLibTests.Core2.JsonHelpers;
using Xunit;

namespace TMDbLibTests.Core2
{
    public class ClientCompanyTests : TestBase
    {
        private static Dictionary<CompanyMethods, Func<Company, object>> _methods;

        public ClientCompanyTests()
        {
            _methods = new Dictionary<CompanyMethods, Func<Company, object>>
            {
                [CompanyMethods.Movies] = company => company.Movies
            };
        }

        [Fact]
        public void TestCompaniesExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / movies");
            IgnoreMissingCSharp("origin_country / origin_country");

            Company company = Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox).Result;

            Assert.NotNull(company);

            // TODO: Test all properties
            Assert.Equal("20th Century Fox", company.Name);

            // Test all extras, ensure none of them exist
            foreach (Func<Company, object> selector in _methods.Values)
            {
                Assert.Null(selector(company));
            }
        }

        [Fact]
        public void TestCompaniesExtrasExclusive()
        {
            // Ignore missing json
            IgnoreMissingJson("movies.results[array] / media_type");
            IgnoreMissingCSharp("origin_country / origin_country");

            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetCompanyAsync(id, extras).Result, IdHelper.TwentiethCenturyFox);
        }

        [Fact]
        public void TestCompaniesExtrasAll()
        {
            // Ignore missing json
            IgnoreMissingJson("movies.results[array] / media_type");
            IgnoreMissingCSharp("origin_country / origin_country");

            CompanyMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Company item = Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [Fact]
        public void TestCompaniesGetters()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type", "origin_country / origin_country");

            //GetCompanyMoviesAsync(int id, string language, int page = -1)
            SearchContainerWithId<SearchMovie> resp = Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox).Result;
            SearchContainerWithId<SearchMovie> respPage2 = Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, 2).Result;
            SearchContainerWithId<SearchMovie> respItalian = Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, "it").Result;

            Assert.NotNull(resp);
            Assert.NotNull(respPage2);
            Assert.NotNull(respItalian);

            Assert.True(resp.Results.Count > 0);
            Assert.True(respPage2.Results.Count > 0);
            Assert.True(respItalian.Results.Count > 0);

            bool allTitlesIdentical = true;
            for (int index = 0; index < resp.Results.Count; index++)
            {
                Assert.Equal(resp.Results[index].Id, respItalian.Results[index].Id);

                if (resp.Results[index].Title != respItalian.Results[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.False(allTitlesIdentical);
        }

        [Fact]
        public void TestCompaniesImages()
        {
            IgnoreMissingJson(" / movies");
            IgnoreMissingCSharp("origin_country / origin_country");

            // Get config
            Config.Client.GetConfigAsync().Sync();

            // Test image url generator
            Company company = Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox).Result;

            Uri url = Config.Client.GetImageUrl("original", company.LogoPath);
            Uri urlSecure = Config.Client.GetImageUrl("original", company.LogoPath, true);

            Assert.True(TestHelpers.InternetUriExists(url));
            Assert.True(TestHelpers.InternetUriExists(urlSecure));
        }

        [Fact]
        public void TestCompaniesFull()
        {
            IgnoreMissingJson(" / movies");
            IgnoreMissingCSharp("origin_country / origin_country");

            Company company = Config.Client.GetCompanyAsync(IdHelper.ColumbiaPictures).Result;

            Assert.NotNull(company);

            Assert.Equal(IdHelper.ColumbiaPictures, company.Id);
            Assert.Equal("Columbia Pictures Industries, Inc. (CPII) is an American film production and distribution company. Columbia Pictures now forms part of the Columbia TriStar Motion Picture Group, owned by Sony Pictures Entertainment, a subsidiary of the Japanese conglomerate Sony. It is one of the leading film companies in the world, a member of the so-called Big Six. It was one of the so-called Little Three among the eight major film studios of Hollywood's Golden Age.", company.Description);
            Assert.Equal("Culver City, California", company.Headquarters);
            Assert.Equal("http://www.sonypictures.com/", company.Homepage);
            Assert.Equal("/71BqEFAF4V3qjjMPCpLuyJFB9A.png", company.LogoPath);
            Assert.Equal("Columbia Pictures", company.Name);
        }
    }
}
