using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Objects;
using tainicom.Aether.Physics2D.Dynamics;

namespace Pong
{
    public class PongGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect spriteBatchEffect;
        private SpriteFont font;
        private TextureHelper textureHelper;

        private Ball ball;
        private readonly GameCage cage = new GameCage();
        private Player leftPlayer;
        private Player rightPlayer;
        private ScoreWall leftScoreWall;
        private ScoreWall rightScoreWall;

        // Simple camera controls
        private readonly Vector3 cameraPosition = new Vector3(0, 0, 0);
        private const float CameraViewWidth = 20f;

        // physics
        private World world;

        public PongGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.Reach,
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
                PreferMultiSampling = true
            };

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        private void DeactivateBall()
        {
            ball.IsActive = false;
        }

        private void ResetGame()
        {
            world.Remove(ball.Body);
            var circle = world.CreateCircle(Ball.Radius, 1f, new Vector2(0, Ball.Radius));
            circle.BodyType = BodyType.Dynamic;
            circle.SetRestitution(1f);
            circle.SetFriction(0.0f);

            ball = new Ball(circle, textureHelper.Get<Texture2D>("ball"));
            ball.Body.ApplyLinearImpulse(RandomHelper.GetRandomBool() ? new Vector2(8f, 0) : new Vector2(-8f, 0));
        }

        protected override void Initialize()
        {
            world = new World(Vector2.Zero);

            var circle = world.CreateCircle(Ball.Radius, 1f, new Vector2(0, Ball.Radius));
            circle.BodyType = BodyType.Dynamic;
            circle.SetRestitution(1f);
            circle.SetFriction(0.0f);

            ball = new Ball(circle);

            var horizontalWallSize = new Vector2(20f, 0.1f);
            var lowerWallBody =
                world.CreateRectangle(horizontalWallSize.X, horizontalWallSize.Y, 1f, new Vector2(0, -6));
            lowerWallBody.BodyType = BodyType.Static;
            lowerWallBody.SetRestitution(0.3f);
            lowerWallBody.SetFriction(0.5f);

            var upperWallBody = lowerWallBody.DeepClone(world);
            upperWallBody.Position = new Vector2(0, 5);

            cage.LowerWall = new Wall(lowerWallBody, horizontalWallSize);
            cage.UpperWall = new Wall(upperWallBody, horizontalWallSize);


            var verticalWallSize = new Vector2(0.1f, 12f);
            var leftWallBody = world.CreateRectangle(verticalWallSize.X, verticalWallSize.Y, 1f, new Vector2(-10f, 0));
            leftWallBody.BodyType = BodyType.Static;
            leftWallBody.SetRestitution(0.3f);
            leftWallBody.SetFriction(0.5f);

            var rightWallBody = leftWallBody.DeepClone(world);
            rightWallBody.Position = new Vector2(10f, 0);

            cage.LeftWall = new Wall(leftWallBody, verticalWallSize);
            cage.RightWall = new Wall(rightWallBody, verticalWallSize);


            var playerSize = new Vector2(0.25f, 2f);
            var leftPlayerBody =
                world.CreateRectangle(playerSize.X, playerSize.Y, 5f, new Vector2(-9f, 0.5f));
            leftPlayerBody.BodyType = BodyType.Kinematic;
            leftPlayerBody.SetRestitution(1f);
            leftPlayerBody.SetFriction(0.5f);
            leftPlayer = new Player(leftPlayerBody, playerSize);

            var rightPlayerBody = leftPlayerBody.DeepClone(world);
            rightPlayerBody.Position = new Vector2(9f, 0.5f);
            rightPlayer = new Player(rightPlayerBody, playerSize);

            var leftScoreWallBody =
                world.CreateRectangle(verticalWallSize.X, verticalWallSize.Y, 1f, new Vector2(-9.3f, 0));
            leftScoreWallBody.BodyType = BodyType.Static;

            var rightScoreWallBody = leftScoreWallBody.DeepClone(world);
            rightScoreWallBody.Position = new Vector2(9.3f, 0);

            leftScoreWall = new ScoreWall(leftScoreWallBody, rightPlayer);
            rightScoreWall = new ScoreWall(rightScoreWallBody, leftPlayer);

            leftPlayer.AddCallback(DeactivateBall);
            rightPlayer.AddCallback(DeactivateBall);

            ball.Body.ApplyLinearImpulse(RandomHelper.GetRandomBool() ? new Vector2(8f, 0) : new Vector2(-8f, 0));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteBatchEffect = new BasicEffect(graphics.GraphicsDevice)
            {
                TextureEnabled = true
            };

            textureHelper = new TextureHelper(Content);
            textureHelper.Load();

            // Load sprites
            ball.Texture = textureHelper.Get<Texture2D>("ball");
            var wallTexture = textureHelper.Get<Texture2D>("wall");
            font = textureHelper.Get<SpriteFont>("score");
            cage.LowerWall.Texture = wallTexture;
            cage.UpperWall.Texture = wallTexture;
            cage.LeftWall.Texture = wallTexture;
            cage.RightWall.Texture = wallTexture;

            leftPlayer.Texture = textureHelper.Get<Texture2D>("player1");
            rightPlayer.Texture = textureHelper.Get<Texture2D>("player2");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            if (!ball.IsActive)
                ResetGame();

            world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        private void HandleKeyboard()
        {
            var state = Keyboard.GetState();

            var movement = PlayerMovement.None;

            if (state.IsKeyDown(Keys.W))
                movement = PlayerMovement.Up;
            if (state.IsKeyDown(Keys.S))
                movement = PlayerMovement.Down;
            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.S))
                movement = PlayerMovement.None;

            leftPlayer.Move(movement, 3.95f, -4.95f);

            movement = PlayerMovement.None;

            if (state.IsKeyDown(Keys.Up))
                movement = PlayerMovement.Up;
            if (state.IsKeyDown(Keys.Down))
                movement = PlayerMovement.Down;
            if (state.IsKeyDown(Keys.Up) && state.IsKeyDown(Keys.Down))
                movement = PlayerMovement.None;

            rightPlayer.Move(movement, 3.95f, -4.95f);

            if (state.IsKeyDown(Keys.Escape))
                Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update camera View and Projection.
            var vp = GraphicsDevice.Viewport;
            spriteBatchEffect.View =
                Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Forward, Vector3.Up);
            spriteBatchEffect.Projection =
                Matrix.CreateOrthographic(CameraViewWidth, CameraViewWidth / vp.AspectRatio, 0f, -1f);

            // Draw player and ground. 
            // Our View/Projection requires RasterizerState.CullClockwise and SpriteEffects.FlipVertically.
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise,
                spriteBatchEffect);

            ball.Draw(spriteBatch);
            cage.Draw(spriteBatch);

            leftPlayer.Draw(spriteBatch);
            rightPlayer.Draw(spriteBatch);

            var textureOrigin = new Vector2(font.Texture.Width, font.Texture.Height) / 2f;

            spriteBatch.DrawString(font, "Player 1: " + leftPlayer.Score, new Vector2(-7, 6.5f), Color.Indigo, 0,
                textureOrigin, new Vector2(0.02f), SpriteEffects.FlipVertically, 1);
            spriteBatch.DrawString(font, "Player 2: " + rightPlayer.Score, new Vector2(8f, 6.5f), Color.DarkRed, 0,
                textureOrigin, new Vector2(0.02f), SpriteEffects.FlipVertically, 1);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}