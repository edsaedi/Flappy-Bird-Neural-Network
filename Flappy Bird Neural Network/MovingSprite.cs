using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Flappy_Bird_Neural_Network
{
    public class MovingSprite : SpriteBase
    {
        protected override Rectangle? sourceRectangle { get; }
        private Vector2 speed;

        public MovingSprite(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Vector2 scale, Vector2 speed)
           : base(texture, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0)
        {
            this.sourceRectangle = sourceRectangle;
            this.speed = speed;
        }

        public void Update()
        {
            position += speed;
        }

        public void SetPosition(Vector2 position) 
        {
            base.position = position;        
        }
    }
}
