using System.Linq;
using System.Text;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    internal class HtmlParagraphWriter
    {
        private readonly StringBuilder _output = new StringBuilder();
        private bool _previousLineEmpty;
        private bool _paragraphOpen;
        private readonly string[] _dontMuckWith = new[] { "pre", "ul", "ol", "blockquote", "hr", "table" };
        private string _currentBadTag;

        public void WriteLine(string line)
        {
            if (_currentBadTag != null)
            {
                var badTagsEncountered = _dontMuckWith.Where(bad => line.Contains("</" + bad + ">"));
                if (badTagsEncountered.Count() > 0)
                {
                    _currentBadTag = null;
                }
                _output.Append(line);
            }
            else if (line.Trim().Length == 0 && _currentBadTag == null)
            {
                if (!_previousLineEmpty && _paragraphOpen)
                {
                    _output.AppendLine("</p>");
                    _paragraphOpen = false;
                    _previousLineEmpty = false;
                }
                _previousLineEmpty = true;
            }
            else
            {
                var badTagsEncountered = _dontMuckWith.Where(bad => line.Contains("<" + bad + " ") || line.Contains("<" + bad + ">"));
                if (badTagsEncountered.Count() == 0)
                {
                    if (!line.StartsWith("<") && !line.StartsWith("["))
                    {
                        if (_paragraphOpen == false)
                        {
                            _output.AppendLine("<p>");
                            _paragraphOpen = true;
                        }
                    }

                    _output.Append(line);
                    _previousLineEmpty = false;
                }   
                else
                {
                    _currentBadTag = badTagsEncountered.First();
                    if (_paragraphOpen)
                    {
                        _output.AppendLine("</p>");
                        _paragraphOpen = false;
                    }
                    _output.Append(line);
                }
            }
        }

        public override string ToString()
        {
            return _output.ToString();
        }
    }
}