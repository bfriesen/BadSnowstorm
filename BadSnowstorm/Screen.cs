using System;

namespace BadSnowstorm
{
    public abstract class Screen : ContentArea, IShowable
    {
        internal Screen()
        {
            Pause = TimeSpan.Zero;
        }

        protected Screen(string name)
            : base(name)
        {
            Pause = TimeSpan.Zero;
        }

        public Song Song { get; protected set; }

        public TimeSpan Pause { get; protected set; }

        public virtual void Show(IConsole console)
        {
            RenderContent(console);
        }
    }
}