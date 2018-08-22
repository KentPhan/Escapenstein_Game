using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Managers
{
    public class CollisionManager : Manager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static CollisionManager _instance;
        public static CollisionManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CollisionManager();
                }
                return _instance;
            }
        }


        private List<Entity> entities;

        private CollisionManager()
        {
            
        }
        
        public override void Initialize(List<Entity> entities)
        {
            this.entities = entities;
        }

        public override void Update(GameTime time)
        {
            if (entities == null)
                return;

            // TODO: Need to optimize maybe to check layers 
            foreach (Entity entity in entities)
            {
                entity.CollisionComponent?.CheckDoesCollide(entities);
            }
        }
    }
}
