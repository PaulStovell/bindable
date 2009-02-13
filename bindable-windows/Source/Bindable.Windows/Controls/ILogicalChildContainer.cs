namespace Bindable.Windows.Controls
{
    public interface ILogicalChildContainer
    {
        void AddLogicalChild(object child);
        void RemoveLogicalChild(object child);
    }
}