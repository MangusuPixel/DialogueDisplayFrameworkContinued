using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace DialogueDisplayFramework.Framework
{
    internal class ApiConsumerManager
    {
        internal static List<DialogueDisplayApi> ApiConsumers = new();

        internal static void RegisterApiConsumer(DialogueDisplayApi api)
        {
            ApiConsumers.Add(api);
        }

        internal static void SetCurrentData(DialogueDisplayData data)
        {
            ApiConsumers.ForEach(c => c.CurrentDisplayData = data);
        }

        internal static void InvokeRenderingDialogueBox(SpriteBatch b, DialogueBox box, IDialogueDisplayData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingDialogueBox, b, box, data));
        }

        internal static void InvokeRenderedDialogueBox(SpriteBatch b, DialogueBox box, IDialogueDisplayData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedDialogueBox, b, box, data));
        }

        internal static void InvokeRenderingDialogueString(SpriteBatch b, DialogueBox box, IDialogueStringData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingDialogueString, b, box, data));
        }

        internal static void InvokeRenderedDialogueString(SpriteBatch b, DialogueBox box, IDialogueStringData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedDialogueString, b, box, data));
        }

        internal static void InvokeRenderingPortrait(SpriteBatch b, DialogueBox box, IPortraitData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingPortrait, b, box, data));
        }

        internal static void InvokeRenderedPortrait(SpriteBatch b, DialogueBox box, IPortraitData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedPortrait, b, box, data));
        }

        internal static void InvokeRenderingJewel(SpriteBatch b, DialogueBox box, IBaseData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingJewel, b, box, data));
        }

        internal static void InvokeRenderedJewel(SpriteBatch b, DialogueBox box, IBaseData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedJewel, b, box, data));
        }

        internal static void InvokeRenderingButton(SpriteBatch b, DialogueBox box, IBaseData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingButton, b, box, data));
        }

        internal static void InvokeRenderedButton(SpriteBatch b, DialogueBox box, IBaseData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedButton, b, box, data));
        }

        internal static void InvokeRenderingGifts(SpriteBatch b, DialogueBox box, IGiftsData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingGifts, b, box, data));
        }

        internal static void InvokeRenderedGifts(SpriteBatch b, DialogueBox box, IGiftsData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedGifts, b, box, data));
        }

        internal static void InvokeRenderingHearts(SpriteBatch b, DialogueBox box, IHeartsData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingHearts, b, box, data));
        }

        internal static void InvokeRenderedHearts(SpriteBatch b, DialogueBox box, IHeartsData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedHearts, b, box, data));
        }

        internal static void InvokeRenderingImage(SpriteBatch b, DialogueBox box, IImageData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingImage, b, box, data));
        }

        internal static void InvokeRenderedImage(SpriteBatch b, DialogueBox box, IImageData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedImage, b, box, data));
        }

        internal static void InvokeRenderingText(SpriteBatch b, DialogueBox box, ITextData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingText, b, box, data));
        }

        internal static void InvokeRenderedText(SpriteBatch b, DialogueBox box, ITextData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedText, b, box, data));
        }

        internal static void InvokeRenderingDivider(SpriteBatch b, DialogueBox box, IDividerData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderingDivider, b, box, data));
        }

        internal static void InvokeRenderedDivider(SpriteBatch b, DialogueBox box, IDividerData data)
        {
            ApiConsumers.ForEach(c => InvokeCallback(c, c.OnRenderedDivider, b, box, data));
        }

        internal static void InvokeCallback<T>(DialogueDisplayApi consumer, Action<SpriteBatch, DialogueBox, T> callback, SpriteBatch b, DialogueBox box, T data)
        {
            try
            {
                callback(b, box, data);
            }
            catch (Exception ex)
            {
                ModEntry.SMonitor.LogOnce($"{consumer.ModManifest.Name} is crashing, please report the following error to them.\n[{consumer.ModManifest.Name}] {ex}", LogLevel.Error);
            }
        }

        internal static void InvokeCallback<T>(DialogueDisplayApi consumer, Delegate callback, SpriteBatch b, DialogueBox box, T data)
        {
            try
            {
                callback.DynamicInvoke(b, box, data);
            }
            catch (Exception ex)
            {
                ModEntry.SMonitor.LogOnce($"{consumer.ModManifest.Name} is crashing, please report the following error to them.\n[{consumer.ModManifest.Name}] {ex}", LogLevel.Error);
            }
        }
    }
}
