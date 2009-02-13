using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    internal class HtmlListWriter
    {
        private readonly StringBuilder Output = new StringBuilder();
        private readonly Stack<string> Indentations = new Stack<string>();
        private readonly Stack<string> OpenItems = new Stack<string>();
        private int _itemsWritten;
        
        public void AppendToLastItem(string text)
        {
            Output.Append(text);
        }

        public void WriteItem(int itemLevel, string listType, string text)
        {
            // Open the first list
            if (Indentations.Count == 0)
            {
                IncreaseLevel(listType);
            }

            // Decide whether to add a new level, decrease an old level, or simply close the previous node
            if (itemLevel < Indentations.Count)
            {
                CloseLastItem();
                DecreaseToLevel(itemLevel);
            }
            else if (itemLevel > Indentations.Count)
            {
                IncreaseLevel(listType);
            }
            else if (itemLevel == Indentations.Count && _itemsWritten > 0)
            {
                CloseLastItem();
            }

            // Write the current item
            OpenNewItem();

            Output.Append(text);
            _itemsWritten++;
        }

        private void CloseLastItem()
        {
            if (OpenItems.Count <= 0) return;
            
            OpenItems.Pop();
            Output.AppendLine("</li>");
        }

        private void OpenNewItem()
        {
            OpenItems.Push("<li>");
            Output.Append("<li>");
        }

        private void DecreaseLevel()
        {
            var listType = Indentations.Pop();
            Output.Append("</");
            Output.Append(listType);
            Output.AppendLine(">");
        }

        private void DecreaseToLevel(int newLevel)
        {
            var numberToClose = (Indentations.Count - newLevel);
            for (var i = 0; i < numberToClose && Indentations.Count != 0; i++)
            {
                DecreaseLevel();
                CloseLastItem();
            }
        }

        private void IncreaseLevel(string listType)
        {
            Indentations.Push(listType);
            Output.AppendLine();
            Output.Append("<");
            Output.Append(listType);
            Output.AppendLine(">");
        }

        public override string ToString()
        {
            CloseLastItem();
            DecreaseToLevel(0);
            return Output.ToString();
        }
    }
}
