using System;

namespace BadSnowstorm
{
    public class TextInput : Input, IAcceptsInput
    {
        private Action _onInputAccepted;

        public TextInput(Func<IActionResult> textEnteredResult)
            : base(InputType.Text)
        {
            ActionResult = () => Value == null ? new GoBack() : textEnteredResult();
        }

        public string Value { get; set; }
        
        public Action OnInputAccepted
        {
            get
            {
                return _onInputAccepted;
            }
            set
            {
                _onInputAccepted = value ?? (Action)(() => {});
            }
        }

        public Func<IActionResult> ActionResult { get; private set; }

        public override string GetContent()
        {
            return Prompt;
        }
    }
}