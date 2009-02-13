using NHibernate;
using System.Collections;

namespace Bindable.Cms.Domain.Framework
{
    public interface IPostSave
    {
        void OnSaved();
    }

    public class PostSaveInterceptor : EmptyInterceptor
    {
        public override void PostFlush(ICollection entities)
        {
            foreach (var entity in entities)
            {
                var postSaveNotifiable = entity as IPostSave;
                if (postSaveNotifiable != null) postSaveNotifiable.OnSaved();
            }
        }
    }
}
