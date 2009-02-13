using System.ComponentModel;
using System.Linq;
using Bindable.Core.Helpers;

namespace Bindable.Core.CommandLine
{
    [Name("help", "h", "?")]
    [Description("Provides help about the application or a specific command or topic")]
    [Definition("ApplicationExeFileName help [TOPIC]")]
    [Example("ApplicationExeFileName help", true)]
    [Example("ApplicationExeFileName help install")]
    [Example("ApplicationExeFileName help versions")]
    public class HelpCommand : ICommand
    {
        private readonly CommandLineOptionSet _optionsParser;

        public HelpCommand()
        {
            _optionsParser = new CommandLineOptionSet();
            _optionsParser.Ignore("help");
            _optionsParser.AddUnnamed("TOPIC", "The help topic to display (optional)", next => Topic = next);
        }

        public string Topic { get; private set; }

        public void Execute(CommandExecutionContext context, string[] args)
        {
            _optionsParser.Parse(args);

            if (string.IsNullOrEmpty(Topic))
            {
                Help(context);
            }
            else
            {
                var command = context.CommandLocator.FindCommand(Topic);
                if (command != null)
                {
                    command.Help(context);
                }
                else
                {
                    var topic = context.HelpLocator.FindTopic(Topic);
                    if (topic != null)
                    {
                        topic.Help(context);
                    }
                    else
                    {
                        UltraConsole.WriteLine("Unknown topic: {0}", Topic);
                        Help(context);
                    }
                }
            }
        }

        public void Help(CommandExecutionContext context)
        {
            var examples = context.CommandLocator.FindAllCommands().SelectMany(command => ReflectionHelper.GetAttributesValue<ExampleAttribute, string>(command, desc => desc.Example, desc => desc.ShowOnDefaultHelp)).OrderBy(e => e);
            
            UltraConsole.WriteParagraph(context.ApplicationDescription);
            UltraConsole.WriteHeading("Usage: ");
            UltraConsole.WriteList(
                string.Format("{0} help", context.ApplicationExeFileName),
                string.Format("{0} COMMAND [arguments] [options]", context.ApplicationExeFileName)
                );
            UltraConsole.WriteHeading("Examples: ");
            UltraConsole.WriteList(examples.Select(ex => ex.Replace("ApplicationExeFileName", context.ApplicationExeFileName)));
            UltraConsole.WriteHeading("Further help: ");
            UltraConsole.WriteColumns(string.Format("{0} help commands", context.ApplicationExeFileName), "Lists all available commands");
            UltraConsole.WriteColumns(string.Format("{0} help COMMAND", context.ApplicationExeFileName), "Get help for a particular command");
        }
    }
}