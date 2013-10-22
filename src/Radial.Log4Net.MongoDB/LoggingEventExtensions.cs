using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Log4Net.MongoDB
{
    /// <summary>
    /// LoggingEventExtensions
    /// </summary>
    public static class LoggingEventExtensions
    {
        /// <summary>
        /// Automatics the bson model.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        /// <returns></returns>
        public static BsonLogModel ToBsonModel(this LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
                return null;

            var model = new BsonLogModel();
            model.Time = loggingEvent.TimeStamp;
            model.Level = loggingEvent.Level.ToString();
            model.Logger = loggingEvent.LoggerName;
            model.Machine = Environment.MachineName;

            string msg = loggingEvent.RenderedMessage;

            if (loggingEvent.ExceptionObject != null)
                msg += Environment.NewLine + loggingEvent.ExceptionObject.ToString();

            model.Message = msg.Trim();

            return model;
        }
    }
}
