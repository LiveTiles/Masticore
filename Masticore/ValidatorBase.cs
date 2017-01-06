using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masticore
{

    /// <summary>
    /// The abstract class definitions for validation
    /// </summary>
    /// <source>http://stackoverflow.com/a/5087990</source>
    /// <typeparam name="T">A Generic Type</typeparam>
    public abstract class ValidatorBase<T>
    {
        public class Rule
        {
            public Func<T, TestResult> Test { get; set; }

            public string GenericError { get; set; }
        }

        //Test results
        public class TestResult
        {
            public bool HasPassed { get; set; }

            public string ErrorMessage { get; set; }
        }

     

        protected abstract IEnumerable<Rule> Rules { get; }

        /// <summary>
        /// Runs through the defined rules and, if they do not pass, returns a set of errors
        /// </summary>
        /// <param name="t">The generic type</param>
        /// <returns>An IEnumerable of type string</returns>
        public Task<IEnumerable<string>> Validate(T t)
        {
            return Task.FromResult(this.Rules.Where(rule => !rule.Test(t).HasPassed).Select(rule => rule.Test(t).ErrorMessage));
        }
    }
    /// <summary>
    /// The abstract class definitions for validation on a single type
    /// </summary>
    /// <typeparam name="T1">A generic type for the first object to compare</typeparam>
    /// <typeparam name="T2">A generic type for the first object to compare</typeparam>
    public abstract class ValidatorBase<T1, T2>
    {
        public class Rule
        {
            public Func<T1, T2, TestResult> Test { get; set; }

           public string GenericError { get; set; }
        }

        public class TestResult
        {
            public bool HasPassed { get; set; }

            public string ErrorMessage { get; set; }
        }

        protected abstract IEnumerable<Rule> Rules { get; }

        /// <summary>
        /// Tests all the defined rules, and then returns the associated error messages.
        /// </summary>
        /// <param name="t1">A parameter of generic type T1 as defined in <see cref="ValidatorBase{T1, T2}"/></param>
        /// <param name="t2">A parameter of generic type T2 as defined in <see cref="ValidatorBase{T1, T2}"/></param>
        /// <returns>The error messages, (if anything failed validation), as an IEnumerable.</returns>
        public Task<IEnumerable<string>> Validate(T1 t1, T2 t2)
        {

            return Task.FromResult(this.Rules.Select(r => r.Test(t1, t2)).Where(t => !t.HasPassed).Select(t => t.ErrorMessage));
        }
    }

}

