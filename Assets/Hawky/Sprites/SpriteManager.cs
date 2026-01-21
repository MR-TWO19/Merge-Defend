using Hawky.GameFlow;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hawky.Sprites
{
    public partial class SpriteManager : RuntimeSingleton<SpriteManager>
    {
        private Dictionary<string, Sprite> _spritesLoaded = new Dictionary<string, Sprite>();
        private Dictionary<string, Texture2D> _texturesLoaded = new Dictionary<string, Texture2D>();
        public Sprite LoadSprite(string materialId)
        {
            if (_spritesLoaded.TryGetValue(materialId, out var sprite) == false)
            {
                var link = Path.Combine(ResourcesLink.SPRITES, materialId);
                sprite = Resources.Load<Sprite>(link);

                if (sprite == null)
                {
                    return null;
                }

                _spritesLoaded[materialId] = sprite;
            }

            return sprite;
        }

        public Texture2D LoadTexture(string textureId)
        {
            if (_texturesLoaded.TryGetValue(textureId, out var texture) == false)
            {
                var link = Path.Combine(ResourcesLink.TEXTURES, textureId);
                texture = Resources.Load<Texture2D>(link);

                if (texture == null)
                {
                    return null;
                }

                _texturesLoaded[textureId] = texture;
            }

            return texture;
        }
    }

}