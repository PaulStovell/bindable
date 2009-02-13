using System;

namespace Bindable.Core
{
    /// <summary>
    /// Represents the comparison rules to use when evaluating whether the SoundEx values of two strings match.
    /// </summary>
    [Flags]
    public enum SoundExComparison
    {
        /// <summary>
        /// The default rules. The first letter must match, and the SoundEx values will only be compared if they are of the same length.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The first letter is not required to be an exact match; instead, the SoundEx score of the first letters will be compared.
        /// </summary>
        IncludeSimilarFirstLetter = 2,
        
        /// <summary>
        /// The SoundEx value on the left is allowed to be less than the SoundEx value on the right. For example, the 
        /// SoundEx score "T260" will match the score "T267". 
        /// </summary>
        IncludeLongerWords = 4
    }
}