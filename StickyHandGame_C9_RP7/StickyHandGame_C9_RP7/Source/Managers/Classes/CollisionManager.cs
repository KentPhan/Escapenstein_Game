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

        private Entity _playerEntity;
        private List<Entity> _entities;

        private CollisionManager()
        {
            
        }
        
        public void Initialize(List<Entity> entities)
        {
            this._entities = entities;
        }

        public void AssignPlayerEntity(Entity player)
        {
            this._playerEntity = player;
        }

        public override void Update(GameTime time)
        {
            if (_entities == null)
                return;

            // TODO: Need to optimize maybe to check layers 
            _playerEntity.CollisionComponent?.CheckDoesCollide(_entities);
        }
    }
}
