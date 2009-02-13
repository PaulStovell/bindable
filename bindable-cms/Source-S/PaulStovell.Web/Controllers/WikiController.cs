using System;
using System.Web.Mvc;
using PaulStovell.Domain.Model;
using PaulStovell.Domain.Repositories;

namespace PaulStovell.Web.Controllers
{
    [HandleError]
    public class WikiController : Controller
    {
        private readonly IWikiRepository _wikiRepository;
        private readonly IEntryRepository _entryRepository;

        public WikiController(IWikiRepository wikiRepository, IEntryRepository entryRepository)
        {
            _wikiRepository = wikiRepository;
            _entryRepository = entryRepository;
        }

        public ActionResult Entry(string wiki, string path)
        {
            var wikiStore = _wikiRepository.FindActiveWiki(wiki);
            if (wikiStore == null) 
                return View("NotFound");

            ViewData["NavigationGroup"] = wikiStore.Title;
            ViewData["Area"] = wiki;
            ViewData["Wiki"] = wikiStore;

            var entry = _entryRepository.FindEntry(wiki, path);
            if (entry == null) 
                return View("NotFound");
            
            ViewData["Entry"] = entry;

            return View("Entry");
        }

        public ActionResult Submit(string returnUri, string wiki, string path, string name, string email, string url, string comment)
        {
            var wikiStore = _wikiRepository.FindActiveWiki(wiki);
            if (wikiStore == null)
                return View("NotFound");

            var entry = _entryRepository.FindEntry(wiki, path);
            if (entry == null)
                return Entry(wiki, path);

            var submission = entry.CreateComment();
            submission.AuthorEmail = email;
            submission.AuthorIP = "";
            submission.AuthorName = name;
            submission.AuthorUrl = email;
            submission.CommentBody = comment;
            
            var validationResult = submission.Validate();
            if (!validationResult.Valid)
            {
                ViewData["ValidationResult"] = validationResult;
                return Entry(wiki, path);
            }
            return RedirectToAction("Entry", new { wiki, path });
        }
    }
}
