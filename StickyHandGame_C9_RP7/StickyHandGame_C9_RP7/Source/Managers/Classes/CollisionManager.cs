using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Managers
{
    public class CollisionManager : Manager
    {

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

        private CollisionManager()
        {
            
        }
        
        public override void Initialize(Entity[] entities)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
