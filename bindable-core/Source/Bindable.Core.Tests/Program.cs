using MbUnit.Core;

namespace Bindable.Core.Tests
{
    internal static class Program
    {
        internal static int Main(string[] args)
        {
            using (var runner = new AutoRunner())
            {
                runner.Run();
            }
            return 0;
        }
    }
}