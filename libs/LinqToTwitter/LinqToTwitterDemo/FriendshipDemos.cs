﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToTwitter;

namespace LinqToTwitterDemo
{
    /// <summary>
    /// Shows friendship demos
    /// </summary>
    public class FriendshipDemos
    {
        /// <summary>
        /// Run all friendship related demos
        /// </summary>
        /// <param name="twitterCtx">TwitterContext</param>
        public static void Run(TwitterContext twitterCtx)
        {
            //CreateFriendshipFollowDemo(twitterCtx);
            //DestroyFriendshipDemo(twitterCtx);
            //CreateFriendshipNoDeviceUpdatesDemo(twitterCtx);
            FriendshipExistsDemo(twitterCtx);
            //FriendshipShowDemo(twitterCtx);
            //FriendshipIncomingDemo(twitterCtx);
            //FriendshipOutgoingDemo(twitterCtx);
        }

        #region Friendship Demos

        private static void CreateFriendshipNoDeviceUpdatesDemo(TwitterContext twitterCtx)
        {
            var user = twitterCtx.CreateFriendship("LinqToTweeter", string.Empty, string.Empty, false);

            Console.WriteLine(
                "User Name: {0}, Status: {1}",
                user.Name,
                user.Status.Text);
        }

        private static void DestroyFriendshipDemo(TwitterContext twitterCtx)
        {
            var user = twitterCtx.DestroyFriendship("LinqToTweeter", string.Empty, string.Empty);

            Console.WriteLine(
                "User Name: {0}, Status: {1}",
                user.Name,
                user.Status.Text);
        }

        private static void CreateFriendshipFollowDemo(TwitterContext twitterCtx)
        {
            var user = twitterCtx.CreateFriendship("LinqToTweeter", string.Empty, string.Empty, true);

            Console.WriteLine(
                "User Name: {0}, Status: {1}",
                user.Name,
                user.Status.Text);
        }

        /// <summary>
        /// shows how to show that one user follows another with Friendship Exists
        /// </summary>
        /// <param name="twitterCtx"></param>
        private static void FriendshipExistsDemo(TwitterContext twitterCtx)
        {
            var friendship =
                (from friend in twitterCtx.Friendship
                 where friend.Type == FriendshipType.Exists &&
                       friend.SubjectUser == "JoeMayo" &&
                       friend.FollowingUser == "LinqToTweeter"
                 select friend)
                 .FirstOrDefault();

            Console.WriteLine(
                "LinqToTweeter follows JoeMayo: " +
                friendship.IsFriend);
        }

        /// <summary>
        /// shows how to show that one user follows another with Friendship Exists
        /// </summary>
        /// <param name="twitterCtx"></param>
        private static void FriendshipShowDemo(TwitterContext twitterCtx)
        {
            var friendship =
                (from friend in twitterCtx.Friendship
                 where friend.Type == FriendshipType.Show &&
                       friend.SourceScreenName == "JoeMayo" &&
                       friend.TargetScreenName == "LinqToTweeter"
                 select friend)
                 .First();

            Console.WriteLine(
                "\nJoeMayo follows LinqToTweeter: " + 
                friendship.SourceRelationship.FollowedBy + 
                "\nLinqToTweeter follows JoeMayo: " +
                friendship.TargetRelationship.FollowedBy);
        }

        /// <summary>
        /// Shows how to check who has an incoming request to logged in user's locked account
        /// </summary>
        /// <param name="twitterCtx">twitterCtx</param>
        private static void FriendshipIncomingDemo(TwitterContext twitterCtx)
        {
            var request =
                (from req in twitterCtx.Friendship
                 where req.Type == FriendshipType.Incoming
                 select req)
                 .FirstOrDefault();

            request.IDInfo.IDs.ForEach(req => Console.WriteLine(req));
        }

        /// <summary>
        /// Shows all outgoing requests from the logged in user to locked accounts
        /// </summary>
        /// <param name="twitterCtx">twitterCtx</param>
        private static void FriendshipOutgoingDemo(TwitterContext twitterCtx)
        {
            var request =
                (from req in twitterCtx.Friendship
                 where req.Type == FriendshipType.Outgoing
                 select req)
                 .FirstOrDefault();

            request.IDInfo.IDs.ForEach(req => Console.WriteLine(req));
        }

        #endregion

    }
}
