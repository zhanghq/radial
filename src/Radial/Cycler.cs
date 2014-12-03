using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radial
{
    /// <summary>
    /// Execute function cyclically.
    /// </summary>
    public static class Cycler
    {
        /// <summary>
        /// Execute function cyclically .
        /// </summary>
        /// <param name="predicateFunc">The predicate function, return true to circulate this function.</param>
        /// <param name="waitMilliseconds">The loop wait milliseconds.</param>
        /// <param name="maxLoops">The max loop times before function return true or stop until function return true if set to null.</param>
        public static void Execute(Func<bool> predicateFunc, int waitMilliseconds, int? maxLoops = null)
        {
            Execute(predicateFunc, waitMilliseconds, maxLoops, null);
        }

        /// <summary>
        /// Execute function cyclically .
        /// </summary>
        /// <param name="predicateFunc">The predicate function, return true to circulate this function.</param>
        /// <param name="waitMilliseconds">The loop wait milliseconds.</param>
        /// <param name="maxLoops">The max loop times before function return true or stop until function return true if set to null.</param>
        /// <param name="logger">The user specified log instance, if not set value, will use Logger.Default as default.</param>
        public static void Execute(Func<bool> predicateFunc, int waitMilliseconds, int? maxLoops, Logger logger)
        {
            if (predicateFunc == null)
                return;

            if (maxLoops.HasValue && maxLoops < 0)
                maxLoops = 0;

            if (waitMilliseconds < 0)
                waitMilliseconds = 0;

            if (logger == null)
                logger = Logger.Default;

            string cycleId = Guid.NewGuid().ToString("N");

            int nextLoop = 0;

            try
            {
                do
                {
                    if (nextLoop > 0)
                        logger.Debug("[cycle: {0} loop: {1}] cycle start", cycleId, nextLoop);

                    if (!predicateFunc())
                    {
                        logger.Debug("[cycle: {0} loop: {1}] cycle exited due to function return false", cycleId, nextLoop);
                        break;
                    }

                    nextLoop++;

                    if (maxLoops.HasValue && nextLoop > maxLoops)
                    {
                        logger.Debug("[cycle: {0} loop: {1}] cycle completed", cycleId, maxLoops);
                        break;
                    }

                    logger.Debug("[cycle: {0} next loop: {1}] execute again after waiting {2} ms", cycleId, nextLoop, waitMilliseconds);
                    Thread.Sleep(waitMilliseconds);
                }
                while (true);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[cycle: {0} loop: {1}] cycle terminated", cycleId, nextLoop);
            }
        }

        /// <summary>
        /// Execute function asynchronous cyclically .
        /// </summary>
        /// <param name="predicateFunc">The predicate function, return true to circulate this function.</param>
        /// <param name="waitMilliseconds">The loop wait milliseconds.</param>
        /// <param name="maxLoops">The max loop times before function return true or stop until function return true if set to null.</param>
        public static void ExecuteAsync(Func<bool> predicateFunc, int waitMilliseconds, int? maxLoops = null)
        {
            ExecuteAsync(predicateFunc, waitMilliseconds, maxLoops, null);
        }

        /// <summary>
        /// Execute function asynchronous cyclically .
        /// </summary>
        /// <param name="predicateFunc">The predicate function, return true to circulate this function.</param>
        /// <param name="waitMilliseconds">The loop wait milliseconds.</param>
        /// <param name="maxLoops">The max loop times before function return true or stop until function return true if set to null.</param>
        /// <param name="logger">The user specified log instance, if not set value, will use Logger.Default as default.</param>
        public static void ExecuteAsync(Func<bool> predicateFunc, int waitMilliseconds, int? maxLoops, Logger logger)
        {
            if (predicateFunc == null)
                return;

            if (maxLoops.HasValue && maxLoops < 0)
                maxLoops = 0;

            if (waitMilliseconds < 0)
                waitMilliseconds = 0;

            if (logger == null)
                logger = Logger.Default;

            ThreadPool.QueueUserWorkItem(o =>
            {
                string cycleId = Guid.NewGuid().ToString("N");

                int nextLoop = 0;

                try
                {
                    do
                    {
                        if (nextLoop > 0)
                            logger.Debug("[cycle: {0} loop: {1}] cycle start", cycleId, nextLoop);

                        if (!predicateFunc())
                        {
                            logger.Debug("[cycle: {0} loop: {1}] cycle exited due to function return false", cycleId, nextLoop);
                            break;
                        }

                        nextLoop++;

                        if (maxLoops.HasValue && nextLoop > maxLoops)
                        {
                            logger.Debug("[cycle: {0} loop: {1}] cycle completed", cycleId, maxLoops);
                            break;
                        }

                        logger.Debug("[cycle: {0} next loop: {1}] execute again after waiting {2} ms", cycleId, nextLoop, waitMilliseconds);
                        Thread.Sleep(waitMilliseconds);
                    }
                    while (true);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "[cycle: {0} loop: {1}] cycle terminated", cycleId, nextLoop);
                }
            });
        }
    }
}
