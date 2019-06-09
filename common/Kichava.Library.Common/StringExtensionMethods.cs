namespace Kichava.Library.Common
{
    using System;

    public static class StringExtensionMethods
    {
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }
    }
}
