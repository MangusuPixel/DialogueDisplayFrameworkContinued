using StardewValley;
using StardewValley.Delegates;

namespace DialogueDisplayFramework.Framework
{
    internal class DialogueGameStateQueries
    {
        internal static void Register()
        {
            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_NAME", (string[] query, GameStateQueryContext context) =>
            {
                if (!ArgUtility.TryGet(query, 1, out string name, out string error))
                {
                    return GameStateQuery.Helpers.ErrorResult(query, error);
                }
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.Name.ToString().ToLower() == name.ToLower();
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_GENDER", (string[] query, GameStateQueryContext context) =>
            {
                if (!ArgUtility.TryGet(query, 1, out string gender, out string error))
                {
                    return GameStateQuery.Helpers.ErrorResult(query, error);
                }
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.Gender.ToString().ToLower() == gender.ToLower();
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_CAN_BE_ROMANCED", (string[] query, GameStateQueryContext context) =>
            {
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.datable.Value == true;
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_CAN_SOCIALIZE", (string[] query, GameStateQueryContext context) =>
            {
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.CanSocialize == true;
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_CAN_RECEIVE_GIFTS", (string[] query, GameStateQueryContext context) =>
            {
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.CanReceiveGifts() == true;
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_APPEARANCE_ID", (string[] query, GameStateQueryContext context) =>
            {
                if (!ArgUtility.TryGet(query, 1, out string appearanceid, out string error))
                {
                    return GameStateQuery.Helpers.ErrorResult(query, error);
                }
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                NPC talkedNPC = Game1.getCharacterFromName(npcName);
                return talkedNPC?.LastAppearanceId?.ToLower() == appearanceid.ToLower();
            });

            GameStateQuery.Register("Mangupix.DDFC_SPEAKER_FRIENDSHIP_POINTS", (string[] query, GameStateQueryContext context) =>
            {
                if (!ArgUtility.TryGetInt(query, 1, out var minPoints, out string error, "int minPoints") || !ArgUtility.TryGetOptionalInt(query, 2, out var maxPoints, out error, int.MaxValue, "int maxPoints"))
                {
                    return GameStateQuery.Helpers.ErrorResult(query, error);
                }
                if (!Game1.player.modData.TryGetValue("Mangupix.DialogueDisplayFrameworkContinued_NPCTalked", out string npcName))
                    return false;

                int friendshipLevelForNPC = Game1.player.getFriendshipLevelForNPC(npcName);
                return friendshipLevelForNPC >= minPoints && friendshipLevelForNPC <= maxPoints;
            });
        }
    }
}
