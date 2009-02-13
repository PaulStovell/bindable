using System;
using System.Linq;

namespace Bindable.Core
{
    /// <summary>
    /// Represents an English-only SoundEx representation of a string.
    /// </summary>
    /// <remarks>
    /// Follows the rules outlined on Wikipedia and here:
    /// http://www.archives.gov/publications/general-info-leaflets/55.html
    /// </remarks>
    public struct SoundEx : IComparable<SoundEx>, IComparable, IEquatable<SoundEx>, IEquatable<string>
    {
        private static readonly char[] _vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
        private readonly char _first;
        private readonly int _rating;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEx"/> struct.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="rating">The rating.</param>
        private SoundEx(char first, int rating)
            : this()
        {
            _first = first;
            _rating = rating;
        }

        /// <summary>
        /// Gets the first character of the SoundEx string.
        /// </summary>
        /// <value>The first.</value>
        public char First
        {
            get { return _first; }
        }

        /// <summary>
        /// Gets the rating.
        /// </summary>
        /// <value>The rating.</value>
        public int Rating
        {
            get { return _rating; }
        }

        /// <summary>
        /// Gets the SoundEx of the specified text, limiting the SoundEx to the standard three digits.
        /// </summary>
        /// <param name="text">The text to evaluate.</param>
        /// <returns>A SoundEx representing the SoundEx value and with the ability to compare it to other SoundEx values or strings.</returns>
        public static SoundEx Evaluate(string text)
        {
            return Evaluate(text, 3);
        }

        /// <summary>
        /// Gets the SoundEx of the specified text, limiting the SoundEx to a custom number of digits.
        /// </summary>
        /// <param name="text">The text to evaluate.</param>
        /// <param name="maxDigitsLength">The maximum number of digits to include in the SoundEx value; the default is 3.</param>
        /// <returns>
        /// A SoundEx representing the SoundEx value and with the ability to compare it to other SoundEx values or strings.
        /// </returns>
        public static SoundEx Evaluate(string text, int maxDigitsLength)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text");

            var first = text[0];
            var remainder = text.Substring(1);
            var rating = string.Empty;
            var lastCharacter = '\0';
            var lastAppended = '\0';
            var lastAppendedEncoded = 0;
            for (var i = 0; i < remainder.Length; i++)
            {
                var current = remainder[i];
                var currentEncoded = Score(current);
                if (currentEncoded != 0)
                {
                    var doubleLetters = lastAppended == current;
                    if (!doubleLetters)
                    {
                        var doubleSoundExCode = lastAppendedEncoded == currentEncoded;
                        var doubleSoundExCodeSeperatedByVowel = doubleSoundExCode && _vowels.Contains(char.ToLower(lastCharacter));
                        if (!doubleSoundExCode || doubleSoundExCodeSeperatedByVowel)
                        {
                            rating += currentEncoded;
                        }
                        lastAppended = current;
                        lastAppendedEncoded = currentEncoded;
                    }
                }
                lastCharacter = current;
                if (rating.Length == maxDigitsLength) break;
            }

            var ratingValue = 0;
            if (rating.Length > 0)
            {
                ratingValue = int.Parse(rating);
            }

            return new SoundEx(first, ratingValue);
        }

        /// <summary>
        /// Gets the score for a particular letter, returning 0 if the digit should not be counted.
        /// </summary>
        /// <param name="letter">The letter to get the SoundEx score for.</param>
        /// <returns></returns>
        public static int Score(char letter)
        {
            switch (char.ToLower(letter))
            {
                case 'b':
                case 'f':
                case 'p':
                case 'v':
                    return 1;
                case 'c':
                case 'g':
                case 'j':
                case 'k':
                case 'q':
                case 's':
                case 'x':
                case 'z':
                    return 2;
                case 'd':
                case 't':
                    return 3;
                case 'l':
                    return 4;
                case 'm':
                case 'n':
                    return 5;
                case 'r':
                    return 6;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(SoundEx other)
        {
            return ToString().CompareTo(other.ToString());
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj"/>.
        /// Zero
        /// This instance is equal to <paramref name="obj"/>.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="obj"/> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            return ToString().CompareTo(obj.ToString());
        }

        /// <summary>
        /// Gets a value indicating whether the given string has the same SoundEx as this SoundEx.
        /// </summary>
        /// <param name="text">The string to perform a SoundEx comparison on.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="text"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(string text)
        {
            return Equals(text, SoundExComparison.Default);
        }

        /// <summary>
        /// Gets a value indicating whether the given string has the same SoundEx as this SoundEx.
        /// </summary>
        /// <param name="text">The string to perform a SoundEx comparison on.</param>
        /// <param name="comparison">The comparison rules to use.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="text"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(string text, SoundExComparison comparison)
        {
            return Equals(Evaluate(text), SoundExComparison.Default);
        }

        /// <summary>
        /// Gets a value indicating whether the given SoundEx is the same as this SoundEx.
        /// </summary>
        /// <param name="other">The SoundEx to compare to.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(SoundEx other)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the given SoundEx is the same as this SoundEx.
        /// </summary>
        /// <param name="other">The SoundEx to compare to.</param>
        /// <param name="comparison">The comparison rules to use.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(SoundEx other, SoundExComparison comparison)
        {
            if ((comparison & SoundExComparison.IncludeSimilarFirstLetter) == SoundExComparison.IncludeSimilarFirstLetter)
            {
                if (Score(First) != Score(other.First))
                {
                    return false;
                }
            }
            else
            {
                if (First != other.First)
                {
                    return false;
                }
            }

            if ((comparison & SoundExComparison.IncludeLongerWords) == SoundExComparison.IncludeLongerWords)
            {
                var left = Rating.ToString();
                var right = other.Rating.ToString();
                return right.StartsWith(left);
            }
            return Rating == other.Rating;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the SoundEx.
        /// </summary>
        /// <returns>
        /// A string representation of the SoundEx.
        /// </returns>
        public override string ToString()
        {
            var rating = Rating.ToString();
            if (rating.Length < 3)
            {
                rating = rating.PadRight(3, '0');
            }
            return First + rating;
        }
    }
}
