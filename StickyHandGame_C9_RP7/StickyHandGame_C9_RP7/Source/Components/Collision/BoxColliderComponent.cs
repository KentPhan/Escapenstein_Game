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
        private Point _center;

        private float _width;

        private float _height;


        public BoxColliderComponent(Point center, float width, float height)
        {
            _center = center;
            _width = width;
            _height = height;
        }
    }
}
