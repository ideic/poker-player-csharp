﻿using System;

namespace HostConsole.PlayerResponse
{
    internal class BetRequest : PlayerResponseBase
    {
        public override string MyAction
        {
            get { return "bet_request"; }
        }

        protected override string DoAction()
        {
            //new FormDataParser().GameStateParser(Data);
            return MyAction;
        }
    }
}