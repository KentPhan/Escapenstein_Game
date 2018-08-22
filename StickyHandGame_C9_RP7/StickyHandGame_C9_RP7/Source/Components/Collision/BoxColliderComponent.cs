using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{
    public class BoxColliderComponent : CollisionComponent
    {
        public Point TLeft { get; set; }
        public Point TRight { get; set; }
        public Point BLeft { get; set; }
        public Point BRight { get; set; }

    }
}
