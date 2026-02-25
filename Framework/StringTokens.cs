using DialogueDisplayFramework.Framework;
using StardewValley;
using StardewValley.TokenizableStrings;
using System;

namespace DialogueDisplayFramework
{
    internal class StringTokens
    {
        internal static readonly string UniqueID = "Mangupix.DDFC";

        internal static void Register()
        {
            TokenParser.RegisterParser($"{UniqueID}_Speaker", SpeakerToken);
        }

        private static bool SpeakerToken(string[] query, out string replacement, Random _, Farmer _2)
        {
            replacement = null;

            var display = DialogueDisplayPatcher.CurrentDisplay;

            if (display?.Speaker == null)
                return false;

            switch (query[0].ToLower())
            {
                case "name": replacement = display.Speaker.Name;
                    break;
                case "portrait": replacement = display.GetSpeakerPortraitAssetPath();
                    break;
                default: return false;
            }

            return true;
        }
    }
}
