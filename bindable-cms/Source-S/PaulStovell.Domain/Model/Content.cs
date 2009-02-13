using System.Collections.Generic;

namespace PaulStovell.Domain.Model
{
    public class Content
    {
        public Content()
        {
            SourceFiles = new List<string>();
        }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Parent { get; set; }
        public string Html { get; set; }
        
        /// <summary>
        /// Contains the full physical paths to all source files (both the root, and any includes, and 
        /// their includes, and so on) used to load this content. This is primarily to allow cache 
        /// dependencies to monitor all files related to a single cache entry for the content.
        /// </summary>
        public List<string> SourceFiles { get; private set; }
    }
}