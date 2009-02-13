namespace Bindable.Core.CommandLine
{
    public abstract class AbstractPromptedCommand : AbstractCommand, IPromptedCommand
    {
        private readonly OptionsPrompter _optionsPrompter = new OptionsPrompter();

        protected AbstractPromptedCommand()
        {
            InitializePrompts();
        }

        protected OptionsPrompter OptionsPrompter
        {
            get { return _optionsPrompter; }
        }

        protected abstract void InitializePrompts();

        public void ExecutePrompted(CommandExecutionContext context)
        {
            OptionsPrompter.Ask();
            Execute(context);
        }
    }
}