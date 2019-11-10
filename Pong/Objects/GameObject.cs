using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Pong.Objects
{
    public abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 TextureSize;
        protected Vector2 Origin;
        public Body Body { get; protected set; }

        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                TextureSize = new Vector2(texture.Width, texture.Height);
                Origin = TextureSize / 2f;
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}