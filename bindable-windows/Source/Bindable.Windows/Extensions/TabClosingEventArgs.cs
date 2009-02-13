using System.Windows;
using System.Windows.Controls;

namespace Bindable.Windows.Extensions
{
    public delegate void TabClosingEventHandler(object sender, TabClosingEventArgs e);

    public class TabClosingEventArgs : RoutedEventArgs
    {
        public TabClosingEventArgs(TabItem tabItem, RoutedEvent routedEvent) : base(routedEvent)
        {
            TabItem = tabItem;
        }

        public TabItem TabItem { get; private set; }

        public bool Cancel { get; set; }
    }
}