using System;

public static class EnglishExtensions
{
    private static readonly string[] HigherDenominations = new[] { "", "thousand", "million" };
    private static readonly string[] TwentyToNinety = new[] { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
    private static readonly string[] ZeroToNineteen = new[] { 
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", 
        "sixteen", "seventeen", "eighteen", "nineteen"
     };

    private static string ConvertLessThanHundred(int value)
    {
        if (value < 20)
        {
            return ZeroToNineteen[value];
        }
        var ten = TwentyToNinety[(value - 20) / 10];
        if ((value % 10) != 0)
        {
            return (ten + "-" + ZeroToNineteen[value % 10]);
        }
        return ten;
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

    public static string ToEnglish(this int value)
    {
        if (value < 100)
        {
            return ConvertLessThanHundred(value);
        }
        if (value < 0x3e8)
        {
            return ConvertLessThanThousand(value);
        }
        var result = string.Empty;
        for (var denomination = 0; denomination < HigherDenominations.Length; denomination++)
        {
            var nextDenomination = denomination - 1;
            var denominationValue = (int) Math.Pow(1000.0, (double) denomination);
            if (denominationValue > value)
            {
                var mod = (int) Math.Pow(1000.0, (double) nextDenomination);
                var quotient = value / mod;
                var remainder = value - (quotient * mod);
                result = ConvertLessThanThousand(quotient) + " " + HigherDenominations[nextDenomination];
                if (remainder > 0)
                {
                    result = result + ", " + remainder.ToEnglish();
                }
                return result;
            }
        }
        return result;
    }
}
