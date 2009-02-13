using System.Collections.Generic;

namespace Bindable.Core
{
    public class SoundExDictionary
    {
        private readonly List<string> _dictionaryEntries = new List<string>();
        private readonly Dictionary<string, SoundEx> _dictionarySoundExs = new Dictionary<string, SoundEx>();

        public void Load(IEnumerable<string> strings)
        {
            foreach (var entry in strings)
            {
                _dictionaryEntries.Add(entry);
                _dictionarySoundExs.Add(entry, SoundEx.Evaluate(entry));
            }
        }

        public void Load(params string[] strings)
        {
            Load((IEnumerable<string>)strings);
        }

        public string[] Match(string text)
        {
            return Match(text, SoundExComparison.Default);
        }

        public string[] Match(string text, SoundExComparison comparison)
        {
            var results = new List<string>();
            var textSoundEx = SoundEx.Evaluate(text);
            foreach (var item in _dictionaryEntries)
            {
                var itemSoundEx = _dictionarySoundExs[item];
                if (textSoundEx.Equals(itemSoundEx, comparison))
                {
                    results.Add(item);
                }
            }
            return results.ToArray();
        }
    }
}