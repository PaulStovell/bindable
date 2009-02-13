using System;
using System.Linq.Expressions;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Operators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Checks a condition on a specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static ISwitchDeclaration<TSource> Switch<TSource>(this IBindable<TSource> source)
        {
            return new EmptySwitchDefinition<TSource>(source, source.Dispatcher);
        }

        #region Result type not known yet

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TSource condition, TResult result)
        {
            return switchDeclaration.Case(c => AreEqual(c, condition), result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TSource condition, Expression<Func<TSource, TResult>> result)
        {
            return switchDeclaration.Case(c => AreEqual(c, condition), result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, bool>> condition, TResult result)
        {
            return switchDeclaration.Case(condition, i => result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, bool>> condition, Expression<Func<TSource, TResult>> result)
        {
            return switchDeclaration.CreateForResultType<TResult>().Case(condition, result);
        }

        /// <summary>
        /// Sets the default case for the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TResult result)
        {
            return switchDeclaration.Default(r => result);
        }

        /// <summary>
        /// Sets the default case for the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="switchDeclaration">The switch declaration.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, TResult>> result)
        {
            return switchDeclaration.CreateForResultType<TResult>().Default(result);
        }

        #endregion

        #region Result type known

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TSource condition, TResult result)
        {
            return currentSwitch.Case(c => AreEqual(c, condition), result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TSource condition, Expression<Func<TSource, TResult>> result)
        {
            return currentSwitch.Case(c => AreEqual(c, condition), result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, bool>> condition, TResult result)
        {
            return currentSwitch.Case(condition, i => result);
        }

        /// <summary>
        /// Adds a case to the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, bool>> condition, Expression<Func<TSource, TResult>> result)
        {
            return currentSwitch.AddCase(new SwitchCase<TSource, TResult>(condition, result));
        }

        /// <summary>
        /// Sets the default case for the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TResult result)
        {
            return currentSwitch.Default(i => result);
        }

        /// <summary>
        /// Sets the default case for the current switch statement.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <param name="result">The result.</param>
        /// <returns>The switch statement.</returns>
        public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, TResult>> result)
        {
            return currentSwitch.AddCase(new SwitchDefault<TSource, TResult>(result));
        }

        #endregion

        /// <summary>
        /// Ends the switch.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="currentSwitch">The current switch.</param>
        /// <returns></returns>
        public static IBindable<TResult> EndSwitch<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch)
        {
            return currentSwitch as IBindable<TResult>;
        }

        private static bool AreEqual(object lhs, object rhs)
        {
            return lhs == null ? rhs == null :
                rhs == null ? false : lhs.Equals(rhs);
        }
	}
}
