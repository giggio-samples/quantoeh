﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToTwitter
{
    /// <summary>
    /// social graph information
    /// </summary>
    [Serializable]
    public class SocialGraph
    {
        /// <summary>
        /// type of request
        /// </summary>
        public SocialGraphType Type { get; set; }

        /// <summary>
        /// The ID or screen_name of the user to retrieve the friends ID list for
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Specfies the ID of the user for whom to return the friends list. 
        /// Helpful for disambiguating when a valid user ID is also a valid screen name. 
        /// </summary>
        public ulong UserID { get; set; }

        /// <summary>
        /// Specfies the screen name of the user for whom to return the friends list. 
        /// Helpful for disambiguating when a valid screen name is also a user ID.
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// Page to return
        /// </summary>
        [Obsolete("This property has been deprecated and will be ignored by Twitter. Please use Cursor/CursorMovement properties instead.")]
        public int Page { get; set; }

        /// <summary>
        /// Indicator for which page to get next
        /// </summary>
        /// <remarks>
        /// This is not a page number, but is an indicator to
        /// Twitter on which page you need back. Your choices
        /// are Previous and Next, which you can find in the
        /// CursorResponse property when your response comes back.
        /// </remarks>
        public string Cursor { get; set; }

        /// <summary>
        /// Contains Next and Previous cursors
        /// </summary>
        /// <remarks>
        /// This is read-only and returned with the response
        /// from Twitter. You use it by setting Cursor on the
        /// next request to indicate that you want to move to
        /// either the next or previous page.
        /// </remarks>
        public Cursors CursorMovement { get; internal set; }

        /// <summary>
        /// List of IDs returned from query
        /// </summary>
        public List<string> IDs { get; set; }
    }
}
