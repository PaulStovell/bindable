using System.Collections.Generic;
using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement
{
    public class MarkupContent
    {
        public MarkupContent()
        {
            SourceFiles = new List<string>();
        }

        public List<string> SourceFiles { get; private set; }
        public string Html { get; set; }
    }

    public class MarkupRenderer
    {
        private readonly MarkupRendererContext _context;
        private readonly List<IMarkupFormatter> _formatters;

        public MarkupRenderer(MarkupRendererContext context)
        {
            _context = context;
            _formatters = new List<IMarkupFormatter>
                              {
                                  // Block-level
                                  new ListingFormatter(),
                                  new QuoteFormatter(),
                                  new ImageFormatter(),
                                  new TableFormatter(),
                  
                                  // Paragraph
                                  new ListFormatter(),

                                  // Paragraph level
                                  new BoldFormatter(), 
                                  new HorizontalRuleFormatter(), 
                                  new BackSlashFormatter(),
                                  new HeadingFormatter(),
                                  new InternalLinkFormatter(),
                                  new ParagraphFormatter(),
                                  new IncludeFormatter()
                              };
        }

        public MarkupContent Render(string markupContent)
        {
            var content = new MarkupContent { Html = markupContent };
            foreach (var formatter in _formatters)
            {
                formatter.Apply(content, _context);
            }
            return content;
        }
    }
}