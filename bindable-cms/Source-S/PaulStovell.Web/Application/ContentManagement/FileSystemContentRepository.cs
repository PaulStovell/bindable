//using System;
//using System.IO;
//using System.Web;
//using System.Web.Caching;
//using PaulStovell.Domain.Model;
//using PaulStovell.Web.Application.ContentManagement.Markup;

//namespace PaulStovell.Web.Application.ContentManagement
//{
//    public class FileSystemContentRepository : IContentRepository
//    {
//        private readonly string _directory;

//        public FileSystemContentRepository(string directory)
//        {
//            _directory = directory;
//        }
        
//        public Content LoadContent(string area, string entryName)
//        {
//            // Use the cache if available
//            var cacheKey = "Wiki--" + area + "--" + entryName;
//            var cached = HttpContext.Current.Cache[cacheKey];
//            if (cached != null)
//            {
//                return (Content)cached;
//            }

//            // Validate the request
//            if (area == null || area.Contains("/") || area.Contains(".")) return null;
//            if (entryName == null || entryName.Contains("/") || entryName.Contains(".")) return null;
//            var physicalPath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}.wiki", _directory, area, entryName));
//            if (!File.Exists(physicalPath))
//            {
//                return null;
//            }
            
//            // Load and transform the content
//            var entryMarkup = File.ReadAllText(physicalPath);
//            var context = new MarkupRendererContext(this, _directory, area);
//            var renderer = new MarkupRenderer(context);
//            var finalContent = renderer.Render(entryMarkup);

//            finalContent.SourceFiles.Add(physicalPath);
//            HttpContext.Current.Cache.Add(cacheKey, finalContent, new CacheDependency(finalContent.SourceFiles.ToArray()),
//                                          DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration,
//                                          CacheItemPriority.Normal, null);
//            return finalContent;
//        }
//    }
//}