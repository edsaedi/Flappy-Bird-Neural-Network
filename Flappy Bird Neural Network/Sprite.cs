﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public abstract class Sprite
    {
        Texture2D texture;
        protected Vector2 position;
        protected abstract Rectangle? sourceRectangle { get; }
        Color color;
        float rotation;
        Vector2 origin;
        Vector2 scale;
        SpriteEffects effects;
        float layerDepth;

        public Sprite(Texture2D texture, Vector2 position)
        : this(texture, position, Color.White) { }

        public Sprite(Texture2D texture, Vector2 position, Color color)
        : this(texture, position, color, Vector2.One) { }

        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 scale)
       : this(texture, position, color, 0f, position, scale, SpriteEffects.None, 0) { }

        public Sprite(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
