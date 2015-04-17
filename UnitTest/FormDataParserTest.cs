using System;
using HostConsole;
using HostConsole.DTO;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class FormDataParserTest
    {
        private IFormDataParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new FormDataParser();
        }

        [Test]
        public void ActionParser_GiveNoAction_WhenActionIsNotDefined()
        {
           Assert.That(_parser.ActionParser(""), Is.EqualTo("NoAction"));
        }

        [Test]
        public void ActionParser_GiveNoAction_WhenActionIsNull()
        {
            Assert.That(_parser.ActionParser(null), Is.EqualTo("NoAction"));
        }

        [TestCase("'action=check'", "check")]
        [TestCase("error", "NoAction")]
        [TestCase("'action=version'", "version")]
        [TestCase("'action=bet_request&game_state={   \"players\":[     {       \"name\":\"Player 1\",       \"stack\":1000,       \"status\":\"active\",       \"bet\":0,       \"hole_cards\":[],       \"version\":\"Version name 1\",       \"id\":0     },     {       \"name\":\"Player 2\",       \"stack\":1000,       \"status\":\"active\",       \"bet\":0,       \"hole_cards\":[],       \"version\":\"Version name 2\",       \"id\":1     }   ],   \"tournament_id\":\"550d1d68cd7bd10003000003\",   \"game_id\":\"550da1cb2d909006e90004b1\",   \"round\":0,   \"bet_index\":0,   \"small_blind\":10,   \"orbits\":0,   \"dealer\":0,   \"community_cards\":[],   \"current_buy_in\":0,   \"pot\":0 }'", "bet_request")]
        [TestCase("'action=showdown&game_state={  \"players\":[    {      \"name\":\"Player 1\",      \"stack\":1000,      \"status\":\"active\",      \"bet\":0,      \"hole_cards\":[],      \"version\":\"Version name 1\",      \"id\":0    },    {      \"name\":\"Player 2\",      \"stack\":1000,      \"status\":\"active\",      \"bet\":0,      \"hole_cards\":[],      \"version\":\"Version name 2\",      \"id\":1    }  ],  \"tournament_id\":\"550d1d68cd7bd10003000003\",  \"game_id\":\"550da1cb2d909006e90004b1\",  \"round\":0,  \"bet_index\":0,  \"small_blind\":10,  \"orbits\":0,  \"dealer\":0,  \"community_cards\":[],  \"current_buy_in\":0,  \"pot\":0}'", "showdown")]
        public void ActionParser_GiveActionType_WhenActionIsNull(string input, string parserValue)
        {
            Assert.That(_parser.ActionParser(input), Is.EqualTo(parserValue));
        }

        [Test]
        public void GameStateParser_GivesbackValidGameState()
        {
            var input = "'action=bet_request&game_state={   \"players\":[     {       \"name\":\"Player 1\",       \"stack\":1000,       \"status\":\"active\",       \"bet\":1,       \"hole_cards\":[{\"rank\": \"5\",\"suit\": \"spades\"},{\"rank\": \"K\",\"suit\": \"hearts\"},{\"rank\": \"7\",\"suit\": \"clubs\"}],       \"version\":\"Version name 1\",       \"id\":2     },     {       \"name\":\"Player 2\",       \"stack\":2000,       \"status\":\"active\",       \"bet\":20,       \"hole_cards\":[{\"rank\": \"5\",\"suit\": \"spades\"},{\"rank\": \"K\",\"suit\": \"hearts\"},{\"rank\": \"7\",\"suit\": \"clubs\"}],       \"version\":\"Version name 2\",       \"id\":21     }   ],   \"small_blind\":10,   \"orbits\":1,   \"dealer\":2,   \"community_cards\": [{\"rank\": \"4\",\"suit\": \"spades\"},{\"rank\": \"A\",\"suit\": \"hearts\"},{\"rank\": \"6\",\"suit\": \"clubs\"}], \"current_buy_in\":3,   \"pot\":4 }'";
            var parseResult = _parser.GameStateParser(input);

            Assert.That(parseResult.small_blind, Is.EqualTo(10));
            Assert.That(parseResult.orbits, Is.EqualTo(1));
            Assert.That(parseResult.dealer, Is.EqualTo(2));
            Assert.That(parseResult.current_buy_in, Is.EqualTo(3));
            Assert.That(parseResult.pot, Is.EqualTo(4));

            CheckCards(parseResult.community_cards, "4,spades,A,hearts,6,clubs");

            CheckPlayers(parseResult.players, new[]
                                              {
                                                  "Player 1,1000,active,1,5;spades;K;hearts;7;clubs,Version name 1,2",
                                                  "Player 2,2000,active,20,5;spades;K;hearts;7;clubs,Version name 2,21",
                                              });
        }

        private void CheckPlayers(Player[] players, string[] expectedvalues)
        {
            Assert.That(players.Length, Is.EqualTo(expectedvalues.Length));

            for (int i = 0; i < expectedvalues.Length; i++)
            {
                CheckPlayer(players[i], expectedvalues[i]);
            }

        }

        private void CheckPlayer(Player player, string expectedValues)
        {
            var expectedItems = expectedValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.That(player.name, Is.EqualTo(expectedItems[PlayerProp.Name.Int()]));
            Assert.That(player.stack.ToString(), Is.EqualTo(expectedItems[PlayerProp.Stack.Int()]));
            Assert.That(player.status, Is.EqualTo(expectedItems[PlayerProp.Status.Int()]));
            Assert.That(player.bet.ToString(), Is.EqualTo(expectedItems[PlayerProp.Bet.Int()]));

            CheckCards(player.hole_cards, expectedItems[PlayerProp.HoleCard.Int()].Replace(';',','));

            Assert.That(player.version, Is.EqualTo(expectedItems[PlayerProp.Version.Int()]));
            Assert.That(player.id.ToString(), Is.EqualTo(expectedItems[PlayerProp.Id.Int()]));
        }

        private void CheckCards(Card[] cards, string expectedvalues)
        {
            var expectedItems = expectedvalues.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            Assert.That(cards.Length, Is.EqualTo(expectedItems.Length /2));

            for (int i = 0; i < cards.Length; i++)
            {
                Assert.That(cards[i].Rank, Is.EqualTo(expectedItems[2*i]));
                Assert.That(cards[i].Suit, Is.EqualTo(expectedItems[2*i+1]));
            }

        }
    }

    public enum PlayerProp
    {
        Name = 0,
        Stack,
        Status,
        Bet,
        HoleCard,
        Version,
        Id
    }

    public static class EnumExtension
    {
        public static int Int(this PlayerProp enumValue)
        {
            return (int) enumValue;
        }
    }
}
