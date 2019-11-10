using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class TextureHelper
    {
        private readonly ContentManager contentManager;
        private readonly Dictionary<string, object> internalDictionary;

        public TextureHelper(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            internalDictionary = new Dictionary<string, object>();
        }

        private void Load<T>(string name)
        {
            Add(name,contentManager.Load<T>(name));
        }

        public void Load()
        {
            Load<Texture2D>("ball");
            Load<Texture2D>("wall");
            Load<Texture2D>("player1");
            Load<Texture2D>("player2");
            Load<SpriteFont>("score");
        }

        public void Add<T>(string name, T obj)
        {
            internalDictionary.Add(name, obj);
        }

        public T Get<T>(string name)
        {
            return (T) internalDictionary[name];
        }
    }
}