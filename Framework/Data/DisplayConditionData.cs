using StardewValley;

namespace DialogueDisplayFramework.Data
{
    public class DisplayConditionData
    {
        public bool Disabled { get; set; } = false;
        public string Query { get; set; }

        // Environment
        public bool? IsAndroid { get; set; }
        public int? BoxWidthEquals { get; set; }
        public int? BoxWidthOver { get; set; }
        public int? BoxWidthUnder { get; set; }
        public string Location { get; set; }

        // NPC data
        public string Speaker { get; set; }
        public Gender? Gender { get; set; }
        public int? Age { get; set; }
        public bool? CanSocialize { get; set; }
        public bool? CanBeRomanced { get; set; }
        public bool? CanReceiveGifts { get; set; }
        public bool? IsBirthdayToday { get; set; }
        public bool? IsEventActor { get; set; }
        public string AppearanceId { get; set; }
        public bool? IsIslandAttire { get; set; }

        // Relationship data
        public bool? IsDatingPlayer { get; set; }
        public bool? GiftReceivedToday { get; set; }
        public int? GiftsReceivedThisWeek { get; set; }
        public int? FriendshipPointsEquals { get; set; }
        public int? FriendshipPointsOver { get; set; }
        public int? FriendshipPointsUnder { get; set; }
    }
}
