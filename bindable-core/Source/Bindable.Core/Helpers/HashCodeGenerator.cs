namespace Bindable.Core.Helpers
{
    /// <summary>
    /// A helper class for generating hash codes.
    /// </summary>
    public static class HashCodeGenerator
    {
        /// <summary>
        /// Generates a hash code from a set of fields. The hash code will be unique (close enough) for the combination of field values.
        /// </summary>
        /// <param name="fields">The field values.</param>
        /// <returns>A unique hash code for the combination of fields.</returns>
        public static int GenerateHashCode(params object[] fields)
        {
            unchecked
            {
                var hash = 31;
                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        if (field == null) continue;
                        hash *= 41;
                        hash += field.GetHashCode();
                    }
                }
                return hash;
            }
        }
    }
}
