using System;
using System.Globalization;
using System.Text;
using System.Linq;

namespace Bindable.Windows.AutoCorrection
{
    public class TitleCaseNameCorrector : IAutoCorrector
    {
        private static readonly string[] _namesNotToCorrect = new string[] { "bin" };
        private static readonly string[] _prefixesOrSuffixesToPeriod = new string[] { "mr", "mrs", "miss", "dr", "jnr", "snr" };

        public Correction Correct(object originalValue, Type targetType, CultureInfo currentCulture)
        {
            var names = (originalValue ?? string.Empty).ToString().Split(' ', '\r', '\n', '\t');
            var nameBuilder = new StringBuilder();
            var containsLike = new Func<string[], string, bool>((arr, n) => arr.Where(a => a.Equals(n, StringComparison.CurrentCultureIgnoreCase)).Count() > 0);
            foreach (var name in names)
            {
                var originalName = name.Trim();
                if (originalName.Length > 0)
                {
                    if (containsLike(_namesNotToCorrect, originalName) == false)
                    {
                        nameBuilder.Append(originalName[0].ToString().ToUpper(currentCulture));
                        if (name.Length > 1)
                        {
                            nameBuilder.Append(originalName.Substring(1).ToLower());
                        }

                        if (containsLike(_prefixesOrSuffixesToPeriod, originalName))
                        {
                            nameBuilder.Append(".");
                        }
                    }
                    else
                    {
                        nameBuilder.Append(originalName);
                    }
                    nameBuilder.Append(" ");
                }
            }

            var corrected = nameBuilder.ToString().Trim();
            return corrected != (string)originalValue ? new Correction(originalValue, corrected) : null;
        }
    }
}