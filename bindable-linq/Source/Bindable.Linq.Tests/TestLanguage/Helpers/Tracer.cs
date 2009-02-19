using System;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// Represents a helper class used for tracing the execution of a scenario, to provide
    /// context to any test failures.
    /// </summary>
    public static class Tracer
    {
        private static int _indentation;

        /// <summary>
        /// Writes the string using the specified format and arguments.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Write(string format, params object[] arguments)
        {
            Console.Write("".PadLeft(_indentation * 4) + string.Format(format, arguments));
        }

        /// <summary>
        /// Writes a line containing a string with the specified format and arguments.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void WriteLine(string format, params object[] arguments)
        {
            Write(format + Environment.NewLine, arguments);
        }

        /// <summary>
        /// Indents the output and returns an <see cref="IDisposable"/> reference that 
        /// allows the Indent call to be done within a Using block.
        /// </summary>
        /// <returns></returns>
        public static IDisposable Indent()
        {
            _indentation++;
            return new Disposer(() => _indentation--);
        }

        /// <summary>
        /// An object returned from the Indent call.
        /// </summary>
        private class Disposer : IDisposable
        {
            private readonly Action _callbackAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="Disposer"/> class.
            /// </summary>
            /// <param name="callbackAction">The callback action.</param>
            public Disposer(Action callbackAction)
            {
                _callbackAction = callbackAction;
            }

            #region IDisposable Members
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _callbackAction();
            }
            #endregion
        }
    }
}