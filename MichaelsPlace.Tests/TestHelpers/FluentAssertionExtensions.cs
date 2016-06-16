using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions.Execution;
using FluentAssertions.Mvc;

namespace MichaelsPlace.Tests.TestHelpers
{
    public static class FluentAssertionExtensions
    {
        /// <summary>
        /// Asserts that the subject is a <see cref="T:System.Web.Mvc.ContentResult" />.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="!:reason" />.
        /// </param>
        public static ContentResultAssertions BeHttpStatusResult(this ActionResultAssertions @this, HttpStatusCode statusCode, string reason = null, params object[] reasonArgs)
        {
            Execute.Assertion.BecauseOf(reason, reasonArgs).ForCondition(@this.Subject is HttpStatusCodeResult)
                .FailWith("Expected ActionResult to be {0}{reason}, but found {1}", (object)typeof(ContentResult).Name, (object)@this.Subject.GetType().Name);

            Execute.Assertion.BecauseOf(reason, reasonArgs).ForCondition(((HttpStatusCodeResult)@this.Subject).StatusCode == (int)statusCode)
                .FailWith("Expected status code to be {0}{reason}, but found {1}", statusCode, (HttpStatusCode)((HttpStatusCodeResult)@this.Subject).StatusCode);

            return new ContentResultAssertions(@this.Subject as ContentResult);
        }

    }
}
