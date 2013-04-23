namespace BadSnowstorm
{
    public abstract class Input
    {
        protected Input(InputType inputType)
        {
            InputType = inputType;
        }

        public InputType InputType { get; private set; }
        public string Prompt { get; set; }
        public abstract string GetContent();
    }
}