using DialogueDisplayFramework.Data;
using StardewValley;
using StardewValley.Menus;

namespace DialogueDisplayFramework.Framework
{
    public class DialogueDisplay
    {
        public DialogueDisplayData Data { get; private set; }
        public DialogueBox Box { get; private set; }
        public bool PreventGetCurrentString { get; set; }

        private bool? _isWearingIslandAttire;
        private Friendship _friendship;

        internal DialogueDisplay(DialogueBox dialogueBox)
        {
            Box = dialogueBox;
            Data = new DialogueDisplayData();
        }

        // Extracted from StardewValley.Menus.DialogueBox:shouldPortraitShake()
        public bool ShouldPortraitShake()
        {
            if (Box.newPortaitShakeTimer > 0)
                return true;

            var dialogue = Box.characterDialogue;
            var list = dialogue.speaker.GetData()?.ShakePortraits;
            if (list?.Count > 0)
            {
                return list.Contains(dialogue.getPortraitIndex());
            }

            return false;
        }

        public bool GetIsWearingIslandAttire()
        {
            if (!_isWearingIslandAttire.HasValue)
            {
                var npc = Box.characterDialogue.speaker;
                _isWearingIslandAttire = ModEntry.SHelper.Reflection.GetField<bool>(npc, "isWearingIslandAttire").GetValue();
            }

            return _isWearingIslandAttire.Value;
        }

        public Friendship GetSpeakerFriendship()
        {
            if (_friendship == null)
            {
                var npc = Box.characterDialogue.speaker;
                Game1.player.friendshipData.TryGetValue(npc.Name, out _friendship);
            }

            return _friendship;
        }
    }
}
