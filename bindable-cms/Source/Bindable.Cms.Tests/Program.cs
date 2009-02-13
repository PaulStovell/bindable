using MbUnit.Core;

namespace Bindable.Cms.Tests
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            using (var runner = new AutoRunner())
            {
                runner.Run();
                runner.ReportToHtml();
            }
        }
    }
}
