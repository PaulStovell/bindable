using System;
using System.Windows;
using System.Windows.Data;

namespace Bindable.Windows.Helpers
{
    /// <summary>
    /// Contains helper methods for evaluating WPF data bindings.
    /// </summary>
    public static class BindingHelper
    {
        /// <summary>
        /// Clones a binding and assigns a different data context.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="replacementDataContext">The replacement data context.</param>
        /// <returns></returns>
        private static BindingBase CloneBinding(BindingBase original, object replacementDataContext)
        {
            if (original is Binding)
            {
                var originalBinding = (Binding) original;
                var duplicate = new Binding();
                duplicate.BindsDirectlyToSource = originalBinding.BindsDirectlyToSource;
                duplicate.Converter = originalBinding.Converter;
                duplicate.ConverterCulture = originalBinding.ConverterCulture;
                duplicate.ConverterParameter = originalBinding.ConverterParameter;
                duplicate.FallbackValue = originalBinding.FallbackValue;
                duplicate.IsAsync = originalBinding.IsAsync;
                duplicate.Mode = originalBinding.Mode;
                duplicate.Path = originalBinding.Path;
                // Decide on a source
                if (originalBinding.RelativeSource != null) duplicate.RelativeSource = originalBinding.RelativeSource;
                else if (originalBinding.ElementName != null) duplicate.ElementName = originalBinding.ElementName;
                else duplicate.Source = replacementDataContext;
                
                return duplicate;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Evaluates a WPF data binding.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="binding">The binding to evaluate.</param>
        /// <returns></returns>
        public static object EvaluateBinding(BindingBase binding, object dataContext)
        {
            var result = default(object);
            if (dataContext != null)
            {
                var target = new TemporaryBindingTarget();
                var duplicatedBinding = CloneBinding(binding, dataContext);
                var expression = BindingOperations.SetBinding(target, TemporaryBindingTarget.ResultProperty,
                                                              duplicatedBinding);
                expression.UpdateTarget();
                result = target.Result;
                BindingOperations.ClearBinding(target, TemporaryBindingTarget.ResultProperty);
            }
            return result;
        }

        /// <summary>
        /// Evaluates the SelectedValuePath on a data item.
        /// </summary>
        /// <param name="dataItem">The data item.</param>
        /// <param name="bindingPath">The selected value path.</param>
        /// <returns></returns>
        public static object EvaluateBindingPath(string bindingPath, object dataItem)
        {
            var binding = new Binding(bindingPath) { Source = dataItem };
            var result = EvaluateBinding(binding, dataItem);
            return result;
        }

        /// <summary>
        /// A class that is used as a temporary target in a WPF BindingExpression.
        /// </summary>
        private class TemporaryBindingTarget : DependencyObject
        {
            public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(object), typeof(TemporaryBindingTarget), new UIPropertyMetadata(null));

            /// <summary>
            /// Gets or sets the result.
            /// </summary>
            /// <value>The result.</value>
            public object Result
            {
                get { return GetValue(ResultProperty); }
                set { SetValue(ResultProperty, value); }
            }
        }
    }
}
