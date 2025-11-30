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

        public static DialogueDisplayData MergeEntries(DialogueDisplayData entry, DialogueDisplayData filler)
        {
            return new DialogueDisplayData()
            {
                XOffset = entry.XOffset ?? filler.XOffset,
                YOffset = entry.YOffset ?? filler.YOffset,
                Width = entry.Width ?? filler.Width,
                Height = entry.Height ?? filler.Height,
                Dialogue = entry.Dialogue ?? filler.Dialogue,
                Portrait = entry.Portrait ?? filler.Portrait,
                Name = entry.Name ?? filler.Name,
                Jewel = entry.Jewel ?? filler.Jewel,
                Button = entry.Button ?? filler.Button,
                Gifts = entry.Gifts ?? filler.Gifts,
                Hearts = entry.Hearts ?? filler.Hearts,
                Images = (entry.Images is null || filler.Images is null) ? entry.Images ?? filler.Images : filler.Images.Concat(entry.Images).ToList(),
                Texts = (entry.Texts is null || filler.Texts is null) ? entry.Texts ?? filler.Texts : filler.Texts.Concat(entry.Texts).ToList(),
                Dividers = (entry.Dividers is null || filler.Dividers is null) ? entry.Dividers ?? filler.Dividers : filler.Dividers.Concat(entry.Dividers).ToList(),
            };
        }
    }
}
