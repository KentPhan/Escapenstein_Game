using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// need this for GameTime.
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;

namespace StickyHandGame_C9_RP7.Source.Entities.Core
{
    public abstract class Entity
    {
        protected int id = 0;
        protected Game1 g;
        private static int count = 0;
        public Vector2 position;
        protected Entity(Game1 g) {
            this.id = Entity.GetId();
            position = new Vector2(0, 0);
            this.g = g;
        }
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Reset();
        public static int GetId() {
            return ++count;
        }
        
    }
}
