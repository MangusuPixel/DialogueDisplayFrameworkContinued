using DialogueDisplayFramework.Framework;
using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DialogueDisplayFramework.Api
{
    internal class ApiConsumerManager
    {
        internal static List<DialogueDisplayApi> ApiConsumers = new();

        internal static void RegisterApiConsumer(DialogueDisplayApi api)
        {
            ApiConsumers.Add(api);
        }

        internal static void RaiseRenderingDialogueBox(SpriteBatch b, DialogueDisplay display, DialogueDisplayData data)
        {
            var args = new RenderEventArgs<IDialogueDisplayData>(b, display, data.GetAdapter());
            ApiConsumers.ForEach(c => c.OnRaiseRenderingDialogueBox(args));
        }

        internal static void RaiseRenderedDialogueBox(SpriteBatch b, DialogueDisplay display, IDialogueDisplayData data)
        {
            var args = new RenderEventArgs<IDialogueDisplayData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedDialogueBox(args));
        }

        internal static void RaiseRenderingDialogueString(SpriteBatch b, DialogueDisplay display, IDialogueStringData data)
        {
            var args = new RenderEventArgs<IDialogueStringData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingDialogueString(args));
        }

        internal static void RaiseRenderedDialogueString(SpriteBatch b, DialogueDisplay display, IDialogueStringData data)
        {
            var args = new RenderEventArgs<IDialogueStringData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedDialogueString(args));
        }

        internal static void RaiseRenderingPortrait(SpriteBatch b, DialogueDisplay display, IPortraitData data)
        {
            var args = new RenderEventArgs<IPortraitData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingPortrait(args));
        }

        internal static void RaiseRenderedPortrait(SpriteBatch b, DialogueDisplay display, IPortraitData data)
        {
            var args = new RenderEventArgs<IPortraitData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedPortrait(args));
        }

        internal static void RaiseRenderingJewel(SpriteBatch b, DialogueDisplay display, IBaseData data)
        {
            var args = new RenderEventArgs<IBaseData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingJewel(args));
        }

        internal static void RaiseRenderedJewel(SpriteBatch b, DialogueDisplay display, IBaseData data)
        {
            var args = new RenderEventArgs<IBaseData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedJewel(args));
        }

        internal static void RaiseRenderingButton(SpriteBatch b, DialogueDisplay display, IBaseData data)
        {
            var args = new RenderEventArgs<IBaseData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingButton(args));
        }

        internal static void RaiseRenderedButton(SpriteBatch b, DialogueDisplay display, IBaseData data)
        {
            var args = new RenderEventArgs<IBaseData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedButton(args));
        }

        internal static void RaiseRenderingGifts(SpriteBatch b, DialogueDisplay display, IGiftsData data)
        {
            var args = new RenderEventArgs<IGiftsData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingGifts(args));
        }

        internal static void RaiseRenderedGifts(SpriteBatch b, DialogueDisplay display, IGiftsData data)
        {
            var args = new RenderEventArgs<IGiftsData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedGifts(args));
        }

        internal static void RaiseRenderingHearts(SpriteBatch b, DialogueDisplay display, IHeartsData data)
        {
            var args = new RenderEventArgs<IHeartsData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingHearts(args));
        }

        internal static void RaiseRenderedHearts(SpriteBatch b, DialogueDisplay display, IHeartsData data)
        {
            var args = new RenderEventArgs<IHeartsData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedHearts(args));
        }

        internal static void RaiseRenderingImage(SpriteBatch b, DialogueDisplay display, IImageData data)
        {
            var args = new RenderEventArgs<IImageData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingImage(args));
        }

        internal static void RaiseRenderedImage(SpriteBatch b, DialogueDisplay display, IImageData data)
        {
            var args = new RenderEventArgs<IImageData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedImage(args));
        }

        internal static void RaiseRenderingText(SpriteBatch b, DialogueDisplay display, ITextData data)
        {
            var args = new RenderEventArgs<ITextData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingText(args));
        }

        internal static void RaiseRenderedText(SpriteBatch b, DialogueDisplay display, ITextData data)
        {
            var args = new RenderEventArgs<ITextData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedText(args));
        }

        internal static void RaiseRenderingDivider(SpriteBatch b, DialogueDisplay display, IDividerData data)
        {
            var args = new RenderEventArgs<IDividerData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderingDivider(args));
        }

        internal static void RaiseRenderedDivider(SpriteBatch b, DialogueDisplay display, IDividerData data)
        {
            var args = new RenderEventArgs<IDividerData>(b, display, data);
            ApiConsumers.ForEach(c => c.OnRaiseRenderedDivider(args));
        }
    }
}
