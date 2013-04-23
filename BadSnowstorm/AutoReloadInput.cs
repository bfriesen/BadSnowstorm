using System;
using System.Threading;

namespace BadSnowstorm
{
    public class AutoReloadInput : Input, IAcceptsInput
    {
        private readonly string _initialInputContent;
        private readonly Func<IActionResult> _getActionResult;

        public AutoReloadInput(Controller currentController)
            : this(currentController, "", 0)
        {
        }

        public AutoReloadInput(Controller currentController, string initialInputContent, int delayMilliseconds)
            : base(InputType.AutoReload)
        {
            _initialInputContent = initialInputContent;
            _getActionResult = () =>
            {
                Thread.Sleep(delayMilliseconds); // TODO: extract Thread.Sleep to interface.
                return new Reload(currentController);
            };
        }

        public override string GetContent()
        {
            return _initialInputContent;
        }

        public Action OnInputAccepted { get; set; }

        public Func<IActionResult> ActionResult
        {
            get { return _getActionResult; }
        }
    }
}
