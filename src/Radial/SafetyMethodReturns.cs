using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    public struct SafetyMethodReturns
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    public struct SafetyMethodReturns<T1>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    public struct SafetyMethodReturns<T1, T2>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    /// <typeparam name="T4">The type of the Value 4.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3, T4>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3, T4> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    /// <typeparam name="T4">The type of the Value 4.</typeparam>
    /// <typeparam name="T5">The type of the Value 5.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3, T4, T5> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    /// <typeparam name="T4">The type of the Value 4.</typeparam>
    /// <typeparam name="T5">The type of the Value 5.</typeparam>
    /// <typeparam name="T6">The type of the Value 6.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3, T4, T5, T6> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    /// <typeparam name="T4">The type of the Value 4.</typeparam>
    /// <typeparam name="T5">The type of the Value 5.</typeparam>
    /// <typeparam name="T6">The type of the Value 6.</typeparam>
    /// <typeparam name="T7">The type of the Value 7.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3, T4, T5, T6, T7> OtherValues { get; set; }
    }

    /// <summary>
    /// Indicate the return value of a safety method which does not throw any exceptions.
    /// </summary>
    /// <typeparam name="T1">The type of the Value 1.</typeparam>
    /// <typeparam name="T2">The type of the Value 2.</typeparam>
    /// <typeparam name="T3">The type of the Value 3.</typeparam>
    /// <typeparam name="T4">The type of the Value 4.</typeparam>
    /// <typeparam name="T5">The type of the Value 5.</typeparam>
    /// <typeparam name="T6">The type of the Value 6.</typeparam>
    /// <typeparam name="T7">The type of the Value 7.</typeparam>
    /// <typeparam name="T8">The type of the Value 8.</typeparam>
    public struct SafetyMethodReturns<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Gets or sets a value indicating whether is succeed.
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Gets or sets the other return values.
        /// </summary>
        public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> OtherValues { get; set; }
    }
}
