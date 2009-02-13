using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Criterion;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Framework.Validation;
using PaulStovell.Domain.Model;

namespace PaulStovell.Domain.Repositories.NHibernate
{
    /// <summary>
    /// Implements the retrieval and storage of members and related security objects using NHibernate.
    /// </summary>
    public class MembershipRepository : AbstractRepository, IMembershipRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipRepository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public MembershipRepository(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Validates and saves a member.
        /// </summary>
        /// <param name="member">The member to save.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating whether the member was saved.
        /// </returns>
        public ValidationResult SaveMember(Member member)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Serializable))
            {
                var validationResult = member.Validate();
                
                // Ensure OpenID is unique
                if (Session.CreateCriteria(typeof(Member))
                    .Add(Restrictions.Like("OpenId", member.OpenId))
                    .Add(Restrictions.Not(Restrictions.Eq("Id", member.Id)))
                    .SetLockMode(LockMode.Upgrade)
                    .UniqueResult() != null)
                {
                    validationResult.FlagRule("OpenId", string.Format("The OpenID '{0}' is already in use.", member.OpenId));
                }

                // Commit the record
                if (validationResult.Valid)
                {
                    Session.SaveOrUpdate(member);
                    transaction.Commit();
                }
                return validationResult;
            }
        }

        /// <summary>
        /// Finds the member by their OpenID.
        /// </summary>
        /// <param name="openId">The member's unique OpenID.</param>
        /// <returns>The member, or null if not found.</returns>
        public Member FindMemberByOpenId(string openId)
        {
            return Session.CreateCriteria(typeof(Member)).Add(Restrictions.Eq("OpenId", openId)).UniqueResult<Member>();
        }

        /// <summary>
        /// Finds all members.
        /// </summary>
        /// <returns></returns>
        public ICollection<Member> FindAllMembers()
        {
            return Session.CreateCriteria(typeof(Member)).List<Member>();
        }
    }
}