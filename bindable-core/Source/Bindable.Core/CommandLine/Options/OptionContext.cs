namespace Bindable.Core.CommandLine.Options
{
    public class OptionContext
    {
        private readonly OptionValueCollection c;
        private readonly OptionSet set;

        public OptionContext(OptionSet set)
        {
            this.set = set;
            c = new OptionValueCollection(this);
        }

        public Option Option { get; set; }

        public string OptionName { get; set; }

        public int OptionIndex { get; set; }

        public OptionSet OptionSet
        {
            get { return set; }
        }

        public OptionValueCollection OptionValues
        {
            get { return c; }
        }
    }
}