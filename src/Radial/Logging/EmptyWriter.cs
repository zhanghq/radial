using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Logging
{
    /// <summary>
    /// EmptyWriter
    /// </summary>
    /// <seealso cref="Radial.LogWriter" />
    class EmptyWriter : LogWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyWriter"/> class.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        public EmptyWriter(string name) : base(name)
        {
        }

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Debug(string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Debug(Exception exception) { }

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Debug(Exception exception, string format, params object[] args) { }


        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Info(string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Info(Exception exception) { }

        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Info(Exception exception, string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Warn(string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Warn(Exception exception) { }

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Warn(Exception exception, string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Error level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Error(string format, params object[] args) { }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Error(Exception exception) { }

        /// <summary>
        /// Writes a log with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Error(Exception exception, string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Fatal(string format, params object[] args) { }

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Fatal(Exception exception) { }

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Fatal(Exception exception, string format, params object[] args) { }
    }
}
