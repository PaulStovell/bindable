using System;

namespace Bindable.Core.Helpers
{
    public static class Guard
    {
        public static void NotNull(object o, string name)
        {
            if (o == null) throw new ArgumentException(name);
        }
    }
}
