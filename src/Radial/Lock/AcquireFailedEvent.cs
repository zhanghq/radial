using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Lock
{
    /// <summary>
    /// Acquire failed event handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="Radial.Lock.AcquireFailedEventArgs"/> instance containing the event data.</param>
    public delegate void AcquireFailedEventHandler(object sender, AcquireFailedEventArgs args);

    /// <summary>
    /// Lock acquire failed event args.
    /// </summary>
    public sealed class AcquireFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the lock key.
        /// </summary>
        public string LockKey { get; internal set; }

        /// <summary>
        /// Gets the current retry times.
        /// </summary>
        public int RetryTimes { get; internal set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to keep on retrying.
        /// </summary>
        /// <value>
        ///   <c>true</c> if need to keep on; otherwise, <c>false</c>.
        /// </value>
        public bool KeepOnRetry { get; set; }
    }
}
