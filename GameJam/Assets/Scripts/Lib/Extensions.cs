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
        public static T RandomElement<T>(this T[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                return default(T);
            }
            else if (arr.Length == 1)
            {
                return arr[0];
            }

            return arr[Unrandom.Range(0, arr.Length)];
        }
    }
}
