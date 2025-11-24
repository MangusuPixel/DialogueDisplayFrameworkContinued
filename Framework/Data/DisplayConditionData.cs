using StardewValley;

namespace DialogueDisplayFramework.Framework.Data
{
    public class DisplayConditionData
    {
        public bool Disabled = false;
        public string Query;

        // NPC data
        public string Speaker;
        public Gender Gender;
        public bool? CanSocialize;
        public bool? CanBeRomanced;
        public bool CanReceiveGifts;

        // Player friendship data
        public bool? IsRomancingPlayer;
        public bool? CanReceiveGift;
        public int? GiftsReceivedThisWeek;

        // Appearance data
        public string AppearanceId;
        public bool? IsIndoorsAttire;
        public bool? IsOutdoorsAttire;
        public bool? IsIslandAttire;
    }
}
