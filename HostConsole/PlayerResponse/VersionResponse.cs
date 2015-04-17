using System;

namespace HostConsole.PlayerResponse
{
    internal class VersionResponse : PlayerResponseBase
    {
        public override string MyAction
        {
            get { return "version"; }
        }

        protected override string DoAction()
        {
            return MyAction;
        }
    }
}