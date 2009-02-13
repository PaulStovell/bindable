using System;

namespace Bindable.Core.CommandLine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefinitionAttribute : Attribute
    {
        public DefinitionAttribute(string definition)
        {
            Definition = definition;
        }

        public string Definition { get; set; }
    }
}