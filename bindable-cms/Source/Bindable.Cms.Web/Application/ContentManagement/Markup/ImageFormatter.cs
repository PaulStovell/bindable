using System.Text;
using System.Text.RegularExpressions;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class ImageFormatter : AbstractRegexFormatter
    {
        public ImageFormatter() : base(@"\[Image\s+(?'index'[0-9]+):\s(?'image'.*?)\s+""(?'caption'.*?)""\s*\]")
        {
        }

        public override void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, 
                delegate (Match match)
                    {
                        var image = context.ResolveContentAttachmentPath(match.Groups["image"].Value);
                        var builder = new StringBuilder();
                        builder.AppendLine("<div class='image'>");
                        builder.AppendFormat("<img src='{0}' alt='Image {1}: {2}' />", image, match.Groups["index"].Value, match.Groups["caption"]).AppendLine();
                        builder.AppendFormat("<div>Image {0}: {1}</div>", match.Groups["index"].Value, match.Groups["caption"]);
                        builder.AppendLine("</div>");
                        return builder.ToString();
                    }
                );
        }
    }
}