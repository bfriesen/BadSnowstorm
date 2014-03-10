using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadSnowstorm
{
    internal static class IContentAreaExtensions
    {
        internal static IEnumerable<IContentArea> GetDescendants(this IContentArea contentArea)
        {
            foreach (var child in contentArea.Children.Where(x => x != null))
            {
                yield return child;

                foreach (var descendant in GetDescendants(child))
                {
                    yield return descendant;
                }
            }
        }

        internal static IEnumerable<IContentArea> GetSelfAndDescendants(this IContentArea contentArea)
        {
            yield return contentArea;

            foreach (var descendant in GetDescendants(contentArea))
            {
                yield return descendant;
            }
        }
    }
}
