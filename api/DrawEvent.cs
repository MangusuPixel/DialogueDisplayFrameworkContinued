using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;

namespace DialogueDisplayFramework.Api
{
    public class DrawEvent<TValue>
    {
        private event Action<SpriteBatch, DialogueBox, TValue> Handler;

        public void Add(ModEntry sender, Action<SpriteBatch, DialogueBox, TValue> callback)
        {
            Handler += callback;
        }

        // UNUSED
        public void Remove(ModEntry sender, Action<SpriteBatch, DialogueBox, TValue> callback)
        {
            Handler -= callback;
        }

        public void Raise(SpriteBatch b, DialogueBox db, TValue data)
        {
            Handler?.Invoke(b, db, data);
        }
    }
}
