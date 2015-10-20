﻿using System;

namespace Radial
{
    /// <summary>
    /// Tools for check whether a condition is valid.
    /// </summary>
    public static class Checker
    {
        /// <summary>
        /// Check whether the condition is true, otherwise throw an Exception.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public static void Requires(bool condition, string messageFormat, params object[] args)
        {
            if (!condition)
                throw new Exception(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Check whether the condition is true, otherwise throw an ArgumentException.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public static void Parameter(bool condition, string messageFormat, params object[] args)
        {
            if (!condition)
                throw new ArgumentException(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Check whether the condition is true, otherwise throw an ArgumentException.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public static void Parameter(bool condition, string paramName, string messageFormat, params object[] args)
        {
            if (!condition)
                throw new ArgumentException(string.Format(messageFormat, args), paramName);
        }
    }
}
