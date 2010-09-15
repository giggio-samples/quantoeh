﻿/***********************************************************
 * Credits:
 * 
 * MSDN Documentation -
 * Walkthrough: Creating an IQueryable LINQ Provider
 * 
 * http://msdn.microsoft.com/en-us/library/bb546158.aspx
 * 
 * Matt Warren's Blog -
 * LINQ: Building an IQueryable Provider:
 * 
 * http://blogs.msdn.com/mattwar/default.aspx
 * 
 * Modified By: Joe Mayo 8/26/08
 * 
 * - Added Context property
 * - Changed Execute to delegate to TwitterContext through Context property
 * *********************************************************/

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToTwitter
{
    /// <summary>
    /// implementation of IQueryProvider, part of the LINQ Provider API
    /// </summary>
    [Serializable]
    public class TwitterQueryProvider : IQueryProvider
    {
        /// <summary>
        /// refers to TwitterContext that calling code instantiated
        /// </summary>
        public TwitterContext Context { get; set; }

        /// <summary>
        /// Non-generic version, returns current query to 
        /// calling code as its constructing the query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns>IQueryable that can be executed</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(
                    typeof(TwitterQueryable<>)
                        .MakeGenericType(elementType), 
                    new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        /// <summary>
        /// generic version, returns current query to 
        /// calling code as its constructing the query
        /// </summary>
        /// <typeparam name="TResult">current object type being worked with</typeparam>
        /// <param name="expression">expression tree for query</param>
        /// <returns>IQueryable that can be executed</returns>
        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new TwitterQueryable<TResult>(this, expression);
        }

        /// <summary>
        /// non-generic execute, delegates execution to TwitterContext
        /// </summary>
        /// <param name="expression">Expression Tree</param>
        /// <returns>list of results from query</returns>
        public object Execute(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);

            return GetType()
                .GetMethod("Execute", new Type[] { elementType })
                .Invoke(this, new object[] { expression });
        }

        /// <summary>
        /// generic execute, delegates execution to TwitterContext
        /// </summary>
        /// <typeparam name="TResult">type of query</typeparam>
        /// <param name="expression">Expression tree</param>
        /// <returns>list of results from query</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = 
                typeof(TResult).Name == "IEnumerable`1" ||
                typeof(TResult).Name == "IEnumerable";

            // generic parameter type for method call
            Type resultType = 
                TypeSystem.GetElementType(
                    (expression as MethodCallExpression).Arguments[0].Type);
            Type[] genericArguments = new Type[] { resultType };

            // generic method instance via reflection
            var methodInfo = Context.GetType().GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArguments);

            // use reflection to execute the generic method with the proper arguments
            //  Note: look at ProcessResults method in PostProcessor and you'll see what is being executed
            return (TResult)genericMethodInfo.Invoke(Context, new object[] { expression, isEnumerable });

            //return (TResult)Context.Execute<TResult>(expression, isEnumerable);
        }
    }
}