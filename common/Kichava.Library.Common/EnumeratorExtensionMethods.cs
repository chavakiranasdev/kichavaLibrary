namespace Kichava.Library.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumeratorExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
        {
            if (input == null)
            {
                return true;
            }
            if (!input.Any())
            {
                return true;
            }
            return false;
        }
    }
}
