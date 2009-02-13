using System.Windows.Input;

namespace Bindable.Windows.Extensions
{
    public static class TabCommands
    {
        public static RoutedUICommand CloseTab = new RoutedUICommand("Close Tab", "CloseTab", typeof(TabCommands));
    }
}
