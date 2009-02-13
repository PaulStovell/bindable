using System;
using System.Linq;
using Bindable.Cms.Domain.Model;
using Bindable.Cms.Domain.Repositories.NHibernate;
using Bindable.Cms.Tests.Domain.Repositories.Base;
using MbUnit.Framework;
using NHibernate;

namespace Bindable.Cms.Tests.Domain.Repositories
{
    [TestFixture]
    public class EntryRepositoryTests : BaseRepositoryTestFixture
    {
        [Test]
        public void CanCreateRevisionsForNewEntryBeforeSaving()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = CreateAndSaveMember(session);
                var wiki = CreateAndSaveWiki(session, "MyWiki1");
                var entry = CreateNewEntry("MyFirstEntry", "My First Entry", wiki);
                
                var revision = entry.CreateRevision();
                revision.Member = member;
                revision.Body = "Hello world!";
                revision.ModerationStatus = RevisionModerationStatus.Approved;
                revision.RevisionComment = "Changed stuff";
                
                var entryRepository = new EntryRepository(session);
                entryRepository.SaveEntry(entry);
                Assert.AreEqual(1, entry.Id);
            }
        }

        [Test]
        public void CreatingNewRevisionsWillAlwaysDuplicateTheLastRevision()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = CreateAndSaveMember(session);
                var wiki = CreateAndSaveWiki(session, "MyWiki2");
                var entry = CreateNewEntry("MySecondEntry", "My Second Entry", wiki);
                var revision = entry.CreateRevision();
                revision.Member = member;
                revision.Body = "Hello world!";
                revision.ModerationStatus = RevisionModerationStatus.Approved;
                revision.RevisionComment = "Initial revision";

                var entryRepository = new EntryRepository(session);
                entryRepository.SaveEntry(entry);

                var secondRevision = entry.CreateRevision();
                Assert.AreEqual(revision.Body, secondRevision.Body);
                Assert.AreEqual(revision.Member, secondRevision.Member);
                Assert.AreEqual(RevisionModerationStatus.Approved, secondRevision.ModerationStatus);
                Assert.AreEqual(string.Empty, secondRevision.RevisionComment);
            }
            using (var session = Database.SessionFactory.OpenSession())
            {
                var entryRepository = new EntryRepository(session);
                var entry = entryRepository.FindEntry("MyWiki2", "MySecondEntry");
                var revision = entry.Revisions.First();
                var secondRevision = entry.CreateRevision();
                Assert.AreEqual(revision.Body, secondRevision.Body);
                Assert.AreEqual(revision.Member, secondRevision.Member);
                Assert.AreEqual(RevisionModerationStatus.Approved, secondRevision.ModerationStatus);
                Assert.AreEqual(string.Empty, secondRevision.RevisionComment);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), "The method 'set_Body' on type 'Revision' could not be invoked because the instance has been marked as sealed.")]
        public void CannotModifyPastRevisionsOnceSaved()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = CreateAndSaveMember(session);
                var wiki = CreateAndSaveWiki(session, "MyWiki3");
                var entry = CreateNewEntry("MyThirdEntry", "My Third Entry", wiki);
                var revision = entry.CreateRevision();
                revision.Member = member;
                revision.Body = "Hello world!";
                revision.ModerationStatus = RevisionModerationStatus.Approved;
                revision.RevisionComment = "Initial revision";

                var repository = new EntryRepository(session);
                repository.SaveEntry(entry);
                
                // This should fail, as the revision has been sealed
                entry.Revisions.First().Body = "Blah";
            }
        }

        [Test]
        public void CanAppendCommentsToEntry()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = CreateAndSaveMember(session);
                var wiki = CreateAndSaveWiki(session, "MyWiki4");
                var entry = CreateNewEntry("MyThirdEntry", "My Third Entry", wiki);
                var revision = entry.CreateRevision();
                revision.Member = member;
                revision.Body = "Hello world!";
                revision.ModerationStatus = RevisionModerationStatus.Approved;
                revision.RevisionComment = "Initial revision";

                var comment = entry.CreateComment();
                comment.Member = member;
                comment.CommentBody = "Hello world!";
                comment.ModerationStatus = CommentModerationStatus.Awaiting;
                comment.AuthorName = "Mannius Finch";
                comment.AuthorUrl = "www.mannius.com";
                comment.AuthorIP = "125.10.11.9";
                comment.AuthorEmail = "mannius@mannius.com";

                var repository = new EntryRepository(session);
                repository.SaveEntry(entry);
            }
            using (var session = Database.SessionFactory.OpenSession())
            {
                var repository = new EntryRepository(session);
                var entry = repository.FindEntry("MyWiki4", "MyThirdEntry");
                Assert.AreEqual(1, entry.Comments.Count());
                Assert.AreEqual("Paul Stovell", entry.Comments.First().Member.FullName);
                Assert.AreEqual("Mannius Finch", entry.Comments.First().AuthorName);
            }
        }

        private static Member CreateAndSaveMember(ISession session)
        {
            var member = new Member() { OpenId = Guid.NewGuid().ToString(), EmailAddress = "paul@paulstovell.com", FullName = "Paul Stovell", LastLogin = DateTime.Now };
            new MembershipRepository(session).SaveMember(member);
            return member;
        }

        private static Wiki CreateAndSaveWiki(ISession session, string name)
        {
            var wiki = new Wiki();
            wiki.Name = name;
            wiki.Title = "Hello";
            new WikiRepository(session).SaveWiki(wiki);
            return new WikiRepository(session).FindActiveWiki(name);
        }

        private static Entry CreateNewEntry(string name, string title, Wiki wiki)
        {
            var entry = new Entry();
            entry.Wiki = wiki;
            entry.Name = name;
            entry.Title = title;
            entry.Summary = "";
            return entry;
        }
    }
}