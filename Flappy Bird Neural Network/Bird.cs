using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public class Bird : Sprite
    {
        Bird(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, float rotation, Vector2 scale)
            : base(texture, position, sourceRectangle, Color.White, rotation, position, scale, SpriteEffects.None, 0) { }


    }
}
