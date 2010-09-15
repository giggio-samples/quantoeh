﻿using LinqToTwitter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using System;
using System.Collections;

namespace LinqToTwitterTests
{
    
    
    /// <summary>
    ///This is a test class for SavedSearchRequestProcessorTest and is intended
    ///to contain all SavedSearchRequestProcessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SavedSearchRequestProcessorTest
    {
        private TestContext testContextInstance;

        private string m_testQueryResponse = @"<saved_searches type=""array"">
  <saved_search>
    <id>176136</id>
    <name>#csharp</name>
    <query>#csharp</query>
    <position></position>
    <created_at>Mon May 18 20:27:59 +0000 2009</created_at>
  </saved_search>
  <saved_search>
    <id>210448</id>
    <name>#twitterapi</name>
    <query>#twitterapi</query>
    <position></position>
    <created_at>Sat May 23 01:53:52 +0000 2009</created_at>
  </saved_search>
  <saved_search>
    <id>314612</id>
    <name>dotnet</name>
    <query>dotnet</query>
    <position></position>
    <created_at>Fri Jun 05 01:03:52 +0000 2009</created_at>
  </saved_search>
</saved_searches>";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ProcessResults
        ///</summary>
        [TestMethod()]
        public void ProcessResultsTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>();
            XElement twitterResponse = XElement.Parse(m_testQueryResponse);
            IList actual = target.ProcessResults(twitterResponse);
            Assert.AreEqual(actual.Count, 3);
        }

        /// <summary>
        ///A test for GetParameters
        ///</summary>
        [TestMethod()]
        public void GetParametersTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Expression<Func<SavedSearch, bool>> expression =
                search =>
                    search.Type == SavedSearchType.Show &&
                    search.ID == "123";
            LambdaExpression lambdaExpression = expression as LambdaExpression;

            var queryParams = target.GetParameters(lambdaExpression);

            Assert.IsTrue(
                queryParams.Contains(
                    new KeyValuePair<string, string>("Type", ((int)SavedSearchType.Show).ToString())));
            Assert.IsTrue(
                queryParams.Contains(
                    new KeyValuePair<string, string>("ID", "123")));
        }

        /// <summary>
        ///A test for BuildShowUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LinqToTwitter.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildShowNoIDUrlTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Dictionary<string, string> parameters =
                new Dictionary<string, string>
                {
                    { "Type", SavedSearchType.Show.ToString() }
                };
            string expected = "http://twitter.com/saved_searches/show/123.xml";
            string actual = target.BuildURL(parameters);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BuildShowUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LinqToTwitter.dll")]
        public void BuildShowUrlTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Dictionary<string, string> parameters =
                new Dictionary<string, string>
                {
                    { "Type", SavedSearchType.Show.ToString() },
                    { "ID", "123" }
                };
            string expected = "http://twitter.com/saved_searches/show/123.xml";
            string actual = target.BuildURL(parameters);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BuildSearchesUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LinqToTwitter.dll")]
        public void BuildSearchesUrlTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Dictionary<string, string> parameters =
                new Dictionary<string, string>
                {
                    { "Type", SavedSearchType.Searches.ToString() }
                };
            string expected = "http://twitter.com/saved_searches.xml";
            string actual = target.BuildURL(parameters);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for missing type
        ///</summary>
        [TestMethod()]
        public void MissingTypeTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Dictionary<string, string> parameters = new Dictionary<string, string> { };
            string actual;
            try
            {
                actual = target.BuildURL(parameters);
                Assert.Fail("Expected ArgumentException.");
            }
            catch (ArgumentException ae)
            {
                Assert.AreEqual<string>("Type", ae.ParamName);
            }
        }

        /// <summary>
        ///A test for null parameters
        ///</summary>
        [TestMethod()]
        public void NullParametersTest()
        {
            SavedSearchRequestProcessor<SavedSearch> target = new SavedSearchRequestProcessor<SavedSearch>() { BaseUrl = "http://twitter.com/" };
            Dictionary<string, string> parameters = null;
            string actual;
            try
            {
                actual = target.BuildURL(parameters);
                Assert.Fail("Expected ArgumentException.");
            }
            catch (ArgumentException ae)
            {
                Assert.AreEqual<string>("Type", ae.ParamName);
            }
        }
    }
}
