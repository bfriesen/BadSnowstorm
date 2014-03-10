using System;
using System.Collections.Generic;

namespace BadSnowstorm
{
    public interface IContentArea
    {
        Border Border { get; set; }
        Func<Point, BorderInfo, bool> BorderRenderOverride { get; set; }
        Rectangle Bounds { get; set; }
        List<IContentArea> Children { get; }
        void ClearContent(IConsole console);
        string Content { get; set; }
        ContentAlignment ContentAlignment { get; set; }
        ContentType ContentType { get; set; }
        Point LastCursorLocation { get; }
        string Name { get; }
        Padding Padding { get; set; }
        void RenderContent(IConsole console);
    }
}
