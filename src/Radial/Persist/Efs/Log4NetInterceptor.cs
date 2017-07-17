using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using Microsoft.Practices.Unity;
using System.Data.Entity;

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
            {
                if (p != null && (p.Direction == System.Data.ParameterDirection.Input || p.Direction == System.Data.ParameterDirection.InputOutput))
                {
                    object pValue = null;

                    switch (p.DbType)
                    {
                        case System.Data.DbType.AnsiString:
                        case System.Data.DbType.AnsiStringFixedLength:
                        case System.Data.DbType.Date:
                        case System.Data.DbType.DateTime:
                        case System.Data.DbType.DateTime2:
                        case System.Data.DbType.DateTimeOffset:
                        case System.Data.DbType.Guid:
                        case System.Data.DbType.String:
                        case System.Data.DbType.StringFixedLength:
                        case System.Data.DbType.Time:
                        case System.Data.DbType.Xml:
                            pValue = string.Format("'{0}'", p.Value); break;
                        default: pValue = p.Value; break;
                    }

                    ps.Add(string.Format("{0}={1} [Type={2}, Size={3}]", p.ParameterName, pValue, p.DbType, p.Size));
                }
            }

            string txt = string.Format("{0} -- {1}", command.CommandText.Replace(Environment.NewLine, ""),
                string.Join("; ", ps.ToArray())).Trim('-', ' ');

            if (interceptionContext.OriginalException != null)
                Logger.Get<Log4NetInterceptor>().Error(interceptionContext.OriginalException, txt);
            else
                Logger.Get<Log4NetInterceptor>().Debug(txt);
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
