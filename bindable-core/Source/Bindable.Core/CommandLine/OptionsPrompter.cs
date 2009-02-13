using System;
using System.Collections.Generic;

namespace Bindable.Core.CommandLine
{
    public class OptionsPrompter
    {
        private readonly List<Question> _questions = new List<Question>();

        public void AddQuestion(string text, bool required, Action<string> callback)
        {
            _questions.Add(new Question() {Text = text, IsRequired = required, AnswerCallback = callback});
        }

        public void AddQuestion(string text, Action<string> callback)
        {
            AddQuestion(text, false, callback);
        }

        public void Ask()
        {
            foreach (var question in _questions)
            {
                var line = UltraConsole.Prompt(question.Text, question.IsRequired);
                if (!string.IsNullOrEmpty(line))
                {
                    question.AnswerCallback(line);
                }
            }
        }

        private class Question
        {
            public string Text { get; set; }
            public bool IsRequired { get; set; }
            public Action<string> AnswerCallback { get; set; }
        }
    }
}
