using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Pong.Objects
{
    public class ScoreWall : GameObject
    {
        public Player Player { get; }

        public ScoreWall(Body body, Player player)
        {
            Player = player;
            Body = body;

            body.OnSeparation += IncrementScore;
        }

        private void IncrementScore(Fixture sender, Fixture other, Contact contact)
        {
            Player.IncrementScore();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // yeah, better code next game lol
        }
    }
}