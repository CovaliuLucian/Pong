using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Pong.Objects
{
    public class Player : GameObject
    {
        private readonly Vector2 size;
        public int Score { get; protected set; }
        private readonly List<Action> scoreCallbacks;

        public Player(Body body, Vector2 size, Texture2D texture = null)
        {
            this.size = size;
            Body = body;
            if (texture != null)
            {
                Texture = texture;
            }

            scoreCallbacks = new List<Action>();
            body.OnSeparation += OnSeparation;
        }

        public void AddCallback(Action callback)
        {
            scoreCallbacks.Add(callback);
        }

        public void IncrementScore()
        {
            Score++;
            foreach (var callback in scoreCallbacks)
            {
                callback.Invoke();
            }
        }

        private static void OnSeparation(Fixture sender, Fixture other, Contact contact)
        {
            var playerVelocity = sender.Body.LinearVelocity;
            other.Body.ApplyLinearImpulse(playerVelocity);
            if (other.Body.LinearVelocity.X > 10)
                other.Body.LinearVelocity = new Vector2(10, other.Body.LinearVelocity.Y);
            if (other.Body.LinearVelocity.Y > 10)
                other.Body.LinearVelocity = new Vector2(other.Body.LinearVelocity.X, 10);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Body.Position, null, Color.White, Body.Rotation, Origin, size / TextureSize,
                SpriteEffects.FlipVertically, 0f);
        }

        public void Move(PlayerMovement movement, float upBorder, float downBorder)
        {
            Body.LinearVelocity = Vector2.Zero;
            switch (movement)
            {
                case PlayerMovement.Up:
                    if (Body.Position.Y < upBorder)
                        Body.LinearVelocity = new Vector2(0, 6f);
                    break;
                case PlayerMovement.Down:
                    if (Body.Position.Y > downBorder)
                        Body.LinearVelocity = new Vector2(0, -6f);
                    break;
            }
        }
    }
}