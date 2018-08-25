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
            var nextPosition = new Vector2(0, 0);

            while (distanceMoved < distanceFull)
            {
                // if step movement would be greater than full movement, limit to full movement
                if ((distanceMoved + stepDistance) > distanceFull)
                    nextPosition = unitDirection * (distanceFull - distanceMoved);
                else
                    nextPosition = unitDirection * stepDistance;


                // Get all collisions
                var collisions = CheckPossibleCollisions(entity, nextPosition, possibleCollisions);
                foreach (var collision in collisions)
                {
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

                    // If would collide with a stopping object
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

            if (origin.X + movement.X - (movingBox.Width / 2) < otherOrigin.X + (otherBox.Width / 2) &&
                origin.X + movement.X + (movingBox.Width / 2) > otherOrigin.X - (otherBox.Width / 2) &&
                origin.Y + movement.Y - (movingBox.Height / 2) < otherOrigin.Y + (otherBox.Height / 2) &&
                origin.Y + movement.Y + (movingBox.Height / 2) > otherOrigin.Y - (otherBox.Height / 2))
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
