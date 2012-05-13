using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Radial
{
    /// <summary>
    /// Auto TransactionScope helper class.
    /// </summary>
    public static class AutoTransaction
    {
        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void Complete(Action action)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="transactionToUse">The transaction to be set as the ambient transaction, so that transactional work done inside the scope uses this transaction.</param>
        public static void Complete(Action action, Transaction transactionToUse)
        {
            using (TransactionScope scope = new TransactionScope(transactionToUse))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="scopeOption"> An instance of the System.Transactions.TransactionScopeOption enumeration that describes the transaction requirements associated with this transaction scope.</param>
        public static void Complete(Action action, TransactionScopeOption scopeOption)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="transactionToUse">The transaction to be set as the ambient transaction, so that transactional work done inside the scope uses this transaction.</param>
        /// <param name="scopeTimeout">The System.TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        public static void Complete(Action action, Transaction transactionToUse, TimeSpan scopeTimeout)
        {
            using (TransactionScope scope = new TransactionScope(transactionToUse, scopeTimeout))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="scopeOption">An instance of the System.Transactions.TransactionScopeOption enumeration that describes the transaction requirements associated with this transaction scope.</param>
        /// <param name="scopeTimeout">The System.TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        public static void Complete(Action action, TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption, scopeTimeout))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="scopeOption">An instance of the System.Transactions.TransactionScopeOption enumeration that describes the transaction requirements associated with this transaction scope.</param>
        /// <param name="transactionOptions"> A System.Transactions.TransactionOptions structure that describes the transaction options to use if a new transaction is created.If an existing transaction  is used, the timeout value in this parameter applies to the transaction scope.If that time expires before the scope is disposed, the transaction is aborted.</param>
        public static void Complete(Action action, TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption, transactionOptions))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="transactionToUse">The transaction to be set as the ambient transaction, so that transactional work done inside the scope uses this transaction.</param>
        /// <param name="scopeTimeout">The System.TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        /// <param name="interopOption">An instance of the System.Transactions.EnterpriseServicesInteropOption enumeration that describes how the associated transaction interacts with COM+ transactions.</param>
        public static void Complete(Action action, Transaction transactionToUse, TimeSpan scopeTimeout, EnterpriseServicesInteropOption interopOption)
        {
            using (TransactionScope scope = new TransactionScope(transactionToUse, scopeTimeout, interopOption))
            {
                action();
                scope.Complete();
            }
        }

        /// <summary>
        /// Automatic create a new TransactionScope object, and complete it after action invokes.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="scopeOption">An instance of the System.Transactions.TransactionScopeOption enumeration that describes the transaction requirements associated with this transaction scope.</param>
        /// <param name="transactionOptions">A System.Transactions.TransactionOptions structure that describes the transaction options to use if a new transaction is created.If an existing transaction is used, the timeout value in this parameter applies to the transaction scope.If that time expires before the scope is disposed, the transaction is aborted.</param>
        /// <param name="interopOption">An instance of the System.Transactions.EnterpriseServicesInteropOption enumeration that describes how the associated transaction interacts with COM+ transactions.</param>
        public static void Complete(Action action, TransactionScopeOption scopeOption, TransactionOptions transactionOptions, EnterpriseServicesInteropOption interopOption)
        {
            using (TransactionScope scope = new TransactionScope(scopeOption, transactionOptions, interopOption))
            {
                action();
                scope.Complete();
            }
        }
    }
}
