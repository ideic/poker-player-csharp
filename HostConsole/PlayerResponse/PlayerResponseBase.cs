namespace HostConsole.PlayerResponse
{
    internal abstract class PlayerResponseBase : IPlayerResponse
    {
        private IPlayerResponse _next;

        public IPlayerResponse SetNext(IPlayerResponse nextResponse)
        {
            _next = nextResponse;

            return _next;
        }

        public string GetResponse(string action, string data)
        {
            if (CanHandle(action))
            {
                return DoAction();
            }
            if (_next != null)
            {
                return _next.GetResponse(action, data);
            }

            return "NoHandler";
        }

        protected abstract string DoAction();

        protected abstract bool CanHandle(string action);
    }
}
