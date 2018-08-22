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
        private Vector2 _center;

        private float _width;

        private float _height;


        public BoxColliderComponent(Vector2 center, float width, float height)
        {
            _center = center;
            _width = width;
            _height = height;
        }

        public bool DoesCollide(BoxColliderComponent otherBox)
        {
            float halfWidth = _width / 2;
            float halfHeight = _height / 2;
            Vector2 BLeft = new Vector2(_center.X - halfWidth, _center.Y + halfHeight);
            Vector2 BRight = new Vector2(_center.X + halfWidth, _center.Y + halfHeight);
            Vector2 TLeft = new Vector2(_center.X - halfWidth, _center.Y - halfHeight);
            Vector2 TRight = new Vector2(_center.X + halfWidth, _center.Y - halfHeight);





            return false;
        }
    }
}
