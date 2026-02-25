using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.TokenizableStrings;
using System.Collections.Generic;

namespace DialogueDisplayFramework.Framework
{
    public class DialogueDisplay
    {
        private bool? _isWearingIslandAttire;
        private Friendship _friendship;
        private IAssetName _portraitAssetName;
        private Texture2D _portraitTexture;
        private Dictionary<string, Texture2D> _textures = new();

        public DialogueDisplayData Data { get; private set; }
        public DialogueBox Box { get; private set; }
        public bool PreventGetCurrentString { get; set; }

        public NPC Speaker => Box.characterDialogue.speaker;

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

        public string GetSpeakerPortraitAssetPath()
        {
            if (_portraitAssetName == null)
            {
                var portrait = Speaker.Portrait.Name;
                _portraitAssetName = ModEntry.SHelper.GameContent.ParseAssetName(portrait);
            }

            return _portraitAssetName.BaseName;
        }

        public Texture2D GetPortraitTexture()
        {
            if (_portraitTexture == null)
            {
                if (!string.IsNullOrEmpty(Data.Portrait.TexturePath))
                {
                    var parsedPath = TokenParser.ParseText(Data.Portrait.TexturePath);
                    _portraitTexture = Game1.content.Load<Texture2D>(parsedPath);
                }

                // vanilla default behaviour
                _portraitTexture ??= 
                    Box.characterDialogue.overridePortrait ??
                    Box.characterDialogue.speaker.Portrait;
            }

            return _portraitTexture;
        }

        public bool TryGetImageTexture(ImageData image, out Texture2D texture)
        {
            if (image.HasTokenizedPath() == false && image.TryGetTexture(out texture))
            {
                return true;
            }
            else if (!_textures.TryGetValue(image.TexturePath, out texture))
            {
                var parsedPath = TokenParser.ParseText(image.TexturePath);
                texture = Game1.content.Load<Texture2D>(parsedPath);
                _textures[image.TexturePath] = texture;
            }

            return texture != null;
        }
    }
}
