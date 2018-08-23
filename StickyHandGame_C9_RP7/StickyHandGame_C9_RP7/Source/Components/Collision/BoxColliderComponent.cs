using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{



    public class BoxColliderComponent : CollisionComponent
    {
        public CollisionLayers Layer { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BoxColliderComponent"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="layer">The layer.</param>
        public BoxColliderComponent(Entity entity, CollisionLayers layer)
        {
            this.Entity = entity;
            this.Layer = layer;
            this.CollidedWith = new List<Tuple<CollisionComponent, Vector2>>();
        }

        /// <summary>
        /// Checks the does collide with box.
        /// </summary>
        /// <param name="otherBox">The other box.</param>
        public void CheckDoesCollideWithBox(BoxColliderComponent otherBox)
        {
            Vector2 origin = this.Entity.Position;
            Vector2 otherOrigin = otherBox.Entity.Position;
            float width = this.Entity.Width;
            float height = this.Entity.Height;
            float otherWidth = otherBox.Entity.Width;
            float otherHeight = otherBox.Entity.Height;


            if (origin.X - (width / 2) < otherOrigin.X + (otherWidth / 2) &&
                origin.X + (width / 2) > otherOrigin.X - (otherWidth / 2) &&
                origin.Y - (height / 2) < otherOrigin.Y + (otherHeight / 2) &&
                origin.Y + (height / 2) > otherOrigin.Y - (otherHeight / 2))
            {
                Vector2 direction = (origin - otherOrigin);
                direction.Normalize();
                CollidedWith.Add(new Tuple<CollisionComponent, Vector2>(otherBox, direction));
            }

        }

        /// <summary>
        /// Checks the does collide.
        /// </summary>
        /// <param name="otherComponents">The other components.</param>
        public override void CheckDoesCollide(List<Entity> otherComponents)
        {
            this.CollidedWith = new List<Tuple<CollisionComponent, Vector2>>();
            foreach (Entity otherEntity in otherComponents)
            {
                if (this.Entity == otherEntity)
                    continue;

                CheckDoesCollideWithBox((BoxColliderComponent)otherEntity.CollisionComponent);
            }

        }
    }
}
