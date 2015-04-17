using System;

namespace HostConsole.PlayerResponse
{
    internal class ShowDown : PlayerResponseBase
    {
        public override string MyAction
        {
            get { return "showdown"; }
        }

        protected override string DoAction()
        {
            return MyAction;
        }
    }
}