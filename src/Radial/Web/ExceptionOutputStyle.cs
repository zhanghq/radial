namespace Radial.Web
{
    /// <summary>
    /// Exception message output style.
    /// </summary>
    public enum ExceptionOutputStyle
    {
        /// <summary>
        /// Not set the output style, use system default.
        /// </summary>
        System,
        /// <summary>
        /// Wraps the exception to XML output format.
        /// </summary>
        Xml,
        /// <summary>
        /// Wraps the exception to JSON output format.
        /// </summary>
        Json
    }
}
