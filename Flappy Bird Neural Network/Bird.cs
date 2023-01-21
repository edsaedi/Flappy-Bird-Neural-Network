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
        private int currentIndex = 0;
        Rectangle?[] sourceRectangles;
        public Bird(Texture2D texture, Vector2 position, Rectangle?[] sourceRectangle, float rotation, Vector2 scale)
            : base(texture, position, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0)
        {
            this.sourceRectangles = sourceRectangle;
        }

        protected override Rectangle? sourceRectangle { get => sourceRectangles[currentIndex]; }


        public void Animate(SpriteBatch spriteBatch)
        {
            if (currentIndex == sourceRectangles.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            Draw(spriteBatch);
        }

    }
}
