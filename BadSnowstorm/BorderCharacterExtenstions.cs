using System;
using System.Collections.Generic;
using System.Linq;

namespace BadSnowstorm
{
    public static class BorderCharacterExtenstions
    {
        public static IEnumerable<IBorderCharacter> Merge(this IEnumerable<IEnumerable<IBorderCharacter>> borderCharacters, Func<Point, BorderInfo, bool> excludeMergePredicate)
        {
            return Merge(borderCharacters.SelectMany(x => x), excludeMergePredicate);
        }

        public static IEnumerable<IBorderCharacter> Merge(this IEnumerable<IBorderCharacter> borderCharacters, Func<Point, BorderInfo, bool> excludeMergePredicate)
        {
            if (excludeMergePredicate == null)
            {
                excludeMergePredicate = (point, info) => false;
            }

            var all = borderCharacters.ToList();

            for (int i = 0; i < all.Count; i++)
            {
                var lhs = all[i] as BorderCharacter;

                if (lhs != null)
                {
                    if (!excludeMergePredicate(lhs.Location, lhs.BorderInfo))
                    {
                        for (int j = i + 1; j < all.Count; j++)
                        {
                            var rhs = all[j] as BorderCharacter;

                            if (rhs != null)
                            {
                                if (lhs.Location == rhs.Location)
                                {
                                    if (!excludeMergePredicate(rhs.Location, rhs.BorderInfo))
                                    {
                                        lhs.MergeWith(rhs);
                                    }

                                    all.RemoveAt(j--);
                                }
                            }
                        }

                        yield return lhs;
                    }
                }
            }
        }
    }
}