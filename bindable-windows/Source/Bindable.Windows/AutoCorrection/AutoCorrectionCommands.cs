using System.Windows.Input;

namespace Bindable.Windows.AutoCorrection
{
    public static class AutoCorrectionCommands
    {
        public static RoutedCommand UndoLastCorrection = new RoutedCommand("Undo", typeof(AutoCorrectionCommands));
    }
}
