using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MichaelsPlace.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Makes <paramref name="this"/> collection contain the items from <paramref name="source"/>,
        /// by removing and adding items as needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="source"></param>
        /// <returns><paramref name="this"/></returns>
        public static ICollection<T> MirrorFrom<T>([NotNull] this ICollection<T> @this, [NotNull] ICollection<T> source)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var toRemove = @this.Except(source);
            var toAdd = source.Except(@this);
            foreach (var item in toRemove)
            {
                @this.Remove(item);
            }
            foreach (var item in toAdd)
            {
                @this.Add(item);
            }
            return @this;
        }
    }
}
