namespace Bindable.Core.CommandLine
{
    public class CommandExecutionContext
    {
        private readonly ICommandLocator _commandLocator;
        private readonly IHelpTopicLocator _helpLocator;
        
        public CommandExecutionContext(ICommandLocator commandLocator, IHelpTopicLocator helpLocator)
        {
            _commandLocator = commandLocator;
            _helpLocator = helpLocator;
        }

        public string ApplicationTitle { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationExeFileName { get; set; }

        public ICommandLocator CommandLocator
        {
            get { return _commandLocator; }
        }

        public IHelpTopicLocator HelpLocator
        {
            get { return _helpLocator; }
        }
    }
}