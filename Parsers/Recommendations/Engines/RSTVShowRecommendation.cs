﻿namespace RoliSoft.TVShowTracker.Parsers.Recommendations.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using NUnit.Framework;

    /// <summary>
    /// Provides support for the service located at http://lab.rolisoft.net/tv/
    /// </summary>
    [TestFixture]
    public class RSTVShowRecommendation : RecommendationEngine
    {
        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return "RS TV Show Recommendation";
            }
        }

        /// <summary>
        /// Gets the URL of the site.
        /// </summary>
        /// <value>The site location.</value>
        public override string Site
        {
            get
            {
                return "http://lab.rolisoft.net/tv/";
            }
        }

        /// <summary>
        /// Gets the URL to the favicon of the site.
        /// </summary>
        /// <value>
        /// The icon location.
        /// </value>
        public override string Icon
        {
            get
            {
                return "http://rolisoft.net/favicon.ico";
            }
        }

        /// <summary>
        /// Gets the name of the plugin's developer.
        /// </summary>
        /// <value>The name of the plugin's developer.</value>
        public override string Developer
        {
            get
            {
                return "RoliSoft";
            }
        }

        /// <summary>
        /// Gets the version number of the plugin.
        /// </summary>
        /// <value>The version number of the plugin.</value>
        public override Version Version
        {
            get
            {
                return Utils.DateTimeToVersion("2011-07-12 2:46 PM");
            }
        }

        private readonly int _type;
        private readonly string _key  = "S2qNfbCFCWoQ8RoL1S0FTbjbW",
                                _uuid = Utils.GetUUID();

        /// <summary>
        /// Initializes a new instance of the <see cref="RSTVShowRecommendation"/> class.
        /// </summary>
        public RSTVShowRecommendation()
        {
            _type = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RSTVShowRecommendation"/> class.
        /// </summary>
        /// <param name="type">The type of the algorithm to use.</param>
        public RSTVShowRecommendation(int type)
        {
            _type = type;
        }

        /// <summary>
        /// Gets the list of recommended TV show from the engine.
        /// </summary>
        /// <param name="shows">The currently watched shows.</param>
        /// <returns>Recommended shows list.</returns>
        public override IEnumerable<RecommendedShow> GetList(IEnumerable<string> shows)
        {
            var lab = Utils.GetXML("http://lab.rolisoft.net/tv/api.php?key=" + _key + "&uid=" + _uuid + (_type == 1 ? "&genre=true" : String.Empty) + "&output=xml" + shows.Aggregate(String.Empty, (current, r) => current + ("&show[]=" + Utils.EncodeURL(r))));

            return lab.Descendants("show")
                   .Select(item => new RecommendedShow
                   {
                       Name      = item.Value,
                       Score     = item.Attribute("score").Value,
                       Official  = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " official site"),
                       Wikipedia = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " TV Series site:en.wikipedia.org"),
                       TVRage    = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " intitle:\"TV Show\" site:tvrage.com"),
                       TVDB      = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " intitle:\"Series Info\" site:thetvdb.com"),
                       TVcom     = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " intitle:\"on TV.com\" inurl:summary.html site:tv.com"),
                       Epguides  = item.Attribute("epguides").Value,
                       Imdb      = item.Attribute("imdb").Value,
                       TVTropes  = "http://www.google.com/search?btnI=I'm+Feeling+Lucky&hl=en&q=" + Utils.EncodeURL(item.Value + " site:tvtropes.org")
                   });
        }
    }
}
