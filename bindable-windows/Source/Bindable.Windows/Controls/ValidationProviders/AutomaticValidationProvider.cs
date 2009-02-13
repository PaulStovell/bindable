using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using Bindable.Windows.Framework;

namespace Bindable.Windows.Controls.ValidationProviders
{
    /// <summary>
    /// Represents the automatically applied validation provider, which chooses the right validation provider based on the object being validated. Users with custom 
    /// validation needs can assign their own ValidationProvider to the ValidationScope; this is just a helper for out-of-the-box providers to make life easy.
    /// </summary>
    [ContentProperty("ValidationProviders")]
    public class AutomaticValidationProvider : ValidationProvider, ILogicalChildContainer
    {
        public static readonly DependencyProperty ValidationProvidersProperty = DependencyProperty.Register("ValidationProviders", typeof(LogicalChildCollection<ValidationProvider>), typeof(AutomaticValidationProvider), new UIPropertyMetadata(null));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticValidationProvider"/> class.
        /// </summary>
        public AutomaticValidationProvider()
        {
            ValidationProviders = new LogicalChildCollection<ValidationProvider>(this);
        }

        /// <summary>
        /// Gets or sets the validation providers.
        /// </summary>
        /// <value>The validation providers.</value>
        public LogicalChildCollection<ValidationProvider> ValidationProviders
        {
            get { return (LogicalChildCollection<ValidationProvider>)GetValue(ValidationProvidersProperty); }
            set { SetValue(ValidationProvidersProperty, value); }
        }

        /// <summary>
        /// Adds the provided element as a child of this element.
        /// </summary>
        /// <param name="child">The child element to be added.</param>
        void ILogicalChildContainer.AddLogicalChild(object child)
        {
            AddLogicalChild(child);
        }

        /// <summary>
        /// Removes the specified element from the logical tree for this element.
        /// </summary>
        /// <param name="child">The element to remove.</param>
        void ILogicalChildContainer.RemoveLogicalChild(object child)
        {
            AddLogicalChild(child);
        }

        private ValidationProvider GetFirstAppropriateValidationProvider(ValidationContext context)
        {
            var result = null as ValidationProvider;
            foreach (var item in ValidationProviders)
            {
                if (!item.CanValidate(context)) continue;
                
                result = item;
                break;
            }
            return result;
        }

        /// <summary>
        /// Determines whether this instance can validate the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can validate the specified context; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanValidate(ValidationContext context)
        {
            return GetFirstAppropriateValidationProvider(context) != null;
        }

        /// <summary>
        /// Validates the specified object, returning a collection of validation rules.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<IValidationFieldResult> Validate(ValidationContext context)
        {
            var provider = GetFirstAppropriateValidationProvider(context);
            return provider != null ? provider.Validate(context) : new IValidationFieldResult[0];
        }
    }
}