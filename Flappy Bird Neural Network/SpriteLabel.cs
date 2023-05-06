using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public class SpriteLabel : Sprite
    {
        public SpriteLabel(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Vector2 scale)
            : base(texture, position, sourceRectangle, 0f, scale)
        {

        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
    }
}
