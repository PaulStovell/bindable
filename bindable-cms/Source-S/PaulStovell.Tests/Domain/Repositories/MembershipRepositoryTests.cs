using System;
using NUnit.Framework;
using PaulStovell.Domain.Model;
using PaulStovell.Domain.Repositories.NHibernate;
using PaulStovell.Tests.Testing;

namespace PaulStovell.Tests.Domain.Repositories
{
    [TestFixture]
    public class MembershipRepositoryTests : RepositoryTestFixture
    {
        [Test]
        public void CanCreateMember()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = new Member() { OpenId = "www.paulstovell.com/1", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var repository = new MembershipRepository(session);
                repository.SaveMember(member);
            }
        }

        [Test]
        public void CreatingMembersWithSameOpenIDWillReturnValidationErrorOnSave()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member1 = new Member() { OpenId = "www.paulstovell.com/2", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var member2 = new Member() { OpenId = "www.paulstovell.com/2", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var repository = new MembershipRepository(session);
                var result1 = repository.SaveMember(member1);
                var result2 = repository.SaveMember(member2);   
                Assert.AreEqual(true, result1.Valid);
                Assert.AreEqual(false, result2.Valid);
                Assert.AreEqual("The OpenID 'www.paulstovell.com/2' is already in use.", result2.ForRule("OpenId"));
            }
        }

        [Test]
        public void CanChangeMembersOpenIDSoLongAsNewIDIsNotInUse()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member = new Member() { OpenId = "www.paulstovell.com/4", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var repository = new MembershipRepository(session);
                repository.SaveMember(member);
                var member2 = repository.FindMemberByOpenId("www.paulstovell.com/4");
                member2.FullName = "Fred Stovell";
                member2.OpenId = "www.paulstovell.com/4_1";
                repository.SaveMember(member2);
                var member3 = repository.FindMemberByOpenId("www.paulstovell.com/4_1");
                Assert.AreEqual("Fred Stovell", member3.FullName);
                Assert.AreEqual(null, repository.FindMemberByOpenId("www.paulstovell.com/4"));
            }
        }

        [Test]
        public void CannotChangeMemberOpenIDIfNewIDAlreadyInUse()
        {
            using (var session = Database.SessionFactory.OpenSession())
            {
                var member1 = new Member() { OpenId = "www.paulstovell.com/5", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var member2 = new Member() { OpenId = "www.paulstovell.com/6", FullName = "Paul Stovell", EmailAddress = "paul@paulstovell.com", LastLogin = DateTime.Now };
                var repository = new MembershipRepository(session);
                repository.SaveMember(member1);
                repository.SaveMember(member2);
                var member1_1 = repository.FindMemberByOpenId("www.paulstovell.com/6");
                member1_1.OpenId = "www.paulstovell.com/5";
                var result = repository.SaveMember(member1_1);
                Assert.AreEqual(false, result.Valid);
            }
        }
    }
}