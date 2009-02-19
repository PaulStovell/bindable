using System;
using System.Collections.Generic;
using System.Transactions;
using Bindable.Core.Collections;

namespace Bindable.Core.Transactions
{
    /// <summary>
    /// A base class for objects that can participate in a <see cref="System.Transactions.TransactionScope" /> transaction.
    /// </summary>
    public abstract class TransactionalObject
    {
        private readonly Dictionary<Tuple<string, string>, TransactionStep> _transactionsDictionary = new Dictionary<Tuple<string, string>, TransactionStep>();
        private readonly Dictionary<string, object> _fieldsDictionary = new Dictionary<string, object>();
        private readonly object _lock = new object();

        /// <summary>
        /// Gets the value of a field, or default(T) if no value has been set.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field, or the default value if the field has not been set.</returns>
        protected T GetField<T>(string name)
        {
            return GetField(name, default(T));
        }

        /// <summary>
        /// Gets the value of a field, or a custom default value if no value has been set.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <param name="defaultValue">The default value if the field has not been set.</param>
        /// <returns>The value of the field, or the default value if the field has not been set.</returns>
        protected T GetField<T>(string name, T defaultValue)
        {
            lock (_lock)
            {
                if (_fieldsDictionary.ContainsKey(name))
                {
                    return (T)_fieldsDictionary[name];
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        protected void SetField<T>(string name, T value)
        {
            var original = GetField<T>(name);
            ExecuteWithRollback(
                "Set" + name,
                () => _fieldsDictionary[name] = value,
                () => _fieldsDictionary[name] = original
                );
        }

        /// <summary>
        /// Executes a given action, with a corresponding rollback action.
        /// </summary>
        /// <param name="doAction">The action to perform.</param>
        /// <param name="undoAction">The undo action to perform when the transaction is rolled back.</param>
        protected void ExecuteWithRollback(Action doAction, Action undoAction)
        {
            ExecuteWithRollback(null, doAction, undoAction);
        }

        /// <summary>
        /// Executes a given action, with a corresponding rollback action. A unique name is given to check whether 
        /// an earlier rollback action is already registered for this action, in which case the rollback action provided will not be 
        /// registered.
        /// </summary>
        /// <param name="reusableActionName">A name used to check whether a rollback action is already registered.</param>
        /// <param name="doAction">The action to perform.</param>
        /// <param name="undoAction">The undo action to perform when the transaction is rolled back.</param>
        protected void ExecuteWithRollback(string reusableActionName, Action doAction, Action undoAction)
        {
            lock (_lock)
            {
                if (Transaction.Current == null)
                {
                    if (doAction != null)
                    {
                        doAction();
                    }
                }
                else
                {
                    var step = null as TransactionStep;
                    var existingTransactionId = Tuple.For(reusableActionName, Transaction.Current.TransactionInformation.LocalIdentifier);
                    if (reusableActionName != null)
                    {
                        if (_transactionsDictionary.ContainsKey(existingTransactionId))
                        {
                            step = _transactionsDictionary[existingTransactionId];
                        }
                    }
                    if (step == null)
                    {
                        step = new TransactionStep(_lock)
                        {
                            RollbackAction = undoAction
                        };
                        Transaction.Current.EnlistVolatile(
                            step,
                            EnlistmentOptions.None
                            );

                        if (reusableActionName != null)
                        {
                            _transactionsDictionary.Add(existingTransactionId, step);
                        }
                    }

                    if (doAction != null)
                    {
                        doAction();
                    }
                }
            }
        }
    }
}
