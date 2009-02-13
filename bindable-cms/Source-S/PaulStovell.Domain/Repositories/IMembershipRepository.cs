using System.Collections.Generic;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Model;
using PaulStovell.Domain.Framework.Validation;

namespace PaulStovell.Domain.Repositories
{
    /// <summary>
    /// Abstracts the retrieval and storage of members and related entities.
    /// </summary>
    public interface IMembershipRepository : IRepository
    {
        /// <summary>
        /// Validates and saves a member.
        /// </summary>
        /// <param name="member">The member to save.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the member was saved.</returns>
        ValidationResult SaveMember(Member member);

        /// <summary>
        /// Finds the member by their OpenID.
        /// </summary>
        /// <param name="openId">The member's unique OpenID.</param>
        /// <returns>The member, or null if not found.</returns>
        Member FindMemberByOpenId(string openId);

        /// <summary>
        /// Finds all members.
        /// </summary>
        ICollection<Member> FindAllMembers();
    }
}