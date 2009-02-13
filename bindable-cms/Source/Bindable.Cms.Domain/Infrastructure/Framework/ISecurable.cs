using System;

namespace Bindable.Cms.Domain.Framework
{
    public interface ISecurable
    {
        Guid SecurityKey { get; }
    }
}