namespace Pokermon.Core.Model
{
    public class Player
    {
        public string Nick { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsAllIn { get; set; }
        public int CurrentCash { get; set; }
        public int CurrentBet { get; set; }
    }
}
