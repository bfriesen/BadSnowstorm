namespace BadSnowstorm
{
    public abstract class Controller
    {
        protected readonly Actions _actions;

        protected Controller()
        {
            _actions = new Actions(this);
        }

        protected Actions Actions
        {
            get { return _actions; }
        }

        public abstract ViewModel Index();
    }
}