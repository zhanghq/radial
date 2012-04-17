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
        /// Initializes a new instance of the <see cref="AcquireFailedEventArgs"/> class.
        /// </summary>
        public AcquireFailedEventArgs()
        {
            CancelRetry = true;
        }

        /// <summary>
        /// Gets the output logger.
        /// </summary>
        public Logger OutputLogger { get; internal set; }

        /// <summary>
        /// Gets the lock key.
        /// </summary>
        public string LockKey { get; internal set; }

        /// <summary>
        /// Gets the retry times.
        /// </summary>
        public int RetryTimes { get; internal set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to cancel retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if need to cancel; otherwise, <c>false</c>.
        /// </value>
        public bool CancelRetry { get; set; }
    }
}
