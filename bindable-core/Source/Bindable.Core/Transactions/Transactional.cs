using System;

namespace Bindable.Core.Transactions
{
    /// <summary>
    /// Represents a single transactional value. This class is designed for simple types, for example, <see cref="int"/>, <see cref="string"/> or <see cref="DateTime"/>. 
    /// It would provide no value to, for example, an array or a Customer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Transactional<T> : TransactionalObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactional&lt;T&gt;"/> class with a default value.
        /// </summary>
        public Transactional()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactional&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Transactional(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return GetField<T>("Value"); }
            set { SetField("Value", value); }
        }
    }
}