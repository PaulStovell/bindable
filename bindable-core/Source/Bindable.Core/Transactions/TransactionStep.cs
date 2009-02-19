using System;
using System.Transactions;

namespace Bindable.Core.Transactions
{
    /// <summary>
    /// Used internally as an object to enlist within a <see cref="TransactionScope"/> to perform commit/rollback actions.
    /// </summary>
    internal class TransactionStep : IEnlistmentNotification
    {
        private readonly object _sharedLock;

        public TransactionStep()
            : this(new object())
        {
        }

        public TransactionStep(object sharedLock)
        {
            _sharedLock = sharedLock;
        }

        public Action CommitAction { get; set; }
        public Action RollbackAction { get; set; }
        public Action PrepareCommitAction { get; set; }
        public Action InDoubtAction { get; set; }

        public void Commit(Enlistment enlistment)
        {
            lock (_sharedLock)
            {
                if (CommitAction != null) CommitAction();
            }
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            lock (_sharedLock)
            {
                if (InDoubtAction != null) InDoubtAction();
            }
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            lock (_sharedLock)
            {
                if (PrepareCommitAction != null) PrepareCommitAction();
            }
            preparingEnlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            lock (_sharedLock)
            {
                if (RollbackAction != null) RollbackAction();
            }
            enlistment.Done();
        }
    }
}