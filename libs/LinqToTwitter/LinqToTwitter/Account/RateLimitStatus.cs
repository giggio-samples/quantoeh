﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToTwitter
{
    /// <summary>
    /// holds rate limit info
    /// </summary>
    [Serializable]
    public class RateLimitStatus
    {
        public int RemainingHits { get; set; }

        public int HourlyLimit { get; set; }

        public DateTime ResetTime { get; set; }

        public int ResetTimeInSeconds { get; set; }
    }
}
