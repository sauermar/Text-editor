using System;

namespace Components.Extensions
{
    [Sauerova]
    internal static class Enum<T> where T : struct
    {
        /// <summary>
        /// Returns parsed value of type T or defaultValue, case sensitive.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Parse(string value, T defaultValue)
        {
            return Parse(value, defaultValue, false);
        }
        /// <summary>
        /// Parses an enum of type T from value or returns the defaultValue.
        /// </summary>
        /// <param name="value">The value to be parsed</param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse(string value, T defaultValue, bool ignoreCase)
        {
            var result = default(T);
            if (Enum.TryParse(value, ignoreCase, out result)) {
                return result;
            }

            return defaultValue;
        }
    }
}