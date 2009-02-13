using System;
using System.Collections.Generic;
using System.Reflection;
using Bindable.Core.Helpers;

namespace Bindable.Core.CommandLine
{
    public interface ICommandLocator
    {
        ICommand FindCommand(params string[] args);
        IEnumerable<ICommand> FindAllCommands();
    }
    
    public interface IHelpTopicLocator
    {
        IHelpTopic FindTopic(string name);
        IEnumerable<IHelpTopic> FindAllTopics();
    }

    public class CommandLocator : ICommandLocator
    {
        private readonly Dictionary<string, Type> _commandNameMappings = new Dictionary<string, Type>();
        private readonly Type[] _commandTypes;
        
        public CommandLocator(params Assembly[] assembliesWithCommands)
        {
            _commandTypes = ReflectionHelper.FindConcreteTypesBasedOn(typeof (ICommand), assembliesWithCommands);
            foreach (var commandType in _commandTypes)
            {
                var commandName = ReflectionHelper.GetAttributeValue<NameAttribute, string>(commandType, att => att.Name, "");
                var aliases = ReflectionHelper.GetAttributeValue<NameAttribute, string[]>(commandType, att => att.Aliases, new string[0]);
                _commandNameMappings[commandName.ToLower()] = commandType;
                foreach (var alias in aliases)
                {
                    if (!_commandNameMappings.ContainsKey(alias))
                    {
                        _commandNameMappings[alias.ToLower()] = commandType;
                    }
                }
            }
        }

        public ICommand FindCommand(params string[] args)
        {
            if (args.Length == 0) return null;

            var firstArgument = args[0];

            if (_commandNameMappings.ContainsKey(firstArgument))
            {
                return (ICommand)Activator.CreateInstance(_commandNameMappings[firstArgument]);
            }
            return null;
        }

        public IEnumerable<ICommand> FindAllCommands()
        {
            var results = new List<ICommand>();
            foreach (var command in _commandTypes)
            {
                results.Add((ICommand) Activator.CreateInstance(command));
            }
            return results;
        }
    }

    public class HelpTopicLocator : IHelpTopicLocator
    {
        private readonly Dictionary<string, Type> _topicMappings = new Dictionary<string, Type>();
        private readonly Type[] _topicTypes;

        public HelpTopicLocator(params Assembly[] assembliesWithCommands)
        {
            _topicTypes = ReflectionHelper.FindConcreteTypesBasedOn(typeof(IHelpTopic), assembliesWithCommands);
            foreach (var commandType in _topicTypes)
            {
                var commandName = ReflectionHelper.GetAttributeValue<NameAttribute, string>(commandType, att => att.Name, "");
                var aliases = ReflectionHelper.GetAttributeValue<NameAttribute, string[]>(commandType, att => att.Aliases, new string[0]);
                _topicMappings[commandName.ToLower()] = commandType;
                foreach (var alias in aliases)
                {
                    if (!_topicMappings.ContainsKey(alias))
                    {
                        _topicMappings[alias.ToLower()] = commandType;
                    }
                }
            }
        }

        public IHelpTopic FindTopic(string name)
        {
            if (_topicMappings.ContainsKey(name.ToLower()))
            {
                return (IHelpTopic)Activator.CreateInstance(_topicMappings[name.ToLower()]);
            }
            return null;
        }

        public IEnumerable<IHelpTopic> FindAllTopics()
        {
            var results = new List<IHelpTopic>();
            foreach (var command in _topicTypes)
            {
                results.Add((IHelpTopic)Activator.CreateInstance(command));
            }
            return results;
        }
    }
}
