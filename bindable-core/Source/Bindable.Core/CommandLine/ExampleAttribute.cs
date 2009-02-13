using System;

namespace Bindable.Core.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        public ExampleAttribute(string example) : this(example, false)
        {
        }

        public ExampleAttribute(string example, bool showOnDefaultHelp)
        {
            Example = example;
            ShowOnDefaultHelp = showOnDefaultHelp;
        }

        public string Example { get; set; }
        public bool ShowOnDefaultHelp { get; set; }
    }
}