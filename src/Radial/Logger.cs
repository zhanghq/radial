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
        /// The default log name
        /// </summary>
        public const string DefaultName = "Default";

        /// <summary>
        /// Gets the default log instance.
        /// </summary>
        public static LogWriter Default
        {
            get
            {
                return New(DefaultName);
            }
        }

        /// <summary>
        /// Create a new instance with the specified log name.
        /// </summary>
        /// <param name="logName">The log name.</param>
        /// <returns>log instance.</returns>
        public static LogWriter New(string logName)
        {
            if (string.IsNullOrWhiteSpace(logName))
                logName = DefaultName;

            logName = logName.Trim();

            var instanceLogName = string.Format("__LogName_{0}__", logName);

            if (Dependency.Container.IsRegistered<LogWriter>(instanceLogName))
                return Dependency.Container.Resolve<LogWriter>(instanceLogName);

            if (Dependency.Container.IsRegistered<LogWriter>())
            {
                var instance = Dependency.Container.Resolve<LogWriter>(new ParameterOverride("logName", logName));

                if (instance != null)
                    Dependency.Container.RegisterInstance(instanceLogName, instance, new ContainerControlledLifetimeManager());

                return instance;
            }

            return new Log4NetWriter(logName);
        }

        /// <summary>
        /// Create a new instance with the specified log type.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <returns>log instance.</returns>
        public static LogWriter New(Type logType)
        {
            return New(logType.Name);
        }

        /// <summary>
        /// Create a new instance with the specified log type.
        /// </summary>
        /// <typeparam name="T">the type.</typeparam>
        /// <returns>log instance.</returns>
        public static LogWriter New<T>()
        {
            return New(typeof(T));
        }
    }
}
