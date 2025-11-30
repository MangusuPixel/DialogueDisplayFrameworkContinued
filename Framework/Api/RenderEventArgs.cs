using DialogueDisplayFramework.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DialogueDisplayFramework.Api
{
    public class RenderEventArgs<T> : EventArgs, IRenderEventArgs<T>
    {
        public RenderEventArgs(SpriteBatch spriteBatch, DialogueDisplay display, T data)
        {
            SpriteBatch = spriteBatch;
            DialogueDisplay = display as IDialogueDisplay;
            Data = data;
        }

        public SpriteBatch SpriteBatch { get; }

        public IDialogueDisplay DialogueDisplay { get; }

        public T Data { get; }
    }
}
