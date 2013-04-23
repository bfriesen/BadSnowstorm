using System;

namespace BadSnowstorm
{
    public class MenuItem : IAcceptsInput
    {
        public string Text { get; set; }
        public char Id { get; set; }
        public Action OnInputAccepted { get; set; }
        public Func<IActionResult> ActionResult { get; set; }
    }
}