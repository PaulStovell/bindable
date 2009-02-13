using System;

namespace PaulStovell.Domain.Model
{
    public interface ISecurable
    {
        Guid SecurityKey { get; }
    }
}