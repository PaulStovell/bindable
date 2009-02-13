using System;
using System.Windows;

namespace Bindable.Windows.Extensions
{
    /// <summary>
    /// This class provides a convenient way to add behavior to an element without accidentally keeping the object alive from a garbage collection point of view. 
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public abstract class ElementExtender<TElement> : IDisposable where TElement : FrameworkElement
    {
        private readonly WeakReference _elementReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementExtender&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        protected ElementExtender(TElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            _elementReference = new WeakReference(element, true);
            if (element.IsLoaded)
            {
                AttachElement();
            }
            element.Loaded += Element_Loaded;
            element.Unloaded += Element_Unloaded;
        }

        /// <summary>
        /// Gets the element being extended.
        /// </summary>
        public TElement GetElement()
        {
            return _elementReference.Target as TElement;
        }

        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            AttachElement();
        }

        private void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            DetachElement();
        }

        /// <summary>
        /// Attaches to the target element. This is called when the element is Loaded, or when the control is first extended 
        /// having already been loaded. 
        /// </summary>
        protected abstract void AttachElement();

        /// <summary>
        /// Detaches from the target element. This is called when the element is Unloaded, or when the extended behavior is no 
        /// longer required (when the ElementExtender is Disposed).
        /// </summary>
        protected abstract void DetachElement();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DetachElement();
        }
    }
}
