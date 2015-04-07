namespace HostConsole.DTO
{
    public class GameState
    {
        public Player[] players { get; set; }

        public int small_blind { get; set; }

        public int orbits { get; set; }

        public int dealer { get; set; }

        public Card[] community_cards { get; set; }

        public int current_buy_in { get; set; }

        public int pot { get; set; }
    }
}