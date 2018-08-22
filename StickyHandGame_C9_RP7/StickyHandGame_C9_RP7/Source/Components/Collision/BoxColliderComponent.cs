using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{

    

    public class BoxColliderComponent : CollisionComponent
    {
        public Vector2 Origin { get; }

        public float Width { get; }

        public float Height { get; }

        public CollisionLayers Layer { get; }
        

        public BoxColliderComponent(Entity entity,Vector2 origin, float width, float height, CollisionLayers layer)
        {
            this.Entity = entity;
            this.Origin = origin;
            this.Width = width;
            this.Height = height;
            this.Layer = layer;
            this.CollidedWith = new List<CollisionComponent>();
        }

        /// <summary>
        /// Checks the does collide with box.
        /// </summary>
        /// <param name="otherBox">The other box.</param>
        public void CheckDoesCollideWithBox(BoxColliderComponent otherBox)
        {
            if(this.Origin.X - (this.Width / 2) < otherBox.Origin.X + (otherBox.Width / 2) &&
                                this.Origin.X + (this.Width / 2) > otherBox.Origin.X - (otherBox.Width / 2) &&
                                this.Origin.Y - (this.Height / 2) < otherBox.Origin.Y + (otherBox.Height / 2) &&
                                this.Origin.Y + (this.Height / 2) > otherBox.Origin.Y - (otherBox.Height / 2))
                CollidedWith.Add(otherBox);
        }

        /// <summary>
        /// Checks the does collide.
        /// </summary>
        /// <param name="otherComponents">The other components.</param>
        public override void CheckDoesCollide(List<Entity> otherComponents)
        {
            this.CollidedWith = new List<CollisionComponent>();
            foreach (Entity otherEntity in otherComponents)
            {
                if (this.Entity == otherEntity)
                    continue;
                
                CheckDoesCollideWithBox((BoxColliderComponent)otherEntity.CollisionComponent);
            }
            
        }
    }
}
