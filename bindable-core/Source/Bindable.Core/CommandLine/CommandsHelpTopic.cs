using System.ComponentModel;
using System.Linq;
using Bindable.Core.Helpers;

namespace Bindable.Core.CommandLine
{
    /// <remarks>
    /// This is only temporary. In future we'll want multiple languages and the ability to show the help documents in 
    /// multiple formats, so I suspect we'll go towards an XML or HTML based system either on the file system or as 
    /// embedded resources/satellite assemblies (maybe both). 
    /// </remarks>
    [Name("commands", "cmds")]
    public class CommandsHelpTopic : IHelpTopic
    {
        public void Help(CommandExecutionContext context)
        {
            var commands = context.CommandLocator.FindAllCommands().Select(
                cmd => new
                           {
                               Name = ReflectionHelper.GetAttributeValue<NameAttribute, string>(cmd, n => n.Name, ""),
                               Description = ReflectionHelper.GetAttributeValue<DescriptionAttribute, string>(cmd, n => n.Description, ""),
                           })
                .OrderBy(cmd => cmd.Name);
            
            UltraConsole.WriteParagraph("The following commands are available for {0}.", context.ApplicationTitle);
            UltraConsole.WriteHeading("Commands: ");
            UltraConsole.WriteTable(commands,
                                    table =>
                                        {
                                            table.ShowHeaders = false;
                                            table.AddColumn("Command", cmd => cmd.Name);
                                            table.AddColumn("Description", cmd => cmd.Description);
                                        }
                );
            UltraConsole.WriteHeading("Further help: ");
            UltraConsole.WriteColumns(string.Format("{0} help COMMAND", context.ApplicationExeFileName), "Get help for a particular command");
        }
    }
}