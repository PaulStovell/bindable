using System;
using System.ComponentModel;

namespace Bindable.Core.Language
{
    /// <summary>
    /// A helper to convert integers into English words. I am sure you could write this in, like, two lines of F#, 
    /// but I am not that cool.
    /// </summary>
    public class EnglishConverter : TypeConverter
    {
        private static readonly string[] HigherDenominations = { "", "thousand", "million" };
        private static readonly string[] TwentyToNinety = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private static readonly string[] ZeroToNineteen = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

        /// <summary>
        /// Converts the supplied number into the English equivalent.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An English phrase describing the value.</returns>
        public string ToEnglish(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "The ToEnglish extension can only be used on positive integers.");
            }
            if (value < 100)
            {
                return ConvertLessThanHundred(value);
            }
            if (value < 1000)
            {
                return ConvertLessThanThousand(value);
            }
            var result = string.Empty;
            for (var denomination = 0; denomination < HigherDenominations.Length; denomination++)
            {
                var nextDenomination = denomination - 1;
                var denominationValue = (int)Math.Pow(1000, denomination);
                if (denominationValue <= value)
                {
                    continue;
                }

                var mod = (int)(Math.Pow(1000, nextDenomination));
                var quotient = value / mod;
                var remainder = value - (quotient * mod);

                result = ConvertLessThanThousand(quotient) + " " + HigherDenominations[nextDenomination];
                if (remainder > 0)
                {
                    if (remainder > 100)
                    {
                        result = result + ", " + ToEnglish(remainder);
                    }
                    else
                    {
                        result = result + " and " + ToEnglish(remainder);
                    }
                }
                break;
            }
            return result;
        }

        private static string ConvertLessThanThousand(int val)
        {
            var result = "";
            var quotient = val / 100;
            var remainder = val % 100;
            if (quotient > 0)
            {
                result = ZeroToNineteen[quotient] + " hundred";
                if (remainder > 0)
                {
                    result = result + " and ";
                }
            }
            if (remainder > 0)
            {
                result = result + ConvertLessThanHundred(remainder);
            }
            return result;
        }

        private static string ConvertLessThanHundred(int value)
        {
            if (value < 20)
            {
                return ZeroToNineteen[value];
            }

            var ten = TwentyToNinety[(value - 20) / 10];
            if ((value % 10) != 0)
            {
                return ten + "-" + ZeroToNineteen[value % 10];
            }
            return ten;
        }
    }
}