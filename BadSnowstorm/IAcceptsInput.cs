using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadSnowstorm
{
    public interface IAcceptsInput
    {
        Action OnInputAccepted { get; }
        Func<IActionResult> ActionResult { get; }
    }
}
