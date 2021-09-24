using System;

namespace Components.Extensions
{
    [Sauerova]
    internal static class StringExtensions
    {
        /// <summary>
        /// Parses an input string to single-precision floating-point number, else returns the default value.
        /// </summary>
        public static float ToSingle(this string input, float defaultValue)
        {
            var result = default(float);
            if (Single.TryParse(input, out result)) {
                return result;
            }

            return defaultValue;
        }
    }
}