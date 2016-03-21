using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// Log4NetInterceptor
    /// </summary>
    /// <seealso cref="System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" />
    public class Log4NetInterceptor : IDbCommandInterceptor
    {
        /// <summary>
        /// Renders the log.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        private void RenderLog<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            List<string> ps = new List<string>();
            foreach (DbParameter p in command.Parameters)
                ps.Add(string.Format("{0}='{1}' [Type={2}, Size={3}, Direction={4}]", p.ParameterName, p.Value.ToString(),
                    p.DbType.ToString(), p.Size, p.Direction.ToString()));

            string txt = string.Format("{0} -- {1}", command.CommandText.Replace(Environment.NewLine, ""),
                string.Join("; ", ps.ToArray())).Trim('-', ' ');

            LogWriter logger = null;
            foreach (var d in interceptionContext.DbContexts)
            {
                logger = d.GetLogger();
                break;
            }

            if (logger == null)
                return;

            if (interceptionContext.OriginalException != null)
                logger.Fatal(interceptionContext.OriginalException, txt);
            else
                logger.Info(txt);
        }

        /// <summary>
        /// Nons the query executed.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            RenderLog<int>(command, interceptionContext);
        }

        /// <summary>
        /// Nons the query executing.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        /// <summary>
        /// Readers the executed.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            RenderLog<DbDataReader>(command, interceptionContext);
        }

        /// <summary>
        /// Readers the executing.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            
        }

        /// <summary>
        /// Scalars the executed.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            RenderLog<object>(command, interceptionContext);
        }

        /// <summary>
        /// Scalars the executing.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="interceptionContext">The interception context.</param>
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

        }
    }
}
