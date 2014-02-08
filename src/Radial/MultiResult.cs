using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    public struct MultiResult<T1, T2>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1",Order=1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    public struct MultiResult<T1, T2, T3>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    /// <typeparam name="T4">The type of result value 4.</typeparam>
    public struct MultiResult<T1, T2, T3, T4>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
        /// <summary>
        /// Gets or sets the result value 4.
        /// </summary>
        [JsonProperty("value4", Order = 4)]
        public T4 Value4 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    /// <typeparam name="T4">The type of result value 4.</typeparam>
    /// <typeparam name="T5">The type of result value 5.</typeparam>
    public struct MultiResult<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
        /// <summary>
        /// Gets or sets the result value 4.
        /// </summary>
        [JsonProperty("value4", Order = 4)]
        public T4 Value4 { get; set; }
        /// <summary>
        /// Gets or sets the result value 5.
        /// </summary>
        [JsonProperty("value5", Order = 5)]
        public T5 Value5 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    /// <typeparam name="T4">The type of result value 4.</typeparam>
    /// <typeparam name="T5">The type of result value 5.</typeparam>
    /// <typeparam name="T6">The type of result value 6.</typeparam>
    public struct MultiResult<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
        /// <summary>
        /// Gets or sets the result value 4.
        /// </summary>
        [JsonProperty("value4", Order = 4)]
        public T4 Value4 { get; set; }
        /// <summary>
        /// Gets or sets the result value 5.
        /// </summary>
        [JsonProperty("value5", Order = 5)]
        public T5 Value5 { get; set; }
        /// <summary>
        /// Gets or sets the result value 6.
        /// </summary>
        [JsonProperty("value6", Order = 6)]
        public T6 Value6 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    /// <typeparam name="T4">The type of result value 4.</typeparam>
    /// <typeparam name="T5">The type of result value 5.</typeparam>
    /// <typeparam name="T6">The type of result value 6.</typeparam>
    /// <typeparam name="T7">The type of result value 7.</typeparam>
    public struct MultiResult<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
        /// <summary>
        /// Gets or sets the result value 4.
        /// </summary>
        [JsonProperty("value4", Order = 4)]
        public T4 Value4 { get; set; }
        /// <summary>
        /// Gets or sets the result value 5.
        /// </summary>
        [JsonProperty("value5", Order = 5)]
        public T5 Value5 { get; set; }
        /// <summary>
        /// Gets or sets the result value 6.
        /// </summary>
        [JsonProperty("value6", Order = 6)]
        public T6 Value6 { get; set; }
        /// <summary>
        /// Gets or sets the result value 7.
        /// </summary>
        [JsonProperty("value7", Order = 7)]
        public T7 Value7 { get; set; }
    }

    /// <summary>
    /// Indicate the multiple result values.
    /// </summary>
    /// <typeparam name="T1">The type of result value 1.</typeparam>
    /// <typeparam name="T2">The type of result value 2.</typeparam>
    /// <typeparam name="T3">The type of result value 3.</typeparam>
    /// <typeparam name="T4">The type of result value 4.</typeparam>
    /// <typeparam name="T5">The type of result value 5.</typeparam>
    /// <typeparam name="T6">The type of result value 6.</typeparam>
    /// <typeparam name="T7">The type of result value 7.</typeparam>
    /// <typeparam name="T8">The type of result value 8.</typeparam>
    public struct MultiResult<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Gets or sets the result value 1.
        /// </summary>
        [JsonProperty("value1", Order = 1)]
        public T1 Value1 { get; set; }
        /// <summary>
        /// Gets or sets the result value 2.
        /// </summary>
        [JsonProperty("value2", Order = 2)]
        public T2 Value2 { get; set; }
        /// <summary>
        /// Gets or sets the result value 3.
        /// </summary>
        [JsonProperty("value3", Order = 3)]
        public T3 Value3 { get; set; }
        /// <summary>
        /// Gets or sets the result value 4.
        /// </summary>
        [JsonProperty("value4", Order = 4)]
        public T4 Value4 { get; set; }
        /// <summary>
        /// Gets or sets the result value 5.
        /// </summary>
        [JsonProperty("value5", Order = 5)]
        public T5 Value5 { get; set; }
        /// <summary>
        /// Gets or sets the result value 6.
        /// </summary>
        [JsonProperty("value6", Order = 6)]
        public T6 Value6 { get; set; }
        /// <summary>
        /// Gets or sets the result value 7.
        /// </summary>
        [JsonProperty("value7", Order = 7)]
        public T7 Value7 { get; set; }
        /// <summary>
        /// Gets or sets the result value 8.
        /// </summary>
        [JsonProperty("value8", Order = 8)]
        public T8 Value8 { get; set; }
    }
}
