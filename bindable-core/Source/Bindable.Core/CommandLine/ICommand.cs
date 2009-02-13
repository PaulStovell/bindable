using Bindable.Core.CommandLine;

namespace Bindable.Core.CommandLine
{
    public interface ICommand
    {
        void Execute(CommandExecutionContext context, string[] args);
        void Help(CommandExecutionContext context);
    }
}