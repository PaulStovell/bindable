namespace Bindable.Core.CommandLine.Options
{
    public delegate void OptionAction<TKey, TValue>(TKey key, TValue value);
}