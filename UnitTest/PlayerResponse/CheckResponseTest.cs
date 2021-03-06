﻿using HostConsole.PlayerResponse;
using NUnit.Framework;

namespace UnitTest.PlayerResponse
{
    [TestFixture]
    public class CheckResponseTest
    {
        private IPlayerResponse _response;

        [SetUp]
        public void SetUp()
        {
            _response = new CheckResponse();
        }

        [Test]
        public void GetResponseReturnsTeamName_WhenActionIsCheck()
        {
            Assert.That(_response.GetResponse("check", ""), Is.EqualTo("TeamName"));
        }

        [Test]
        public void GetResponseReturnsNoAction_WhenActionIsNotCheck()
        {
            Assert.That(_response.GetResponse("check2", ""), Is.EqualTo("NoHandler"));
        }

    }
}
