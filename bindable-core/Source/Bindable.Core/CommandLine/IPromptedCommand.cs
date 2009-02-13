namespace Bindable.Core.CommandLine
{
    public interface IPromptedCommand : ICommand
    {
        void ExecutePrompted(CommandExecutionContext context);
    }
}
