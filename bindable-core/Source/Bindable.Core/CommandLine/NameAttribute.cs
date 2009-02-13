using System;

namespace Bindable.Core.CommandLine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name, params string[] aliases)
        {
            Name = name;
            Aliases = aliases;
        }

        public string Name { get; set; }
        public string[] Aliases { get; set; }
    }
}