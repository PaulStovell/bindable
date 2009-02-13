using System;

namespace Bindable.Core.CommandLine
{
    public class CommandLineException : Exception
    {
        public string HelpTopic { get; set; }

        public CommandLineException(string helpTopic, string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
            HelpTopic = helpTopic;
        }
    }
}