using StardewValley.BellsAndWhistles;
using System.Collections.Generic;
using System.Linq;

namespace DialogueDisplayFramework
{
    public class DialogueDisplayData
    {
        public static readonly string MISSING_ID_STR = "MISSING_ID";

        public string copyFrom;
        public string packName;
        public int? xOffset;
        public int? yOffset;
        public int? width;
        public int? height;
        public DialogueData dialogue;
        public PortraitData portrait;
        public TextData name;
        public BaseData jewel;
        public BaseData button;
        //public SpriteData sprite;
        public GiftsData gifts;
        public HeartsData hearts;
        public List<ImageData> images = new();
        public List<TextData> texts = new();
        public List<DividerData> dividers = new();
        public bool disabled;

        public static DialogueDisplayData DefaultValues => new()
        {
            width = 1200,
            height = 384,
            dialogue = new()
            {
                xOffset = 8,
                yOffset = 8,
                width = 716
            },
            portrait = new()
            {
                xOffset = -352,
                yOffset = 32,
                right = true
            },
            name = new()
            {
                xOffset = -222,
                yOffset = 320,
                right = true
            },
            jewel = new()
            {
                xOffset = -64,
                yOffset = 256,
                right = true,
            },
            button = new()
            {
                xOffset = -532,
                yOffset = -44,
                right = true,
                bottom = true
            },
            images = new()
            {
                new()
                {
                    ID = "DialogueDisplayFramework.Images.PortraitBackground",
                    texturePath = "LooseSprites/Cursors",
                    xOffset = -452,
                    right = true,
                    x = 583,
                    y = 411,
                    w = 115,
                    h = 97
                }
            },
            dividers = new()
            {
                new() {
                    ID = "DialogueDisplayFramework.Dividers.PortraitDivider",
                    xOffset = -484,
                    right = true
                }
            }
        };

        public DialogueDisplayData FillEmptyValuesFrom(DialogueDisplayData data)
        {
            if (data == null)
                return this;

            xOffset ??= data.xOffset;
            yOffset ??= data.yOffset;
            width ??= data.width;
            height ??= data.height;
            dialogue ??= data.dialogue;
            portrait ??= data.portrait;
            name ??= data.name;
            jewel ??= data.jewel;
            button ??= data.button;
            //sprite ??= data.sprite;
            gifts ??= data.gifts;
            hearts ??= data.hearts;
            disabled = data.disabled;

            if (data.images != null)
                images = images != null ? data.images.Concat(images).ToList() : data.images;

            if (data.texts != null)
                texts = texts != null ? data.texts.Concat(texts).ToList() : data.texts;

            if (data.dividers != null)
                dividers = dividers != null ? data.dividers.Concat(dividers).ToList() : data.dividers;

            return this;
        }
    }

    public class BaseData
    {
        public int xOffset;
        public int yOffset;
        public bool right;
        public bool bottom;
        public int width = -1;
        public int height;
        public float alpha = 1;
        public float scale = 4;
        public float layerDepth = 0.88f;
        public bool disabled;
    }

    public class DialogueData : BaseData
    {
        public string color;
        public SpriteText.ScrollTextAlignment alignment; // left, center, right
    }

    public class PortraitData : BaseData
    {
        public string texturePath;
        public int x = -1;
        public int y = -1;
        public int w = 64;
        public int h = 64;
        public bool tileSheet = true;
    }

    public class HeartsData : BaseData
    {
        public int heartsPerRow = 14;
        public bool showEmptyHearts = true;
        public bool showPartialhearts = true;
        public bool centered;
    }

    public class GiftsData : BaseData
    {
        public bool showGiftIcon = true;
        public bool inline = false;
    }

    public class SpriteData : BaseData
    {
        public bool background;
        public int frame;
    }

    public class ImageData : BaseData
    {
        public string ID = DialogueDisplayData.MISSING_ID_STR;
        public string texturePath;
        public int x;
        public int y;
        public int w;
        public int h;
    }

    public class TextData : BaseData
    {
        public string ID = DialogueDisplayData.MISSING_ID_STR;
        public string color;
        public string text;
        public bool junimo;
        public bool scroll;
        public string placeholderText;
        public int scrollType = -1;
        public SpriteText.ScrollTextAlignment alignment = SpriteText.ScrollTextAlignment.Center; // left, center, right
        public bool centered; // Deprecated
    }

    public class DividerData : BaseData
    {
        public string ID = DialogueDisplayData.MISSING_ID_STR;
        public bool horizontal;
        public bool small;
        public DividerConnectorData connectors = new() { top = true, bottom = true };
        public string color;
    }

    public class DividerConnectorData
    {
        public bool top;
        public bool bottom;
    }
}