using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Log4Net log writer.
    /// </summary>
    public sealed class Log4NetWriter : LogWriter
    {
        ILog _log;
        static bool S_IsStart = false;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetWriter" /> class.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public Log4NetWriter(string logName) : base(logName)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(logName), "log name can not be empty or null.");

            if (!S_IsStart)
            {
                lock (S_SyncRoot)
                {
                    if (!S_IsStart)
                    {
                        if (File.Exists(ConfigurationPath))
                        {
                            XmlConfigurator.ConfigureAndWatch(new FileInfo(ConfigurationPath));
                            S_IsStart = true;
                        }
                    }
                }
            }

            _log =LogManager.GetLogger(logName);
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        public string ConfigurationPath
        {
            get
            {
                return GlobalVariables.GetConfigPath("log4net.config");
            }
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Debug(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Debug(Exception exception)
        {
            Debug(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Debug(Exception exception, string format, params object[] args)
        {
            _log.Debug(string.Format(format, args), exception);
        }


        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Info(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Info(Exception exception)
        {
            Info(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Info(Exception exception, string format, params object[] args)
        {
            _log.Info(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Warn(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Warn(Exception exception)
        {
            Warn(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Warn(Exception exception, string format, params object[] args)
        {
            _log.Warn(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Error(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Error(Exception exception)
        {
            Error(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Error(Exception exception, string format, params object[] args)
        {
            _log.Error(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Fatal(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public override void Fatal(Exception exception)
        {
            Fatal(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Fatal(Exception exception, string format, params object[] args)
        {
            _log.Fatal(string.Format(format, args), exception);
        }
    }
}
