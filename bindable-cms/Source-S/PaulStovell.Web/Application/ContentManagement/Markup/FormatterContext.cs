using System.Web;
using PaulStovell.Domain.Model;
using PaulStovell.Domain.Repositories;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class MarkupRendererContext
    {
        private readonly IEntryRepository _repository;
        private readonly string _directory;
        private readonly string _area;

        public MarkupRendererContext(IEntryRepository repository, string directory, string area)
        {
            _repository = repository;
            _directory = directory;
            _area = area;
        }

        public IEntryRepository Repository
        {
            get { return _repository; }
        }

        public string ResolveContentAttachmentPath(string relativePath)
        {
            return string.Format("{0}/{1}/Attachments/{2}", _directory, _area, relativePath);
        }

        public string ResolveInterContentLink(string relativePath)
        {
            return string.Format("/{0}/{1}", _area, HttpUtility.UrlEncode(relativePath.ToLower())).ToLower();
        }

        public Content LoadSiblingContent(string path)
        {
            return null;
            //return Repository.FindEntry(_area, path);
        }
    }
}