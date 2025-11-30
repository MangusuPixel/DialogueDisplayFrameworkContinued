using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace DialogueDisplayFramework.Data
{
    public class PortraitData : BaseData, IPortraitData
    {
        public Texture2D _texture;
        public bool? _isTextureValid;

        public string TexturePath { get; private set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? W { get; set; }
        public int? H { get; set; }
        public bool? TileSheet { get; set; }

        public void MergeFrom(PortraitData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            TexturePath ??= other.TexturePath;
            X ??= other.X;
            Y ??= other.Y;
            W ??= other.W;
            H ??= other.H;
            TileSheet ??= other.TileSheet;
        }

        public bool TryGetTexture(out Texture2D texture)
        {
            texture = null;

            if (!_isTextureValid.HasValue)
                _isTextureValid = Game1.content.DoesAssetExist<Texture2D>(TexturePath);

            if (_isTextureValid.Value)
            {
                texture = _texture ??= Game1.content.Load<Texture2D>(TexturePath);
                return true;
            }
            return false;
        }

        public void SetTexturePath(string path)
        {
            TexturePath = path;
            _texture = null;

            if (string.IsNullOrEmpty(TexturePath))
            {
                _isTextureValid = false;
            }
            else
            {
                _isTextureValid = Game1.content.DoesAssetExist<Texture2D>(TexturePath);
            }
        }
    }
}
