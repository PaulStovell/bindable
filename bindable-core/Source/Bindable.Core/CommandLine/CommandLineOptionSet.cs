using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bindable.Core.CommandLine.Options;

namespace Bindable.Core.CommandLine
{
    public class CommandLineOptionSet : OptionSet
    {
        private readonly List<string> _ignoreList = new List<string>();
        private readonly UnnamedArgumentList _unnamedArguments = new UnnamedArgumentList();

        public CommandLineOptionSet()
        {
            Add("<>", _unnamedArguments.Next);
        }

        public OptionSet AddUnnamed(string name, string help, Action<string> action)
        {
            _unnamedArguments.Add(name, help, action);
            return this;
        }

        public void Ignore(params string[] ignore)
        {
            _ignoreList.AddRange(ignore);
        }

        protected override bool Parse(string argument, OptionContext c)
        {
            if (_ignoreList.Where(ig => ig.Equals(argument, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
            {
                return base.Parse(argument, c);
            }
            return true;
        }

        public void Parse(string value)
        {
            var args = (value ?? string.Empty).Split(' ').Where(arg => !string.IsNullOrEmpty(arg));
            Parse(args);
        }

        public void WriteArguments(TextWriter writer)
        {
            _unnamedArguments.WriteArgumentDescriptions(writer);
            writer.WriteLine();
        }

        public void WriteOptions(TextWriter writer)
        {
            WriteOptionDescriptions(writer);
            writer.WriteLine();
        }

        private class UnnamedArgumentList
        {
            private readonly Queue<Action<string>> _actions = new Queue<Action<string>>();
            private readonly List<UnnamedArgument> _arguments = new List<UnnamedArgument>();
          
            public UnnamedArgumentList()
            {
            }

            public void Add(string name, string help, Action<string> action)
            {
                _arguments.Add(new UnnamedArgument() { Name = name ?? string.Empty, Description = help ?? string.Empty });
                _actions.Enqueue(action);
            }

            public void Next(string value)
            {
                if (_actions.Count == 0)
                {
                    throw new CommandLineException("", "The argument '{0}' was not expected.", value);
                }
                var next = _actions.Dequeue();
                next(value);
            }

            public void WriteArgumentDescriptions(TextWriter writer)
            {
                foreach (var argument in _arguments)
                {
                    writer.WriteLine("  {0} {1}", argument.Name.PadRight(26), argument.Description);
                }
            }

            private class UnnamedArgument
            {
                public string Name { get; set; }
                public string Description { get; set; }
            }
        }
    }
}