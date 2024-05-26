using StardewValley.Menus;
using Microsoft.Xna.Framework;
using DialogueDisplayFramework.Data;

namespace DialogueDisplayFramework
{
    public class DialogueBoxDrawUtils
    {
        public static Vector2 GetDataVector(DialogueBox box, BaseData data)
        {
            return new Vector2(box.x + (data.Right ? box.width : 0) + data.XOffset, box.y + (data.Bottom ? box.height : 0) + data.YOffset);
        }
    }
}
