using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Pong.Objects
{
    public class Wall : GameObject
    {
        private readonly Vector2 size;

        public Wall(Body body, Vector2 size, Texture2D texture = null)
        {
            Body = body;
            this.size = size;

            if (texture != null)
                Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Body.Position, null, Color.White, Body.Rotation, Origin, size / TextureSize,
                SpriteEffects.FlipVertically, 0f);
        }
    }
}