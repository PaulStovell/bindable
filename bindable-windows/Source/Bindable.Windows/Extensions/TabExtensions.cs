using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bindable.Windows.Extensions
{
    public sealed class TabExtensions
    {
        public static readonly DependencyProperty SupportsCloseProperty = DependencyProperty.RegisterAttached("SupportsClose", typeof(bool), typeof(TabExtensions), new UIPropertyMetadata(false, SupportsClosePropertySet));
        public static readonly RoutedEvent TabClosingEvent = EventManager.RegisterRoutedEvent("TabClosing", RoutingStrategy.Bubble, typeof(TabClosingEventHandler), typeof(TabExtensions));

        public TabExtensions()
        {
        }

        public static void AddTabClosingHandler(DependencyObject targetElement, TabClosingEventHandler handler)
        {
            ((TabItem)targetElement).AddHandler(TabClosingEvent, handler);
        }

        public static void RemoveTabClosingHandler(DependencyObject targetElement, TabClosingEventHandler handler)
        {
            ((TabItem)targetElement).RemoveHandler(TabClosingEvent, handler);
        }

        public static bool GetSupportsClose(DependencyObject obj)
        {
            return (bool)obj.GetValue(SupportsCloseProperty);
        }

        public static void SetSupportsClose(DependencyObject obj, bool value)
        {
            obj.SetValue(SupportsCloseProperty, value);
        }

        private static void SupportsClosePropertySet(DependencyObject targetElement, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var tabItem = targetElement as TabItem;
                if (tabItem != null)
                {
                    tabItem.Loaded += TabItem_Loaded;
                    if (tabItem.IsLoaded)
                    {
                        TabItem_Loaded(tabItem, null);
                    }
                }
            }
        }

        private static void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem != null)
            {
                tabItem.CommandBindings.Add(new CommandBinding(TabCommands.CloseTab, CloseTabCommand_Executed, CloseTabCommand_CanExecute));
            }
        }

        private static void CloseTabCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem != null)
            {
                e.CanExecute = GetSupportsClose(tabItem);
            }
        }

        private static void CloseTabCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem != null)
            {
                if (GetSupportsClose(tabItem))
                {
                    var tabClosingArgs = new TabClosingEventArgs(tabItem, TabClosingEvent);
                    tabItem.RaiseEvent(tabClosingArgs);
                    if (!tabClosingArgs.Cancel)
                    {
                        var tabControl = (TabControl)tabItem.Parent;
                        tabControl.Items.Remove(tabItem);
                    }
                }
            }
        }
    }
}
