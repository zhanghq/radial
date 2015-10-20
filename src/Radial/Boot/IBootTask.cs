namespace Radial.Boot
{
    /// <summary>
    /// Boot task interface.
    /// </summary>
    public interface IBootTask
    {
        /// <summary>
        /// System initialize process.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Start system.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop system.
        /// </summary>
        void Stop();
    }
}
