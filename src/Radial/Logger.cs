using System;
using log4net.Config;
using log4net;
using System.IO;


namespace Radial
{
    /// <summary>
    /// Log class.
    /// </summary>
    public sealed class Logger
    {
        ILog _log;
        /// <summary>
        /// whether logger is started
        /// </summary>
        static bool S_IsStart = false;
        static object S_SyncRoot = new object();


        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        private Logger()
        {
            Start();

            _log = LogManager.GetLogger(typeof(Logger));

        }


        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        /// <param name="logName">Name of the logger.</param>
        private Logger(string logName)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(logName), "logger name can not be empty or null.");

            Start();

            _log = LogManager.GetLogger(logName);
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return GlobalVariables.GetConfigPath("log4net.config");
            }
        }

        /// <summary>
        /// Starts the log component.
        /// </summary>
        private static void Start()
        {
            lock (S_SyncRoot)
            {
                if (!S_IsStart)
                {
                    if (File.Exists(ConfigurationPath))
                        XmlConfigurator.ConfigureAndWatch(new FileInfo(ConfigurationPath));
                }
            }
        }

        /// <summary>
        /// Gets the default log instance.
        /// </summary>
        public static Logger Default
        {
            get
            {
                return new Logger();
            }
        }

        /// <summary>
        /// Gets the specified log instance.
        /// </summary>
        /// <param name="logName">The log name.</param>
        /// <returns>log instance.</returns>
        public static Logger GetInstance(string logName)
        {
            return new Logger(logName);
        }

        /// <summary>
        /// Gets the specified log instance.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <returns>log instance.</returns>
        public static Logger GetInstance(Type logType)
        {
            return GetInstance(logType.Name);
        }

        /// <summary>
        /// Gets the specified log instance.
        /// </summary>
        /// <typeparam name="T">the type.</typeparam>
        /// <returns>log instance.</returns>
        public static Logger GetInstance<T>()
        {
            return GetInstance(typeof(T));
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Debug(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public void Debug(Exception exception)
        {
            Debug(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Debug level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Debug(Exception exception, string format, params object[] args)
        {
            _log.Debug(string.Format(format, args), exception);
        }


        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Info(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public void Info(Exception exception)
        {
            Info(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Info level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Info(Exception exception, string format, params object[] args)
        {
            _log.Info(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Warn(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public void Warn(Exception exception)
        {
            Warn(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Warn level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Warn(Exception exception, string format, params object[] args)
        {
            _log.Warn(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Error(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public void Error(Exception exception)
        {
            Error(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Error level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Error(Exception exception, string format, params object[] args)
        {
            _log.Error(string.Format(format, args), exception);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Fatal(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public void Fatal(Exception exception)
        {
            Fatal(exception, string.Empty);
        }

        /// <summary>
        /// Logs a message string with the Fatal level.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public void Fatal(Exception exception, string format, params object[] args)
        {
            _log.Fatal(string.Format(format, args), exception);
        }
    }
}
