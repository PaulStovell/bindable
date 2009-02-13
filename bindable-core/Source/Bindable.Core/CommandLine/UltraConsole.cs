using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Core.CommandLine
{
    public static class UltraConsole
    {
        private static int _indentationLevel;

        static UltraConsole()
        {
            var width = Math.Max(Console.BufferWidth, 400);
            var height = Math.Max(Console.BufferHeight, 400);
            Console.SetBufferSize(width, height);
        }

        public static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            var original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = original;
        }

        private static void WriteInternal(string text)
        {
            var padding = "".PadLeft(_indentationLevel);
            if (Console.CursorLeft + text.Length >= Console.BufferWidth)
            {
                text = WrapForConsole(text, padding, padding);
            }
            else
            {
                if (Console.CursorLeft == 0)
                {
                    text = padding + text;
                }
            }
            Console.Write(text);
        }

        public static void WriteLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                WriteLine(line);
            }
        }

        public static void WriteList(IEnumerable<string> lines)
        {
            using (Indent())
            {
                WriteLines(lines);
            }
            WriteLine();
        }

        public static void WriteList(params string[] lines)
        {
            using (Indent())
            {
                WriteLines(lines);
            }
            WriteLine();
        }

        private static string WrapForConsole(string message, string firstLinePrefix, string normalLinePrefix)
        {
            var consoleBuffer = new StringBuilder();
            var maxLength = Console.BufferWidth;

            consoleBuffer.Append(firstLinePrefix);
            var currentLineLength = consoleBuffer.Length;

            var words = message.Split(' ');

            foreach (var currentWord in words)
            {
                if (string.IsNullOrEmpty(currentWord)) continue;

                if ((currentLineLength + currentWord.Length) < maxLength)
                {
                    consoleBuffer.Append(currentWord);
                    consoleBuffer.Append(' ');
                    currentLineLength += currentWord.Length + 1;
                }
                else
                {
                    if (consoleBuffer.Length > 0 && consoleBuffer[consoleBuffer.Length - 1] == ' ')
                        consoleBuffer.Remove(consoleBuffer.Length - 1, 1);
                    consoleBuffer.Append(Environment.NewLine);
                    consoleBuffer.Append(normalLinePrefix);
                    consoleBuffer.Append(currentWord);
                    consoleBuffer.Append(' ');
                    currentLineLength = normalLinePrefix.Length + currentWord.Length + 1;
                }
            }
            return consoleBuffer.ToString();
        }

        public static void WriteLine(string format, params object[] args)
        {
            WriteInternal(string.Format(format, args) + "\r\n");
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static bool PromptForYesNo(string question, params object[] args)
        {
            return PromptForOptions(
                string.Format(question, args),
                new Dictionary<bool, string>
                    {
                        {true, "[Y]es"},
                        {false, "[N]o"}
                    }
                );
        }


        public static void WriteTable<TElement>(IEnumerable<TElement> items, Action<TableBuilder<TElement>> buildTable)
        {
            WriteTableInternal(items, buildTable);
            WriteLine();
        }

        public static void WriteSingleRowTable(Action<TableBuilder<object>> buildTable)
        {
            var items = new object[] {"Hello"};
            WriteTableInternal(items, buildTable);
            WriteLine();
        }

        public static void WriteTableInternal<TElement>(IEnumerable<TElement> items,
                                                        Action<TableBuilder<TElement>> buildTable)
        {
            var table = new TableBuilder<TElement>();
            buildTable(table);
            table.Render(items);
        }

        public static void WriteColumns(params string[] values)
        {
            WriteTableInternal(
                new[] {values},
                table =>
                    {
                        table.ShowHeaders = false;
                        for (var i = 0; i < values.Length; i++)
                        {
                            var j = i;
                            table.AddColumn(i.ToString(), items => items[j]);
                        }
                    }
                );
        }

        public static T PromptForOptions<T>(string question, Dictionary<T, string> answers)
        {
            var answerKeys = new List<string>(answers.Count);
            foreach (var possibleAnswer in answers)
            {
                answerKeys.Add(possibleAnswer.Value.Substring(possibleAnswer.Value.IndexOf("[") + 1, 1).ToLower());
            }

            WriteLine(question);
            for (var i = 0; i < answers.Count; i++)
            {
                WriteLine(string.Format("   [{0}] {1}", answerKeys[i],
                                        answers[answers.Keys.ElementAt(i)].Replace("[", "").Replace("]", "")));
            }

            var answer = "";
            while (answer.Trim() == "" || !answerKeys.Contains(answer.ToLower()))
            {
                WriteInternal("> ");
                answer = ReadLine();
            }
            WriteLine();
            return answers.Keys.ElementAt(answerKeys.IndexOf(answer));
        }

        public static string Prompt(string question, bool isRequired)
        {
            WriteLine(question);
            while (true)
            {
                WriteInternal("> ");
                var line = ReadLine().Trim();
                if (string.IsNullOrEmpty(line))
                {
                    if (isRequired)
                    {
                        continue;
                    }
                }
                return line;
            }
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static void ReadKey()
        {
            Console.ReadKey();
        }

        public static void PressAnyKeyToContinue()
        {
            WriteLine("Press any key to continue...");
            ReadKey();
        }

        public static TemporaryIndent Indent()
        {
            return Indent(2);
        }

        public static TemporaryIndent Indent(int spaces)
        {
            _indentationLevel += spaces;
            return new TemporaryIndent(() => _indentationLevel -= spaces);
        }

        public static IDisposable ChangeColor(ConsoleColor color)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            return new TemporaryDisposable(() => Console.ForegroundColor = previous);
        }

        public static void Write(string value)
        {
            WriteInternal(value);
        }

        public static void WriteParagraph(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
            WriteLine();
        }

        public static void WriteError(string format, params object[] args)
        {
            WriteParagraph(ConsoleColor.Red, format, args);
        }

        public static void WriteParagraph(ConsoleColor consoleColor, string format, params object[] args)
        {
            using (ChangeColor(consoleColor))
            {
                WriteParagraph(format, args);
            }
        }

        public static void WriteHeading(string format, params object[] args)
        {
            WriteHeading(ConsoleColor.White, format, args);
        }

        public static void WriteHeading(ConsoleColor consoleColor, string format, params object[] args)
        {
            using (ChangeColor(consoleColor))
            {
                WriteLine(string.Format(format, args));
            }
        }

        #region Nested type: TemporaryDisposable

        public class TemporaryDisposable : IDisposable
        {
            private readonly Action _callback;

            public TemporaryDisposable(Action callback)
            {
                _callback = callback;
            }

            #region IDisposable Members

            public void Dispose()
            {
                _callback();
            }

            #endregion
        }

        #endregion

        #region Nested type: TemporaryIndent

        public class TemporaryIndent : TemporaryDisposable
        {
            public TemporaryIndent(Action callback) : base(callback)
            {
            }

            public void WriteParagraph(string format, params object[] args)
            {
                UltraConsole.WriteParagraph(format, args);
                Dispose();
            }
        }

        #endregion
    }
}