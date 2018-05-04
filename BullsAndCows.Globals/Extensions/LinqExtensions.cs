using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Globals.Extensions
{
    public static class LinqExtensions
    {
        public static void AddRange<T>(this ICollection<T> oc, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var item in collection)
            {
                oc.Add(item);
            }
        }
    }
}
