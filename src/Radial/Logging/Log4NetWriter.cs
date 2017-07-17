using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Logging
{
    /// <summary>
    /// Log4NetWriter
    /// </summary>
    /// <seealso cref="Radial.LogWriter" />
    public sealed class Log4NetWriter : LogWriter
    {
        ILog _logger;
        static bool S_IsStart = false;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        /// <param name="cfgFilePath">The config file path, if equal to null will use default location.</param>
        public static void Prepare(string cfgFilePath=null)
        {
            if (!S_IsStart)
            {
                lock (S_SyncRoot)
                {
                    if (!S_IsStart)
                    {
                        if (string.IsNullOrWhiteSpace(cfgFilePath))
                            cfgFilePath = GlobalVariables.GetConfigPath("log4net.config");

                        XmlConfigurator.ConfigureAndWatch(new FileInfo(cfgFilePath));
                        S_IsStart = true;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetWriter" /> class.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        public Log4NetWriter(string name) : base(name)
        {
            Prepare();

            _logger =LogManager.GetLogger(Name);
        }


        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Debug(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
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
            _logger.Debug(string.Format(format, args), exception);
        }


        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Info(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
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
            _logger.Info(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Warn(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
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
            _logger.Warn(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Error(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
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
            _logger.Error(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public override void Fatal(string format, params object[] args)
        {
            _logger.FatalFormat(format, args);
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
            _logger.Fatal(string.Format(format, args), exception);
        }
    }
}
