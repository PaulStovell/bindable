using System;
using System.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using Bindable.Linq.Tests.TestLanguage.Steps;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Provides all of the actions that can be performed on an input collection, and allows 
    /// expectations to be added as a result of each action.
    /// </summary>
    internal static class Upon
    {
        /// <summary>
        /// Adds the items to the input collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="itemsToAdd">The items to add.</param>
        /// <returns></returns>
        public static InputAwareStep<TInput> Add<TInput>(params TInput[] itemsToAdd)
        {
            var result = new InputAwareStep<TInput>(
                source => source.AddRange(itemsToAdd)
                );
            return result;
        }

        /// <summary>
        /// Moves the items to the specified index within the input collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="itemsToMove">The items to move.</param>
        /// <returns></returns>
        public static InputAwareStep<TInput> Move<TInput>(int index, params TInput[] itemsToMove)
        {
            var result = new InputAwareStep<TInput>(
                source => source.MoveRange(index, itemsToMove)
                );
            return result;
        }

        /// <summary>
        /// Removes the specified items from the input collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="itemsToRemove">The items to remove.</param>
        /// <returns></returns>
        public static InputAwareStep<TInput> Remove<TInput>(params TInput[] itemsToRemove)
        {
            var result = new InputAwareStep<TInput>(
                source => source.RemoveRange(itemsToRemove)
                );
            return result;
        }

        /// <summary>
        /// Replaces the specified original items in the input collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="originalItems">The original items.</param>
        /// <returns></returns>
        public static ReplaceStep<TInput> Replace<TInput>(params TInput[] originalItems)
        {
            var result = new ReplaceStep<TInput>(originalItems);
            return result;
        }

        /// <summary>
        /// Inserts the items into the specified index within the input collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        /// <returns></returns>
        public static InputAwareStep<TInput> Insert<TInput>(int index, params TInput[] itemsToAdd)
        {
            var result = new InputAwareStep<TInput>(
                source => source.InsertRange(index, itemsToAdd)
                );
            return result;
        }

        /// <summary>
        /// Forces the query to be evaluated by iterating over the items. 
        /// </summary>
        /// <returns></returns>
        public static SimpleStep Evaluate()
        {
            var result = new SimpleStep(
                scenario =>
                {
                    if (scenario.BindableLinqQuery is IEnumerable)
                    {
                        foreach (var o in (IEnumerable)scenario.BindableLinqQuery)
                        {
                            o.ToString();
                        }
                    }
                    else if (scenario.BindableLinqQuery is IBindable)
                    {
                        ((IBindable)scenario.BindableLinqQuery).Evaluate();
                    }
                }
                );
            return result;
        }

        /// <summary>
        /// The query is created but nothing has been done to it yet.
        /// </summary>
        /// <returns></returns>
        public static SimpleStep Construction()
        {
            var result = new SimpleStep(
                delegate (IScenario source) { }
                );
            return result;
        }

        /// <summary>
        /// Reads a given property to verify what would happen when a property is accessed (designed 
        /// for verifying delayed execution).
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static SimpleStep Reading<TReturn>(Func<IBindableCollection, TReturn> callback)
        {
            var result = new SimpleStep(source => callback(source.BindableLinqQuery as IBindableCollection));
            return result;
        }
    }
}