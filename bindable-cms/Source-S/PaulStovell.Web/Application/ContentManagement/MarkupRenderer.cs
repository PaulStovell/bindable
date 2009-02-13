using System.Collections.Generic;
using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement
{
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
                  new SummaryFormatter(), 
                  new ListingFormatter(),
                  new QuoteFormatter(),
                  new TitleFormatter(),
                  new ParentFormatter(),
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

        public Content Render(string markupContent)
        {
            var content = new Content() { Html = markupContent };
            foreach (var formatter in _formatters)
            {
                formatter.Apply(content, _context);
            }
            return content;
        }
    }
}