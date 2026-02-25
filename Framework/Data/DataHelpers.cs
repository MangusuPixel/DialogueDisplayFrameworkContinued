using StardewValley.BellsAndWhistles;
using System.Linq;

namespace DialogueDisplayFramework.Data
{
    public class DataHelpers
    {
        public static readonly string MISSING_ID_STR = "MISSING_ID";

        public static readonly DialogueDisplayData DefaultDisplayData = new()
        {
            Id = "DialogueDisplayFramework.Defaults",
            Width = 1200,
            Height = 384,
            Dialogue = new()
            {
                XOffset = 8,
                YOffset = 8,
                Width = 716,
                Alpha = 1,
                Scale = 4,
                LayerDepth = 0.88f
            },
            Portrait = new()
            {
                W = 64,
                H = 64,
                TileSheet = true,
                XOffset = -352,
                YOffset = 32,
                Right = true,
                Alpha = 1,
                Scale = 4,
                LayerDepth = 0.88f
            },
            Name = new()
            {
                XOffset = -222,
                YOffset = 320,
                Right = true,
                Alpha = 1,
                Scale = 4,
                LayerDepth = 0.88f
            },
            Jewel = new()
            {
                XOffset = -64,
                YOffset = 256,
                Right = true,
                Alpha = 1,
                Scale = 4,
                LayerDepth = 0.88f
            },
            Button = new()
            {
                XOffset = -532,
                YOffset = -44,
                Right = true,
                Bottom = true,
                Alpha = 1,
                Scale = 4,
                LayerDepth = 0.88f
            },
            Images = new()
            {
                new(texturePath: "LooseSprites/Cursors")
                {
                    ID = "DialogueDisplayFramework.Images.PortraitBackground",
                    XOffset = -452,
                    Right = true,
                    X = 583,
                    Y = 411,
                    W = 115,
                    H = 97
                }
            },
            Dividers = new()
            {
                new() {
                    ID = "DialogueDisplayFramework.Dividers.PortraitDivider",
                    XOffset = -484,
                    Right = true
                }
            }
        };

        public static readonly GiftsData DefaultGiftsData = new()
        {
            IconScale = 1f
        };

        public static readonly HeartsData DefaultHeartsData = new()
        {
            HeartsPerRow = 14,
            ShowEmptyHearts = true,
            ShowPartialhearts = true
        };

        public static void FillInDefaults(DialogueDisplayData entry)
        {
            entry.Gifts?.MergeFrom(DefaultGiftsData);
            entry.Hearts?.MergeFrom(DefaultHeartsData);
            entry.MergeFrom(DefaultDisplayData);
        }
    }
}
