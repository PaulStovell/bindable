using System.Windows.Input;

namespace Bindable.Windows.Controls
{
    public static class MessageListCommands
    {
        public static RoutedUICommand Close = new RoutedUICommand("Close", "Close", typeof(MessageListCommands));
        public static RoutedUICommand Focus = new RoutedUICommand("Focus", "Focus", typeof(MessageListCommands));
    }
}