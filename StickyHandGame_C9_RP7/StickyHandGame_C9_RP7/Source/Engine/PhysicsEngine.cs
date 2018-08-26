using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Engine
{
    /// <summary>
    /// Basic collisions are handled here. Take note that this only accounts player collision with other objects right now. Could adapt later.
    /// </summary>
    public static class PhysicsEngine
    {

        /// <summary>
        /// Moves the Entity towards a destination.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="deltaTime">The delta time.</param>
        public static void MoveTowards(Entity entity, Vector2 velocity, float deltaTime)
        {
            if (velocity.Length() <= 0)
                return;

            Vector2 unitDirection = new Vector2(velocity.X, velocity.Y);
            unitDirection.Normalize();
            float distanceFull = (velocity * deltaTime).Length();
            float distanceMoved = 0.0f;
            float stepDistance = 0.01f;

            var possibleCollisions = GameManager.Instance.CollidableNonPlayerEntityList;
            Vector2 nextPosition;

            while (distanceMoved < distanceFull)
            {
                // if step movement would be greater than full movement, limit to full movement
                if ((distanceMoved + stepDistance) > distanceFull)
                    nextPosition = unitDirection * (distanceFull - distanceMoved);
                else
                    nextPosition = unitDirection * stepDistance;


                // Get all collisions
                var collisions = CheckPossibleCollisions(entity, nextPosition, possibleCollisions);

                // For each collision react
                foreach (var collision in collisions)
                {
                    // Found that this worked. With static collisions move the entity a bit away from the colliding object based upon
                    // what side it collided with
                    if (collision.Item1.Entity.CollisionComponent.Layer == CollisionLayers.Static)
                    {
                        switch (collision.Item3)
                        {
                            case Side.Top:
                                entity.Position.Y = collision.Item1.Entity.Position.Y - 32.1f;
                                nextPosition.Y = 0;
                                break;
                            case Side.Bottom:
                                entity.Position.Y = collision.Item1.Entity.Position.Y + 32.1f;
                                nextPosition.Y = 0;
                                break;
                            case Side.Left:
                                entity.Position.X = collision.Item1.Entity.Position.X - 32.1f;
                                nextPosition.X = 0;
                                break;
                            case Side.Right:
                                entity.Position.X = collision.Item1.Entity.Position.X + 32.1f;
                                nextPosition.X = 0;
                                break;
                        }
                    }
                }


                if (nextPosition.Length() <= 0)
                    break;

                entity.Position += nextPosition;

                distanceMoved += nextPosition.Length();
            }

        }


        /// <summary>
        /// Checks the possible collisions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="possibleCollisions">The possible collisions.</param>
        /// <returns></returns>
        private static List<Tuple<CollisionComponent, Vector2, Side>> CheckPossibleCollisions(Entity entity, Vector2 movement, List<Entity> possibleCollisions)
        {
            List<Tuple<CollisionComponent, Vector2, Side>> collisions = new List<Tuple<CollisionComponent, Vector2, Side>>();

            // Check possible collisions with predicted movement
            foreach (Entity otherEntity in possibleCollisions)
            {
                var check = CheckWouldCollide(
                    (BoxColliderComponent)entity.CollisionComponent,
                    movement,
                    (BoxColliderComponent)otherEntity.CollisionComponent);

                // if collided
                if (check != null)
                {
                    // Call collision trigger on moving item
                    entity.CollisionTriggered(check);

                    // Add collision to return list
                    collisions.Add(check);
                }
            }

            return collisions;
        }

        /// <summary>
        /// Checks the collision.
        /// </summary>
        /// <param name="movingBox">The moving box.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="otherBox">The other box.</param>
        /// <returns></returns>
        private static Tuple<CollisionComponent, Vector2, Side> CheckWouldCollide(BoxColliderComponent movingBox, Vector2 movement, BoxColliderComponent otherBox)
        {
            Vector2 origin = movingBox.Entity.Position + movement;
            Vector2 otherOrigin = otherBox.Entity.Position;

            // AABB collision check. Offset box by movement
            if (origin.X + movement.X - (movingBox.Width / 2) < otherOrigin.X + (otherBox.Width / 2) &&
                origin.X + movement.X + (movingBox.Width / 2) > otherOrigin.X - (otherBox.Width / 2) &&
                origin.Y + movement.Y - (movingBox.Height / 2) < otherOrigin.Y + (otherBox.Height / 2) &&
                origin.Y + movement.Y + (movingBox.Height / 2) > otherOrigin.Y - (otherBox.Height / 2))
            {

                // Do dot products to determine which side your hitting the other object from. May not be 100% accurate
                Side side = Side.None;
                Vector2 direction = (origin - otherOrigin);
                direction.Normalize();
                // Compare difference in X and Y components. Because they're squares only. Smaller X component means means bottom and top collisions. Smaller Y component means side collisions.
                // Then do dot products to determine which direction. CosTheta
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
