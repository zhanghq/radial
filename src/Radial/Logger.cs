using System;
using Microsoft.Practices.Unity;

namespace Radial
{
    /// <summary>
    /// The entrance static class of logging.
    /// </summary>
    public sealed class Logger
    {
        /// <summary>
        /// Retrieve or create a specified logger writer.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        /// <returns>logger writer instance.</returns>
        public static LogWriter Get(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "logger name can not be empty or null.");

            name = name.Trim();

            var instanceLoggerName = string.Format("__LogWriter_{0}__", name);

            if (Dependency.Container.IsRegistered<LogWriter>(instanceLoggerName))
                return Dependency.Container.Resolve<LogWriter>(instanceLoggerName);

            LogWriter instance = null;

            if (Dependency.Container.IsRegistered<LogWriter>())
                instance = Dependency.Container.Resolve<LogWriter>(new ParameterOverride("name", name));

            if (instance != null)
                Dependency.Container.RegisterInstance(instanceLoggerName, instance, new ContainerControlledLifetimeManager());
            else
                instance = new Logging.EmptyWriter(name);

            return instance;
        }

        /// <summary>
        /// Retrieve or create a specified logger writer.
        /// </summary>
        /// <param name="type">Type of the logger.</param>
        /// <returns>
        /// logger writer instance.
        /// </returns>
        public static LogWriter Get(Type type)
        {
            Checker.Parameter(type != null, "logger type can not be  null.");

            return Get(type.FullName);
        }

        /// <summary>
        /// Retrieve or create a specified logger writer.
        /// </summary>
        /// <typeparam name="T">Type of the logger.</typeparam>
        /// <returns>logger writer instance.</returns>
        public static LogWriter Get<T>()
        {
            return Get(typeof(T));
        }


        /// <summary>
        /// Writes a log with the Debug level using default logger name.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Debug(string format, params object[] args)
        {
            Get<Logger>().Debug(format, args);
        }

        /// <summary>
        /// Writes a log with the Debug level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public static void Debug(Exception exception)
        {
            Get<Logger>().Debug(exception);
        }

        /// <summary>
        /// Writes a log with the Debug level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Debug(Exception exception, string format, params object[] args)
        {
            Get<Logger>().Debug(exception, format, args);
        }


        /// <summary>
        /// Writes a log with the Info level using default logger name.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Info(string format, params object[] args)
        {
            Get<Logger>().Info(format, args);
        }

        /// <summary>
        /// Writes a log with the Info level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public static void Info(Exception exception)
        {
            Get<Logger>().Info(exception);
        }

        /// <summary>
        /// Writes a log with the Info level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Info(Exception exception, string format, params object[] args)
        {
            Get<Logger>().Info(exception, format, args);
        }

        /// <summary>
        /// Writes a log with the Warn level using default logger name.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Warn(string format, params object[] args)
        {
            Get<Logger>().Warn(format, args);
        }

        /// <summary>
        /// Writes a log with the Warn level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public static void Warn(Exception exception)
        {
            Get<Logger>().Warn(exception);
        }

        /// <summary>
        /// Writes a log with the Warn level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Warn(Exception exception, string format, params object[] args)
        {
            Get<Logger>().Warn(exception, format, args);
        }

        /// <summary>
        /// Writes a log with the Error level using default logger name.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Error(string format, params object[] args)
        {
            Get<Logger>().Error(format, args);
        }

        /// <summary>
        /// Logs a message string with the Error level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public static void Error(Exception exception)
        {
            Get<Logger>().Error(exception);
        }

        /// <summary>
        /// Writes a log with the Error level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Error(Exception exception, string format, params object[] args)
        {
            Get<Logger>().Error(exception, format, args);
        }

        /// <summary>
        /// Writes a log with the Fatal level using default logger name.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Fatal(string format, params object[] args)
        {
            Get<Logger>().Fatal(format, args);
        }

        /// <summary>
        /// Writes a log with the Fatal level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        public static void Fatal(Exception exception)
        {
            Get<Logger>().Fatal(exception);
        }

        /// <summary>
        /// Writes a log with the Fatal level using default logger name.
        /// </summary>
        /// <param name="exception">The exception obj.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The message arguments.</param>
        public static void Fatal(Exception exception, string format, params object[] args)
        {
            Get<Logger>().Fatal(exception, format, args);
        }

    }
}
