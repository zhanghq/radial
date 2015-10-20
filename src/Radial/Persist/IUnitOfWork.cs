namespace Radial.Persist
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkEssential
    {
        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        void Commit();

    }
}
