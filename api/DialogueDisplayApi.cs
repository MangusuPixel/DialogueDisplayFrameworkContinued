using DialogueDisplayFramework.Api;
using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework.Api
{
    public class DialogueDisplayApi
    {
        public DialogueBoxDrawEvents Events;

        private static DialogueDisplayApi _instance;

        public static DialogueDisplayApi Instance => _instance ??= new DialogueDisplayApi();

        public DialogueDisplayData DisplayData { get; internal set; }

        private DialogueDisplayApi()
        {
            Events = new DialogueBoxDrawEvents();
        }
        public void OnRenderingDialogueBox(ModEntry sender, Action<SpriteBatch, DialogueBox, IDialogueDisplayData> callback)
        {
            Events.RenderingDialogueBox.Add(sender, callback);
        }
        public void OnRenderedDialogueBox(ModEntry sender, Action<SpriteBatch, DialogueBox, IDialogueDisplayData> callback)
        {
            Events.RenderedDialogueBox.Add(sender, callback);
        }
        public void OnRenderingDialogueString(ModEntry sender, Action<SpriteBatch, DialogueBox, IDialogueStringData> callback)
        {
            Events.RenderingDialogueString.Add(sender, callback);
        }
        public void OnRenderedDialogueString(ModEntry sender, Action<SpriteBatch, DialogueBox, IDialogueStringData> callback)
        {
            Events.RenderedDialogueString.Add(sender, callback);
        }

        public void OnRenderingPortrait(ModEntry sender, Action<SpriteBatch, DialogueBox, IPortraitData> callback)
        {
            Events.RenderingPortrait.Add(sender, callback);
        }
        public void OnRenderedPortrait(ModEntry sender, Action<SpriteBatch, DialogueBox, IPortraitData> callback)
        {
            Events.RenderedPortrait.Add(sender, callback);
        }

        public void OnRenderingJewel(ModEntry sender, Action<SpriteBatch, DialogueBox, IBaseData> callback)
        {
            Events.RenderingJewel.Add(sender, callback);
        }
        public void OnRenderedJewel(ModEntry sender, Action<SpriteBatch, DialogueBox, IBaseData> callback)
        {
            Events.RenderedJewel.Add(sender, callback);
        }

        public void OnRenderingButton(ModEntry sender, Action<SpriteBatch, DialogueBox, IBaseData> callback)
        {
            Events.RenderingButton.Add(sender, callback);
        }
        public void OnRenderedButton(ModEntry sender, Action<SpriteBatch, DialogueBox, IBaseData> callback)
        {
            Events.RenderedButton.Add(sender, callback);
        }

        public void OnRenderingGifts(ModEntry sender, Action<SpriteBatch, DialogueBox, IGiftsData> callback)
        {
            Events.RenderingGifts.Add(sender, callback);
        }
        public void OnRenderedGifts(ModEntry sender, Action<SpriteBatch, DialogueBox, IGiftsData> callback)
        {
            Events.RenderedGifts.Add(sender, callback);
        }

        public void OnRenderingHearts(ModEntry sender, Action<SpriteBatch, DialogueBox, IHeartsData> callback)
        {
            Events.RenderingHearts.Add(sender, callback);
        }
        public void OnRenderedHearts(ModEntry sender, Action<SpriteBatch, DialogueBox, IHeartsData> callback)
        {
            Events.RenderedHearts.Add(sender, callback);
        }

        public void OnRenderingImage(ModEntry sender, Action<SpriteBatch, DialogueBox, IImageData> callback)
        {
            Events.RenderingImage.Add(sender, callback);
        }
        public void OnRenderedImage(ModEntry sender, Action<SpriteBatch, DialogueBox, IImageData> callback)
        {
            Events.RenderedImage.Add(sender, callback);
        }

        public void OnRenderingText(ModEntry sender, Action<SpriteBatch, DialogueBox, ITextData> callback)
        {
            Events.RenderingText.Add(sender, callback);
        }
        public void OnRenderedText(ModEntry sender, Action<SpriteBatch, DialogueBox, ITextData> callback)
        {
            Events.RenderedText.Add(sender, callback);
        }

        public void OnRenderingDivider(ModEntry sender, Action<SpriteBatch, DialogueBox, IDividerData> callback)
        {
            Events.RenderingDivider.Add(sender, callback);
        }
        public void OnRenderedDivider(ModEntry sender, Action<SpriteBatch, DialogueBox, IDividerData> callback)
        {
            Events.RenderedDivider.Add(sender, callback);
        }
    }
}
