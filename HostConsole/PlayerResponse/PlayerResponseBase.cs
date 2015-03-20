namespace HostConsole.PlayerResponse
{
    public abstract class PlayerResponseBase : IPlayerResponse
    {
        private IPlayerResponse _next;

        public IPlayerResponse SetNext(IPlayerResponse nextResponse)
        {
            _next = nextResponse;

            return _next;
        }

        public string GetResponse(string action, string data)
        {
            if (action == MyAction)
            {
                return DoAction();
            }
            if (_next != null)
            {
                return _next.GetResponse(action, data);
            }

            return "NoHandler";
        }

        public abstract string MyAction { get;}

        protected abstract string DoAction();
    }
}
