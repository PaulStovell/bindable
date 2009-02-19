using System;
using System.Collections.Generic;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// A lookup for projections.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class ProjectionRegister<TSource, TResult> : IDisposable
    {
        private readonly object _projectionLock = new object();
        private readonly IDictionary<TSource, TResult> _projections = new Dictionary<TSource, TResult>();
        private readonly Func<TSource, TResult> _projector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionRegister&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        public ProjectionRegister(Func<TSource, TResult> projector)
        {
            _projector = projector;
        }

        /// <summary>
        /// Gets the projection lock.
        /// </summary>
        private object ProjectionLock
        {
            get { return _projectionLock; }
        }

        /// <summary>
        /// Gets the projections store.
        /// </summary>
        private IDictionary<TSource, TResult> Projections
        {
            get { return _projections; }
        }

        
        /// <summary>
        /// Remembers a projection from the source type to the result type.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <param name="result">The result type.</param>
        public void Store(TSource source, TResult result)
        {
            lock (ProjectionLock)
            {
                if (_projections.ContainsKey(source))
                {
                    _projections[source] = result;
                }
                else
                {
                    _projections.Add(source, result);
                }
            }
        }

        /// <summary>
        /// Determines whether a projection already exists for the given source.
        /// </summary>
        /// <param name="source">The source.</param>
        public bool HasExistingProjection(TSource source)
        {
            return InnerGetExistingProjection(source) != null;
        }

        /// <summary>
        /// Gets an existing projection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private object InnerGetExistingProjection(TSource source)
        {
            Guard.NotNull(source, "source");
            object result = null;
            if (source != null)
            {
                lock (ProjectionLock)
                {
                    if (Projections.ContainsKey(source))
                    {
                        result = Projections[source];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Recalls a previous projection from the source type to the result type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TResult ReProject(TSource source)
        {
            var projected = _projector(source);
            Store(source, projected);
            return projected;
        }

        /// <summary>
        /// Recalls a previous projection from the source type to the result type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TResult Project(TSource source)
        {
            var result = InnerGetExistingProjection(source);

            if (result != null)
            {
                return (TResult) result;
            }
            else
            {
                var projected = _projector(source);
                Store(source, projected);
                return projected;
            }
        }

        /// <summary>
        /// Projects the range of elements.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public IEnumerable<TResult> CreateOrGetProjections(IEnumerable<TSource> range)
        {
            foreach (var source in range)
            {
                if (source != null)
                {
                    yield return Project(source);
                }
            }
        }

        /// <summary>
        /// Gets a range of already existing projections for items.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public IEnumerable<TResult> GetProjections(IEnumerable<TSource> range)
        {
            var results = new List<TResult>();
            foreach (var source in range)
            {
                if (source != null)
                {
                    var existing = InnerGetExistingProjection(source);
                    if (existing != null)
                    {
                        results.Add((TResult) existing);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Removes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Remove(TSource source)
        {
            lock (ProjectionLock)
            {
                if (_projections.ContainsKey(source))
                {
                    _projections.Remove(source);
                }
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="sourceItems">The source items.</param>
        public void RemoveRange(IEnumerable<TSource> sourceItems)
        {
            foreach (var source in sourceItems)
            {
                Remove(source);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (ProjectionLock)
            {
                Projections.Clear();
            }
        }

        /// <summary>
        /// Gets the existing projection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public object GetExistingProjection(TSource item)
        {
            return InnerGetExistingProjection(item);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
    }
}