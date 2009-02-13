using NHibernate;
using System.Collections;

namespace PaulStovell.Domain.Framework
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
