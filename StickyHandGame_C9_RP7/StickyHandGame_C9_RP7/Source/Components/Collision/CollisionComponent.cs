using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Components
{
    public abstract class CollisionComponent
    {

        public Entity Entity { get; protected set; }

        public abstract void CheckDoesCollide(List<Entity> otherComponent);
        public abstract void CheckWillCollide(List<Entity> otherComponents, Vector2 directionMoving);

        public List<Tuple<CollisionComponent, Vector2, Side>> CollidedWith { get; protected set; }
        public List<Tuple<CollisionComponent, Vector2, Side>> WillCollideWith { get; protected set; }

    }
}
