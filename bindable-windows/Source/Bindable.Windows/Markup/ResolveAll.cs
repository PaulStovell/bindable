using System;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.ServiceLocation;

namespace Bindable.Windows.Markup
{
    [MarkupExtensionReturnType(typeof(object))]
    public class ResolveAll : MarkupExtension
    {
        public ResolveAll()
        {
        }

        public ResolveAll(Type type)
        {
            Type = type;
        }

        [ConstructorArgument("type")]
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
                if (provideValueTarget != null)
                {
                    var targetObject = provideValueTarget.TargetObject as DependencyObject;
                    if (targetObject != null)
                    {
                        // If the type isn't specified, infer it from the property being assigned
                        var resolve = Type;
                        if (resolve == null)
                        {
                            if (provideValueTarget.TargetProperty is DependencyProperty)
                            {
                                resolve = ((DependencyProperty)provideValueTarget.TargetProperty).PropertyType;
                            }
                        }

                        if (resolve == null)
                        {
                            throw new Exception("...");
                        }

                        return ServiceLocator.Current.GetAllInstances(resolve);
                    }
                }
            }
            return null;
        }
    }
}