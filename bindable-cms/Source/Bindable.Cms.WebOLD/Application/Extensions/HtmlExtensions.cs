﻿using System.Web.Mvc;
using System.Web.Mvc.Html;
using Bindable.Cms.Domain.Framework.Validation;

namespace Bindable.Cms.Web.Application.Extensions
{
    public static class HtmlExtensions
    {
        public static string WikiLink(this HtmlHelper html, string displayText, string wiki)
        {
            return html.ActionLink(displayText, "Entry", "Wiki", new { controller = "Wiki", wiki, path = "" });
        }

        public static string WikiLink(this HtmlHelper html, string displayText, string wiki, string path)
        {
            return html.ActionLink(displayText, "Entry", new {controller = "Wiki", wiki, path = path});
        }

        public static string ValidationFailure(this HtmlHelper html, string validationRuleName, string associatedElementId)
        {
            return ValidationFailure(html, html.ViewData["ValidationResult"], validationRuleName, associatedElementId);
        }

        public static string ValidationFailure(this HtmlHelper html, object validationResult, string validationRuleName, string associatedElementId)
        {
            return ValidationFailure(html, validationResult, null, validationRuleName, associatedElementId);
        }

        public static string ValidationFailure(this HtmlHelper html, object validationResult, object childItem, string validationRuleName, string associatedElementId)
        {
            var result = validationResult as ValidationResult;
            if (result != null && !string.IsNullOrEmpty(result.ForRule(validationRuleName, childItem)))
            {
                var writer = new TagBuilder("label");
                writer.AddCssClass("validation-failure");
                writer.Attributes.Add("for", associatedElementId);
                writer.SetInnerText(result.ForRule(validationRuleName, childItem));
                return writer.ToString(TagRenderMode.Normal);
            }
            return string.Empty;
        }
    }
}