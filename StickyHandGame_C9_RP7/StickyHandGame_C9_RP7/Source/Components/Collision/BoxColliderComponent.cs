using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            this.WillCollideWith = new List<Tuple<CollisionComponent, Vector2>>();
        }

        /// <summary>
        /// Checks the collision.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="otherBox">The other box.</param>
        /// <returns></returns>
        public static Tuple<CollisionComponent,Vector2> CheckCollision(BoxColliderComponent box, BoxColliderComponent otherBox)
        {
            Vector2 origin = box.Entity.Position;
            Vector2 otherOrigin = otherBox.Entity.Position;
            float width = box.Entity.Width;
            float height = box.Entity.Height;
            float otherWidth = otherBox.Entity.Width;
            float otherHeight = otherBox.Entity.Height;


            if (origin.X - (width / 2) < otherOrigin.X + (otherWidth / 2) &&
                origin.X + (width / 2) > otherOrigin.X - (otherWidth / 2) &&
                origin.Y - (height / 2) < otherOrigin.Y + (otherHeight / 2) &&
                origin.Y + (height / 2) > otherOrigin.Y - (otherHeight / 2))
            {
                Vector2 direction = (origin - otherOrigin);
                direction.Normalize();
                return new Tuple<CollisionComponent, Vector2>(otherBox, direction);
            }
            return null;
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

                var check = BoxColliderComponent.CheckCollision((BoxColliderComponent) this.Entity.CollisionComponent,
                    (BoxColliderComponent) otherEntity.CollisionComponent);
                if(check != null)
                    this.CollidedWith.Add(check);
            }
        }

        public override void CheckWillCollide(List<Entity> otherComponents, Vector2 directionMoving)
        {
            float directionalSpeed = 10.0f;
            float predictedTime = 0.02f;
            Vector2 unitDirection = new Vector2(directionMoving.X, directionMoving.Y);
            unitDirection.Normalize();

            this.WillCollideWith  = new List<Tuple<CollisionComponent, Vector2>>();
            foreach (Entity otherEntity in otherComponents)
            {
                if (this.Entity == otherEntity)
                    continue;

                var predictedEntity = (Entity)this.Entity.Clone();
                predictedEntity.Position += unitDirection * directionalSpeed * predictedTime;

                var check = BoxColliderComponent.CheckCollision(
                    (BoxColliderComponent) predictedEntity.CollisionComponent,
                    (BoxColliderComponent) otherEntity.CollisionComponent);
                if(check != null)
                    this.WillCollideWith.Add(check);

                predictedEntity.UnloadContent();
            }

        }
    }
}
