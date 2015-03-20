using System;

namespace HostConsole.PlayerResponse
{
    public class CheckResponse : PlayerResponseBase
    {
        public override string MyAction
        {
            get { return "check"; }
        }

        protected override string DoAction()
        {
            return "TeamName";
        }
    }
}