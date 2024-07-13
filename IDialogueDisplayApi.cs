using DialogueDisplayFramework.Data;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace DialogueDisplayFramework
{
    public delegate void DialogueBoxRenderDelegate(SpriteBatch b, DialogueBox box, IDialogueDisplayData data);

    internal interface IDialogueDisplayApi
    {
        public DialogueDisplayData CurrentDisplayData { get; set; }

        public DialogueBoxRenderDelegate OnRenderingDialogueBox { get; set; }

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
    }
    public interface IDialogueDisplayData
    {
        public string CopyFrom { get; set; }
        public string PackName { get; set; }
        public int? XOffset { get; set; }
        public int? YOffset { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DialogueStringData Dialogue { get; set; }
        public PortraitData Portrait { get; set; }
        public TextData Name { get; set; }
        public BaseData Jewel { get; set; }
        public BaseData Button { get; set; }
        public GiftsData Gifts { get; set; }
        public HeartsData Hearts { get; set; }
        public List<ImageData> Images { get; set; }
        public List<TextData> Texts { get; set; }
        public List<DividerData> Dividers { get; set; }
        public bool Disabled { get; set; }
    }

    public interface IBaseData
    {
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public bool Right { get; set; }
        public bool Bottom { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Alpha { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }
        public bool Disabled { get; set; }
    }

    public interface IDialogueStringData : IBaseData
    {
        public string Color { get; set; }
        public SpriteText.ScrollTextAlignment Alignment { get; set; }
    }

    public interface IPortraitData : IBaseData
    {
        public string TexturePath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public bool TileSheet { get; set; }
    }

    public interface IGiftsData : IBaseData
    {
        public bool ShowGiftIcon { get; set; }
        public bool Inline { get; set; }
    }

    public interface IHeartsData : IBaseData
    {
        public int HeartsPerRow { get; set; }
        public bool ShowEmptyHearts { get; set; }
        public bool ShowPartialhearts { get; set; }
        public bool Centered { get; set; }
    }

    public interface IImageData : IBaseData
    {
        public string ID { get; set; }
        public string TexturePath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }

    public interface ITextData : IBaseData
    {
        public string ID { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }
        public bool Junimo { get; set; }
        public bool Scroll { get; set; }
        public string PlaceholderText { get; set; }
        public int ScrollType { get; set; }
        public SpriteText.ScrollTextAlignment Alignment { get; set; }
    }

    public interface IDividerData : IBaseData
    {
        public string ID { get; set; }
        public bool Horizontal { get; set; }
        public bool Small { get; set; }
        public DividerConnectorData Connectors { get; set; }
        public string Color { get; set; }
    }
    public interface IDividerConnectorData
    {
        public bool Top { get; set; }
        public bool Bottom { get; set; }
    }
}
