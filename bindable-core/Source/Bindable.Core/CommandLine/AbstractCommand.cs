using System.ComponentModel;
using System.Linq;
using Bindable.Core.Helpers;

namespace Bindable.Core.CommandLine
{
    public abstract class AbstractCommand : ICommand
    {
        private readonly CommandLineOptionSet _optionsParser = new CommandLineOptionSet();

        protected AbstractCommand()
        {
            InitializeOptions();
        }

        protected CommandLineOptionSet OptionsParser
        {
            get { return _optionsParser; }
        }

        protected abstract void InitializeOptions();

        protected abstract void Execute(CommandExecutionContext context);

        public void Execute(CommandExecutionContext context, string[] args)
        {
            OptionsParser.Parse(args);
            Execute(context);
        }

        public void Help(CommandExecutionContext context)
        {
            var name = ReflectionHelper.GetAttributeValue<NameAttribute, string>(this, desc => desc.Name, "[No name given]");
            var description = ReflectionHelper.GetAttributeValue<DescriptionAttribute, string>(this, desc => desc.Description, "[No description given]");
            var usage = ReflectionHelper.GetAttributeValue<DefinitionAttribute, string>(this, desc => desc.Definition, "[No definition given]");
            var examples = ReflectionHelper.GetAttributesValue<ExampleAttribute, string>(this, desc => desc.Example).OrderBy(e => e);

            UltraConsole.WriteHeading(name);
            UltraConsole.Indent().WriteParagraph(description);
            UltraConsole.WriteHeading("Usage: ");
            UltraConsole.Indent().WriteParagraph(usage);
            UltraConsole.WriteHeading("Examples: ");
            UltraConsole.WriteList(examples); 
            UltraConsole.WriteHeading("Options: ");
            OptionsParser.WriteOptions(System.Console.Out);
            UltraConsole.WriteHeading("Arguments: ");
            OptionsParser.WriteArguments(System.Console.Out);
        }
    }
}