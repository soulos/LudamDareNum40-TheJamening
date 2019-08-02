using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unrandom = UnityEngine.Random;

namespace Assets.Scripts.Lib
{
    public static class Extensions
    {
        public static T RandomElement<T>(this ICollection<T> arr)
        {
            if (arr == null || arr.Count == 0)
            {
                return default(T);
            }
            else if (arr.Count == 1)
            {
                return arr.ElementAt(0);
            }

            return arr.ElementAt(Unrandom.Range(0, arr.Count));
        }
    }
}
