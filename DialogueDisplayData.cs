using StardewValley.BellsAndWhistles;
using System.Collections.Generic;

namespace DialogueDisplayFramework
{
    public class DialogueDisplayData
    {
        public string packName;
        public int xOffset;
        public int yOffset;
        public int width;
        public int height;
        public DialogueData dialogue;
        public PortraitData portrait;
        public TextData name;
        public JewelData jewel;
        public ButtonData button;
        public SpriteData sprite;
        public GiftsData gifts;
        public HeartsData hearts;
        public List<ImageData> images = new();
        public List<TextData> texts = new();
        public List<DividerData> dividers = new();
        public bool disabled;
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

    public class JewelData : BaseData
    {
        public bool animate = true;
    }

    public class ButtonData : BaseData
    {
    }

    public class HeartsData : BaseData
    {
        public int heartsPerRow = 14;
        public bool showEmptyHearts = true;
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
        public string ID = "unnamed.image";
        public string texturePath;
        public int x;
        public int y;
        public int w;
        public int h;
    }

    public class TextData : BaseData
    {
        public string ID = "unnamed.text";
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
        public string ID = "unnamed.divider";
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