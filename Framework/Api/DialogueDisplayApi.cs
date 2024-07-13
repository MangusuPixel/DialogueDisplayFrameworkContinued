using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework.Framework
{
    public class DialogueDisplayApi : IDialogueDisplayApi
    {
        public IManifest ModManifest;

        public DialogueBoxRenderDelegate OnRenderingDialogueBox { get; set; }

        public DialogueDisplayData CurrentDisplayData { get; set; }
        //public Action<SpriteBatch, DialogueBox, IDialogueDisplayData> OnRenderingDialogueBox { get; set; }
        public Action<SpriteBatch, DialogueBox, IDialogueDisplayData> OnRenderedDialogueBox { get; set; }
        public Action<SpriteBatch, DialogueBox, IDialogueStringData> OnRenderingDialogueString { get; set; }
        public Action<SpriteBatch, DialogueBox, IDialogueStringData> OnRenderedDialogueString { get; set; }
        public Action<SpriteBatch, DialogueBox, IPortraitData> OnRenderingPortrait { get; set; }
        public Action<SpriteBatch, DialogueBox, IPortraitData> OnRenderedPortrait { get; set; }
        public Action<SpriteBatch, DialogueBox, IBaseData> OnRenderingJewel { get; set; }
        public Action<SpriteBatch, DialogueBox, IBaseData> OnRenderedJewel { get; set; }
        public Action<SpriteBatch, DialogueBox, IBaseData> OnRenderingButton { get; set; }
        public Action<SpriteBatch, DialogueBox, IBaseData> OnRenderedButton { get; set; }
        public Action<SpriteBatch, DialogueBox, IGiftsData> OnRenderingGifts { get; set; }
        public Action<SpriteBatch, DialogueBox, IGiftsData> OnRenderedGifts { get; set; }
        public Action<SpriteBatch, DialogueBox, IHeartsData> OnRenderingHearts { get; set; }
        public Action<SpriteBatch, DialogueBox, IHeartsData> OnRenderedHearts { get; set; }
        public Action<SpriteBatch, DialogueBox, IImageData> OnRenderingImage { get; set; }
        public Action<SpriteBatch, DialogueBox, IImageData> OnRenderedImage { get; set; }
        public Action<SpriteBatch, DialogueBox, ITextData> OnRenderingText { get; set; }
        public Action<SpriteBatch, DialogueBox, ITextData> OnRenderedText { get; set; }
        public Action<SpriteBatch, DialogueBox, IDividerData> OnRenderingDivider { get; set; }
        public Action<SpriteBatch, DialogueBox, IDividerData> OnRenderedDivider { get; set; }

        public DialogueDisplayApi(IManifest mod)
        {
            ModManifest = mod;
        }

        public static DialogueDisplayData GetCharacterDisplay(string name)
        {
            return DialogueBoxInterface.GetSpecialDisplay(name);
        }
    }
}
