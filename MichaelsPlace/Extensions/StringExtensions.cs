using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Extensions
{
    public static class StringExtensions
    {
        public static bool IsPresent(this string @this) => !string.IsNullOrEmpty(@this);
        public static bool IsMissing(this string @this) => string.IsNullOrEmpty(@this);
        public static string ToCamelCase(this string @this) => @this?.Substring(0, 1)?.ToLower() + @this?.Substring(1);
    }
}
