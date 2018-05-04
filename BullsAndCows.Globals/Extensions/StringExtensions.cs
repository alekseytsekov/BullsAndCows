using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Globals.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveControllerSuffix(this string text)
        {
            return text.Replace("Controller", "");
        }
    }
}
