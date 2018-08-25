using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Managers
{
    public static class PhysicsEngine
    {

        public static void MoveTowards(Entity entity, Vector2 velocity, float deltaTime)
        {
            if (velocity.Length() <= 0)
                return;

            Vector2 unitDirection = new Vector2(velocity.X, velocity.Y);
            unitDirection.Normalize();
            float distanceFull = (velocity * deltaTime).Length();
            float distanceMoved = 0.0f;
            float stepDistance = 0.01f;

            var possibleCollisions = GameManager.Instance.NonPlayerEntityList;
            var predictedEntity = (Entity)entity.Clone();

            while (distanceMoved <= distanceFull)
            {
                // if step movement would be greater than full movement, limit to full movement
                if ((distanceMoved + stepDistance) > distanceFull)
                    predictedEntity.Position += unitDirection * (distanceFull - distanceMoved);
                else
                    predictedEntity.Position += unitDirection * stepDistance;

                if (CheckPossibleCollisions(entity, predictedEntity, possibleCollisions))
                    break;

                entity.Position = predictedEntity.Position;
                distanceMoved += stepDistance;
            }

        }

        /// <summary>
        /// Checks the possible collisions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="possibleCollisions">The possible collisions.</param>
        /// <returns></returns>
        private static bool CheckPossibleCollisions(Entity originalEntity, Entity entity, List<Entity> possibleCollisions)
        {
            // Check possible collisions with predicted movement
            bool collidedWithStoppingObject = false;
            foreach (Entity otherEntity in possibleCollisions)
            {
                var check = CheckCollision(
                    (BoxColliderComponent)entity.CollisionComponent,
                    (BoxColliderComponent)otherEntity.CollisionComponent);

                // if collided
                if (check != null)
                {
                    // Call collision trigger on moving item
                    originalEntity.CollisionTriggered(check);

                    // If would collide with a stopping object
                    if (check.Item1.Entity.CollisionComponent.Layer == CollisionLayers.Static)
                    {
                        collidedWithStoppingObject = true;
                    }
                }
            }

            return collidedWithStoppingObject;
        }

        /// <summary>
        /// Checks the collision.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="otherBox">The other box.</param>
        /// <returns></returns>
        private static Tuple<CollisionComponent, Vector2, Side> CheckCollision(BoxColliderComponent box, BoxColliderComponent otherBox)
        {
            Vector2 origin = box.Entity.Position;
            Vector2 otherOrigin = otherBox.Entity.Position;

            if (origin.X - (box.Width / 2) < otherOrigin.X + (otherBox.Width / 2) &&
                origin.X + (box.Width / 2) > otherOrigin.X - (otherBox.Width / 2) &&
                origin.Y - (box.Height / 2) < otherOrigin.Y + (otherBox.Height / 2) &&
                origin.Y + (box.Height / 2) > otherOrigin.Y - (otherBox.Height / 2))
            {

                Side side = Side.None;

                Vector2 direction = (origin - otherOrigin);
                direction.Normalize();
                if (Math.Abs(origin.X - otherOrigin.X) < Math.Abs(origin.Y - otherOrigin.Y))
                {

                    Vector2 up = new Vector2(0, 1);
                    side = (Vector2.Dot(up, direction) > 0) ? Side.Bottom : Side.Top;
                }
                else
                {
                    Vector2 right = new Vector2(1, 0);
                    side = (Vector2.Dot(right, direction) > 0) ? Side.Right : Side.Left;
                }

                return new Tuple<CollisionComponent, Vector2, Side>(otherBox, direction, side);
            }
            return null;
        }



    }
}
