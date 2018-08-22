using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Components
{
    public abstract class CollisionComponent
    {

        public bool HasCollided => CollidedWith?.Count > 0;
        public Entity Entity { get; protected set; }

        public abstract void CheckDoesCollide(List<Entity> otherComponent);
        
        public List<CollisionComponent> CollidedWith { get; protected set; }
    }
}
