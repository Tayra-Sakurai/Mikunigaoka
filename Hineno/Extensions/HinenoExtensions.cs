using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hineno.Extensions
{
    public static class HinenoExtensions
    {
        /// <summary>
        /// Calculates the inner (dot) product of two arrays of numeric values.
        /// </summary>
        /// <remarks>This method computes the sum of the products of corresponding elements from the two
        /// arrays. If either array is null, an ArgumentNullException is thrown. The calculation stops at the end of the
        /// shorter array if the arrays are of unequal length.</remarks>
        /// <typeparam name="T">The numeric type of the array elements. Must implement INumberBase<T>.</typeparam>
        /// <param name="left">The first array of numeric values to use in the inner product calculation. Cannot be null.</param>
        /// <param name="right">The second array of numeric values to use in the inner product calculation. Cannot be null.</param>
        /// <returns>The inner product of the two arrays, computed as the sum of the products of corresponding elements. If the
        /// arrays have different lengths, only pairs up to the length of the shorter array are included.</returns>
        /// <exception cref="ArgumentNullException">One or more parameters are null.</exception>
        public static T GetInnerProduct<T>(this T[] left, T[] right)
            where T : INumberBase<T>
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            T result = T.Zero;

            foreach ((T lelm, T relm) in left.Zip(right))
                result += lelm * relm;

            return result;
        }
    }
}
