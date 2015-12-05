using System.Data;

namespace Radial.Persist
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkEssential
    {
        /// <summary>
        /// Prepares the transaction, typically this method is invoked implicitly, 
       ///  but it  also can be explicit used to implement custom control.
        /// </summary>
        /// <param name="level">The isolation level.</param>
        void PrepareTransaction(IsolationLevel? level = null);
        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        void Commit();

    }
}
