﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToTwitter
{
    /// <summary>
    /// helps to work with trends
    /// </summary>
    [Serializable]
    public class Trend
    {
        /// <summary>
        /// type of trend to query (Trend (all), Current, Daily, or Weekly)
        /// </summary>
        public TrendType Type { get; set; }

        /// <summary>
        /// exclude all trends with hastags if set to true 
        /// (i.e. include "Wolverine" but not "#Wolverine")
        /// </summary>
        public bool ExcludeHashtags { get; set; }

        /// <summary>
        /// date to start
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// time of request
        /// </summary>
        public DateTime AsOf { get; set; }

        /// <summary>
        /// twitter search query on topic
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Search URL returned from Local Trends
        /// </summary>
        public string SearchUrl { get; set; }

        /// <summary>
        /// name of trend topic
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Yahoo Where On Earth ID
        /// </summary>
        public int WeoID { get; set; }

        /// <summary>
        /// Location where trend is occurring
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Return value for Avalable query listing locations of trending topics
        /// </summary>
        public List<Location> Locations { get; set; }
    }
}
