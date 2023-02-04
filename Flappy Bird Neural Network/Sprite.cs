using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public class Sprite : SpriteBase
    {
        protected override Rectangle? sourceRectangle { get; }

        public Sprite(Texture2D texture, Vector2 position)
            : base(texture, position) { }

        public Sprite(Texture2D texture, Vector2 position, Color color)
            : base(texture, position, color) { }

        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 scale)
            : base(texture, position, color, scale) { }

        public Sprite(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            : base(texture, position, color, rotation, origin, scale, effects, layerDepth) { }

        public Sprite(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, float rotation, Vector2 scale)
           : base(texture, position, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0)
        {
            this.sourceRectangle = sourceRectangle;
        }
    }
}
