using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Pong.Objects
{
    public class Ball : GameObject
    {
        public const float Radius = 0.5f;
        public bool IsActive { get; set; } = true;

        public Ball(Body body, Texture2D texture = null)
        {
            Body = body;
            if (texture != null)
            {
                Texture = texture;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var scale = new Vector2(Radius * 2f) / TextureSize;
            spriteBatch.Draw(Texture, Body.Position, null, Color.White, Body.Rotation, Origin, scale,
                SpriteEffects.FlipVertically, 0f);
        }
    }
}