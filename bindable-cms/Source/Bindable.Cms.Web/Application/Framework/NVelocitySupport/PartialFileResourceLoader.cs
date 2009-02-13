using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bindable.Core.Helpers;
using Commons.Collections;
using NVelocity.Exception;
using NVelocity.Runtime.Resource;
using NVelocity.Runtime.Resource.Loader;
using NVelocity.Util;
using System.Globalization;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    /// <summary>
    /// The default NVelocity FileResourceLoader simply reads the contents of a file. By contrast, this loader allows the file to be split into multiple sections, 
    /// and for the sections to be loaded independently.
    /// </summary>
    /// <remarks>
    /// Sections are marked up as follows:
    /// <example>
    /// <code>
    /// [HeadContent]
    ///     HTML for the header section goes here.
    /// [/HeadContent]
    /// [LeftColumn]
    ///     HTML for the left column goes here.
    /// [/LeftColumn]
    /// [Content]
    ///     HTML for the body goes here.
    /// [/Content]
    /// </code>
    /// </example>
    /// When loading the view, parts can be loaded using path similar to the below: 
    /// Foo\Bar\MyView.vm:LeftColumn
    /// If no offset is given, it will look for an section called "Content".
    /// </remarks>
    public class PartialFileResourceLoader : ResourceLoader
    {
        private readonly List<Assembly> _assemblies = new List<Assembly>();

        /// <summary>
        /// Initialises the resource loader from the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public override void Init(ExtendedProperties configuration)
        {
            var assemblies = configuration.GetStringList("assembly");
            foreach (var assembly in assemblies)
            {
                try
                {
                    _assemblies.Add(Assembly.Load(assembly));
                }
                catch (Exception ex)
                {
                    throw new ResourceNotFoundException(string.Format("Error when initializing NVelocity partial file resource loader: The assembly '{0}' could not be loaded.", assembly), ex);
                }
            }
        }

        /// <summary>
        /// Gets the time the file was last written, in ticks.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public override long GetLastModified(Resource resource)
        {
            return resource.LastModified;
        }

        /// <summary>
        /// Determines whether the template has been modified since it was last loaded, to enable caching.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public override bool IsSourceModified(Resource resource)
        {
            return false;
        }

        /// <summary>
        /// Gets the resource stream for the template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        public override Stream GetResourceStream(string templateName)
        {
            // A throwback to the Java Velocity:
            if (templateName == "VM_global_library.vm") return null;

            if (string.IsNullOrEmpty(templateName))
            {
                throw new ResourceNotFoundException("Please specify a file name or path");
            }

            var sectionName = null as string;
            var sectionNameIndex = templateName.LastIndexOf(":");
            if (sectionNameIndex >= 0)
            {
                sectionName = new String(templateName.Substring(sectionNameIndex).Where(c => char.IsLetterOrDigit(c)).ToArray());
                templateName = templateName.Substring(0, sectionNameIndex);
            }
            
            var normalisedPath = NormalizeTemplateName(templateName);
            var contentStream = null as Stream;

            // Look for the resource
            foreach (var assembly in _assemblies)
            {
                var manifestResourceName = assembly.GetManifestResourceNames().Where(name=> name.EndsWith(normalisedPath)).FirstOrDefault();
                if (manifestResourceName == null) continue;

                var manifestResourceStream = assembly.GetManifestResourceStream(manifestResourceName);
                if (manifestResourceStream != null)
                {
                    contentStream = manifestResourceStream;
                    break;
                }
            }

            if (contentStream == null)
            {
                TraceHelper.TraceWarning(string.Format("Error when resolving template: cannot locate resource '{0}'", normalisedPath));
            }

            // If a specific section is requested (Foo.vm:Bar), extract it or throw if it 
            // isn't found within the body of the content
            if (!string.IsNullOrEmpty(sectionName) && contentStream != null)
            {
                var contents = null as string;
                using (var streamReader = new StreamReader(contentStream))
                {
                    contents = streamReader.ReadToEnd();
                }
                contents = ExtractContentSection(contents, sectionName);
                contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            }

            return contentStream;
        }

        private static string NormalizeTemplateName(string templateName)
        {
            var normalisedPath = StringUtils.NormalizePath(templateName);
            if (normalisedPath.StartsWith(@"\"))
            {
                normalisedPath = normalisedPath.Substring(1);
            }
            if (string.IsNullOrEmpty(normalisedPath))
            {
                throw new ResourceNotFoundException(string.Format("Template path error: path '{0}' contains '..' and may be trying to access content outside of template root. Rejected.", normalisedPath));
            }
            if (normalisedPath.StartsWith("/") || normalisedPath.StartsWith("."))
            {
                normalisedPath = normalisedPath.Substring(1);
            }
            if (!normalisedPath.EndsWith(".vm", true, CultureInfo.InvariantCulture))
            {
                normalisedPath += ".vm";
            }
            normalisedPath = normalisedPath.Replace('\\', '.').Replace('/', '.');
            return normalisedPath;
        }

        private static string ExtractContentSection(string contents, string sectionName)
        {
            var contentMatcher = string.Format(@"\[{0}\](?'content'.*?)\[\/{0}\]", sectionName);
            var contentMatcherRegex = new Regex(contentMatcher, RegexOptions.Singleline);

            var match = contentMatcherRegex.Match(contents);
            if (match.Success && match.Groups["content"] != null)
            {
                return (match.Groups["content"].Value ?? string.Empty).Trim();
            }
            return string.Empty;
        }
    }
}