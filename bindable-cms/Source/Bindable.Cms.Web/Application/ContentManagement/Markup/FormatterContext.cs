using System.Web;
using Bindable.Cms.Domain.Repositories;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class MarkupRendererContext
    {
        private readonly IEntryRepository _repository;
        private readonly string _directory;
        private readonly string _wikiName;

        public MarkupRendererContext(IEntryRepository repository, string directory, string wikiName)
        {
            _repository = repository;
            _directory = directory;
            _wikiName = wikiName;
        }

        public IEntryRepository Repository
        {
            get { return _repository; }
        }

        public string ResolveContentAttachmentPath(string relativePath)
        {
            return string.Format("{0}/{1}/Attachments/{2}", _directory, _wikiName, relativePath);
        }

        public string ResolveInterContentLink(string relativePath)
        {
            return string.Format("/{0}/{1}", _wikiName, HttpUtility.UrlEncode(relativePath.ToLower())).ToLower();
        }

        public MarkupContent LoadSiblingContent(string path)
        {
            return null;
            //return Repository.FindEntry(_area, path);
        }
    }
}