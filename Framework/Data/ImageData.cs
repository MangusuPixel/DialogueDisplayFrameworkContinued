using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace DialogueDisplayFramework.Data
{
    public class ImageData : BaseData, IImageData, IMergeableEntry<ImageData>
    {
        public Texture2D _texture;
        public bool? _isTextureReady;
        public bool? _isTokenizedPath;

        public string ID { get; set; }
        public string TexturePath { get; private set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? W { get; set; }
        public int? H { get; set; }

        public ImageData(string texturePath = null)
        {
            TexturePath = texturePath;
        }

        public void MergeFrom(ImageData other)
        {
            if (other == null || other == this)
                return;

            base.MergeFrom(other);
            TexturePath ??= other.TexturePath;
            X ??= other.X;
            Y ??= other.Y;
            W ??= other.W;
            H ??= other.H;
        }

        public bool TryGetTexture(out Texture2D texture)
        {
            texture = null;

            if (!_isTextureReady.HasValue)
            {
                _isTextureReady = !HasTokenizedPath() && Game1.content.DoesAssetExist<Texture2D>(TexturePath);
            }

            if (_isTextureReady.Value)
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

            if (string.IsNullOrEmpty(TexturePath) || HasTokenizedPath())
            {
                _isTextureReady = false;
            } else
            {
                _isTextureReady = Game1.content.DoesAssetExist<Texture2D>(TexturePath);
            }
        }

        public bool HasTokenizedPath()
        {
            _isTokenizedPath ??= TexturePath.Contains('[');
            return _isTokenizedPath.Value;
        }
    }
}
