using Microsoft.Xna.Framework.Graphics;

namespace Pong.Objects
{
    public class GameCage
    {
        public Wall UpperWall { get; set; }
        public Wall LowerWall { get; set; }
        public Wall LeftWall { get; set; }
        public Wall RightWall { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpperWall?.Draw(spriteBatch);
            LowerWall?.Draw(spriteBatch);
            LeftWall?.Draw(spriteBatch);
            RightWall?.Draw(spriteBatch);
        }
    }
}