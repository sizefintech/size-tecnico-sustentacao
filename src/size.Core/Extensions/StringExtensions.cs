using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace size.Core.Extensions
{
    public static  class StringExtensions
    {
        public static string SomenteNumeros(this string toNormalize)
        {
            if (string.IsNullOrEmpty(toNormalize))
                return string.Empty;
            return Regex.Replace(toNormalize, "[^0-9]", string.Empty);
        }

    }
}
