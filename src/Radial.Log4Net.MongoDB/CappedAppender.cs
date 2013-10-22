using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using log4net.Appender;
using log4net.Core;
using MongoDB.Driver.Builders;

namespace Radial.Log4Net.MongoDB
{
    /// <summary>
    /// Capped collection Appender
    /// </summary>
	public class CappedAppender : AppenderSkeleton
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="CappedAppender"/> class.
        /// </summary>
        /// <remarks>
        /// Empty default constructor
        /// </remarks>
        public CappedAppender()
        {
            ConnectionString = "mongodb://localhost/test";
            CollectionName = "logs";
            StorageSize = 100;
        }

        /// <summary>
        /// Gets or sets the connection string, defaults to mongodb://localhost/test.
        /// </summary>
		public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the collection, defaults to "logs".
        /// </summary>
		public string CollectionName { get; set; }

        /// <summary>
        /// Max collection storage size (MB), defaults to 100.
        /// </summary>
        public int StorageSize { get; set; }

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton" /> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        ///   <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent" />.
        ///   </para>
        ///   <para>This method will be called by <see cref="M:DoAppend(LoggingEvent)" />
        /// if all the conditions listed for that method are met.
        ///   </para>
        ///   <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:PreAppendCheck()" /> method.
        ///   </para>
        /// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			var collection = GetCollection();
			collection.Insert(BuildBsonLogModel(loggingEvent));
		}

        /// <summary>
        /// Append a bulk array of logging events.
        /// </summary>
        /// <param name="loggingEvents">the array of logging events</param>
        /// <remarks>
        ///   <para>
        /// This base class implementation calls the <see cref="M:Append(LoggingEvent)" />
        /// method for each element in the bulk array.
        ///   </para>
        ///   <para>
        /// A sub class that can better process a bulk array of events should
        /// override this method in addition to <see cref="M:Append(LoggingEvent)" />.
        ///   </para>
        /// </remarks>
		protected override void Append(LoggingEvent[] loggingEvents)
		{
			var collection = GetCollection();
			collection.InsertBatch(loggingEvents.Select(BuildBsonLogModel));
		}

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <returns></returns>
        private MongoCollection<BsonLogModel> GetCollection()
        {
            MongoUrl url = MongoUrl.Create(ConnectionString);

            MongoClient client = new MongoClient(url);
            MongoServer server = client.GetServer();
            MongoDatabase db = server.GetDatabase(url.DatabaseName);

            if (!db.CollectionExists(CollectionName))
            {
                CollectionOptionsBuilder options = CollectionOptions.SetCapped(true).SetMaxSize(StorageSize * 1024 * 1024);
                db.CreateCollection(CollectionName, options);
            }
            else
            {
                var collection = db.GetCollection(CollectionName);
                var stats = collection.GetStats();

                if (!stats.IsCapped || stats.StorageSize != StorageSize * 1024 * 1024)
                {
                    CommandDocument cd = new CommandDocument();
                    cd.Add("convertToCapped", CollectionName);
                    cd.Add("size", StorageSize * 1024 * 1024);
                    db.RunCommand(cd);
                }
            }


            return db.GetCollection<BsonLogModel>(CollectionName);
        }


        /// <summary>
        /// Builds the bson log model.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        /// <returns></returns>
        private BsonLogModel BuildBsonLogModel(LoggingEvent loggingEvent)
        {
            return loggingEvent.ToBsonModel();
        }
	}
}