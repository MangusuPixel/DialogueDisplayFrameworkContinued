
using StardewModdingAPI;

namespace DialogueDisplayFramework
{
    public class ModConfig
    {
        public bool EnableMod { get; set; }
        public int DialogueWidthOffset { get; set; }
        public int DialogueHeightOffset { get; set; }
        public int DialogueXOffset { get; set; }
        public int DialogueYOffset { get; set; }

        public ModConfig()
        {
            Reset();
        }

        public void Reset()
        {
            EnableMod = true;
            DialogueWidthOffset = 0;
            DialogueHeightOffset = 0;
            DialogueXOffset = 0;
            DialogueYOffset = 0;
        }
    }
}
