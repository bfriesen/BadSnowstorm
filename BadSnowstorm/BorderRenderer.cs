using System;
using System.Collections.Generic;
using System.Linq;

namespace BadSnowstorm
{
    public static class BorderRenderer
    {
        public static void Render(IConsole console, IList<IContentArea> contentAreas)
        {
            var mergedBorderRenderOverride =
                contentAreas
                    .Where(contentArea => contentArea.BorderRenderOverride != null)
                    .Aggregate((Func<Point, BorderInfo, bool>)((location, borderInfo) => false),
                        (accumulatedFunc, contentArea) =>
                            (point, borderInfo) =>
                                accumulatedFunc(point, borderInfo) || contentArea.BorderRenderOverride(point, borderInfo));

            foreach (var borderCharacter in contentAreas
                .Select(contentArea => contentArea.Border.GetBorderCharacters(contentArea.Bounds))
                .Merge(mergedBorderRenderOverride)
                .Where(borderCharacter => 
                    borderCharacter.Location.Left >= 0
                    && borderCharacter.Location.Left < console.WindowWidth
                    && borderCharacter.Location.Top >= 0
                    && borderCharacter.Location.Top < console.WindowHeight))
            {
                console.SetCursorPosition(borderCharacter.Location.Left, borderCharacter.Location.Top);
                console.Write(borderCharacter.GetValue());
            }
        }
    }
}