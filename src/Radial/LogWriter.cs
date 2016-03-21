using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Log writer abstract class.
    /// </summary>
    public abstract class LogWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public LogWriter(string logName) { }

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Debug(string format, params object[] args);

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public abstract void Debug(Exception exception);

        /// <summary>
        /// Writes a log with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Debug(Exception exception, string format, params object[] args);


        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Info(string format, params object[] args);

        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public abstract void Info(Exception exception);

        /// <summary>
        /// Writes a log with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Info(Exception exception, string format, params object[] args);

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Warn(string format, params object[] args);

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public abstract void Warn(Exception exception);

        /// <summary>
        /// Writes a log with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Warn(Exception exception, string format, params object[] args);

        /// <summary>
        /// Writes a log with the Error level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Error(string format, params object[] args);

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public abstract void Error(Exception exception);

        /// <summary>
        /// Writes a log with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Error(Exception exception, string format, params object[] args);

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Fatal(string format, params object[] args);

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public abstract void Fatal(Exception exception);

        /// <summary>
        /// Writes a log with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public abstract void Fatal(Exception exception, string format, params object[] args);
    }
}
