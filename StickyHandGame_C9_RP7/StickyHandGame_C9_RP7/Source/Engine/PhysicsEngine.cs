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
        /// Moves the towards.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="deltaTime">The delta time.</param>
        /// <returns>Final position</returns>
        public static Vector2 MoveTowards(Entity entity, Vector2 velocity, float deltaTime)
        {
            if (velocity.Length() <= 0)
                return entity.Position;

            // TODO do something smarter with Layers later
            if (entity.CollisionComponent.Layer == CollisionLayers.Ghost)
            {
                entity.Position += velocity * deltaTime;
                return entity.Position;
            }


            Vector2 unitDirection = new Vector2(velocity.X, velocity.Y);
            unitDirection.Normalize();
            float distanceFull = (velocity * deltaTime).Length();
            float distanceMoved = 0.0f;
            float stepDistance = 0.04f;

            var possibleCollisions = GameManager.Instance.CollidableNonPlayerEntityList;
            Vector2 nextMovement;
            Vector2 previousMovement;

            while (distanceMoved < distanceFull)
            {

                // if step movement would be greater than full movement, limit to full movement
                if ((distanceMoved + stepDistance) > distanceFull)
                    nextMovement = unitDirection * (distanceFull - distanceMoved);
                else
                    nextMovement = unitDirection * stepDistance;

                previousMovement = nextMovement;

                // Get all collisions
                var collisions = CheckPossibleCollisions(entity, nextMovement, possibleCollisions);

                // For each collision react
                foreach (var collision in collisions)
                {
                    // Found that this worked. With static collisions move the entity a bit away from the colliding object based upon
                    // what side it collided with
                    if (collision.CollisionComponent.Entity.CollisionComponent.Layer == CollisionLayers.Static)
                    {
                        //Entity otherEntity = collision.CollisionComponent.Entity;


                        nextMovement += (collision.NormalVector * 0.05f);

                        // Can lerp like this because of unit vector?
                        //nextMovement = new Vector2(MathHelper.Lerp(0.0f, nextMovement.X, Math.Abs(collision.NormalVector.Y)), MathHelper.Lerp(0.0f, nextMovement.Y, Math.Abs(collision.NormalVector.X)));
                        //nextMovement = Vector2.Zero;

                        //if (Math.Abs(collision.NormalVector.X) > 1.0)
                        //    nextPosition.X = 0.0f;

                        //if (Math.Abs(collision.NormalVector.Y) > 1.0)
                        //    nextPosition.Y = 0.0f;


                        //if (collision.CollisionComponent.BoundaryType ==
                        //    CollisionComponent.CollisionBoundaryType.Square)
                        //{
                        //    // Entity collided on top
                        //    if (collision.NormalVector.Y == 1.0f)
                        //    {
                        //        entity.Position.Y = otherEntity.Position.Y - ((((BoxColliderComponent)entity.CollisionComponent).Height / 2) + (((BoxColliderComponent)otherEntity.CollisionComponent).Height / 2) + 0.1f);
                        //        nextPosition.Y = 0;
                        //    }
                        //    // Entity collided on bottom
                        //    else if (collision.NormalVector.Y == -1.0f)
                        //    {
                        //        entity.Position.Y = otherEntity.Position.Y + ((((BoxColliderComponent)entity.CollisionComponent).Height / 2) + (((BoxColliderComponent)otherEntity.CollisionComponent).Height / 2) + 0.1f);
                        //        nextPosition.Y = 0;
                        //    }
                        //    // Entity collided on left
                        //    else if (collision.NormalVector.X == 1.0f)
                        //    {
                        //        entity.Position.X = otherEntity.Position.X + ((((BoxColliderComponent)entity.CollisionComponent).Width / 2) + (((BoxColliderComponent)otherEntity.CollisionComponent).Width / 2) + 0.1f);
                        //        nextPosition.X = 0;
                        //    }
                        //    // Entity collided on right
                        //    else if (collision.NormalVector.X == -1.0f)
                        //    {
                        //        entity.Position.X = otherEntity.Position.X - ((((BoxColliderComponent)entity.CollisionComponent).Width / 2) + (((BoxColliderComponent)otherEntity.CollisionComponent).Width / 2) + 0.1f);
                        //        nextPosition.X = 0;
                        //    }
                        //    else
                        //    {
                        //        throw new NotImplementedException("Did not implement that collision TRigger");
                        //    }
                        //}


                        //entity.Position = otherEntity.Position + collision.NormalVector * otherEntity.Width; // * some coefficent?
                        //if (collision.NormalVector.X == 0.0f)
                        //    nextPosition.Y = 0;
                        //else if (collision.NormalVector.Y == 0.0f)
                        //    nextPosition.X = 0;

                        // Could maybe add friction here if wanted;


                        //switch (collision.Side)
                        //{
                        //    case Side.Top:
                        //        entity.Position.Y = otherEntity.Position.Y - ((otherEntity.Height) / 2 + entity.CollisionComponent.Height);
                        //        //entity.Position.Y = otherEntity.Position.Y - (otherEntity.Height + 0.1f);
                        //        nextPosition.Y = 0;
                        //        break;
                        //    case Side.Bottom:
                        //        entity.Position.Y = otherEntity.Position.Y + ((otherEntity.Height) + 0.1f);
                        //        nextPosition.Y = 0;
                        //        break;
                        //    case Side.Left:
                        //        entity.Position.X = otherEntity.Position.X - ((otherEntity.Width) + 0.1f);
                        //        nextPosition.X = 0;
                        //        break;
                        //    case Side.Right:
                        //        entity.Position.X = otherEntity.Position.X + ((otherEntity.Width) + 0.1f);
                        //        nextPosition.X = 0;
                        //        break;
                        //}
                    }
                }

                // If next position wouldn't move, that means movement is over
                if (nextMovement.Length() <= 0)
                    break;

                entity.Position += nextMovement;

                if (Vector2.Dot(previousMovement, nextMovement) < 0)
                    break;

                distanceMoved += nextMovement.Length();
            }

            return entity.Position;
        }


        /// <summary>
        /// Checks the possible collisions.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="possibleCollisions">The possible collisions.</param>
        /// <returns></returns>
        private static List<CollisionInfo> CheckPossibleCollisions(Entity entity, Vector2 movement, List<Entity> possibleCollisions)
        {
            List<CollisionInfo> collisions = new List<CollisionInfo>();

            // Check possible collisions with predicted movement
            foreach (Entity otherEntity in possibleCollisions)
            {
                var check = CheckWouldCollide(
                    entity.CollisionComponent,
                    movement,
                    otherEntity.CollisionComponent);

                // if collided
                if (check != null)
                {
                    // Call collision trigger on moving item
                    entity.CollisionTriggered(check.Item1);
                    otherEntity.CollisionTriggered(check.Item2);

                    // Add collision to return list
                    collisions.Add(check.Item1);
                }
            }

            return collisions;
        }


        private static Tuple<CollisionInfo, CollisionInfo> CheckWouldCollide(CollisionComponent movingObject, Vector2 movement, CollisionComponent otherObject)
        {
            Vector2 origin = movingObject.Entity.Position + movement;
            Vector2 otherOrigin = otherObject.Entity.Position;

            //TODO Add Fucking Triangles

            if (movingObject.BoundaryType == CollisionComponent.CollisionBoundaryType.Square && otherObject.BoundaryType == CollisionComponent.CollisionBoundaryType.Square)
            {
                BoxColliderComponent movingBox = (BoxColliderComponent)movingObject;
                BoxColliderComponent otherBox = (BoxColliderComponent)otherObject;

                // AABB collision check. Offset box by movement
                if (origin.X + movement.X - (movingBox.Width / 2) < otherOrigin.X + (otherBox.Width / 2) &&
                    origin.X + movement.X + (movingBox.Width / 2) > otherOrigin.X - (otherBox.Width / 2) &&
                    origin.Y + movement.Y - (movingBox.Height / 2) < otherOrigin.Y + (otherBox.Height / 2) &&
                    origin.Y + movement.Y + (movingBox.Height / 2) > otherOrigin.Y - (otherBox.Height / 2))
                {

                    // Do dot products to determine which side your hitting the other object from. May not be 100% accurate
                    Vector2 normal = new Vector2();
                    Vector2 point = new Vector2();
                    Vector2 otherNormal = new Vector2();
                    Vector2 otherPoint = new Vector2();

                    Vector2 direction = (origin - otherOrigin);
                    direction.Normalize();
                    // Compare difference in X and Y components. Because they're squares only. Smaller X component means means bottom and top collisions. Smaller Y component means side collisions.
                    // Then do dot products to determine which direction. CosTheta
                    // IMPORTANT. Points aren't actually the point of collision, more like the center of the side
                    if (Math.Abs(origin.X - otherOrigin.X) < Math.Abs(origin.Y - otherOrigin.Y))
                    {
                        Vector2 up = new Vector2(0, -1);
                        // If moving collided with the top of the other object
                        if (Vector2.Dot(up, direction) > 0)
                        {
                            normal = new Vector2(0, -1);
                            otherNormal = new Vector2(0, 1);

                            point = new Vector2(otherBox.Entity.Position.X, otherBox.Entity.Position.Y - otherBox.Height);
                            otherPoint = new Vector2(movingBox.Entity.Position.X, movingBox.Entity.Position.Y + movingBox.Height);
                        }
                        // If moving collided with the bottom of the other object
                        else
                        {
                            normal = new Vector2(0, 1);
                            otherNormal = new Vector2(0, -1);

                            point = new Vector2(otherBox.Entity.Position.X, otherBox.Entity.Position.Y + otherBox.Height);
                            otherPoint = new Vector2(movingBox.Entity.Position.X, movingBox.Entity.Position.Y - movingBox.Height);
                        }
                    }
                    else
                    {
                        Vector2 right = new Vector2(1, 0);
                        // If moving collided with the right of the other object
                        if (Vector2.Dot(right, direction) > 0)
                        {
                            normal = new Vector2(1, 0);
                            otherNormal = new Vector2(-1, 0);

                            point = new Vector2(otherBox.Entity.Position.X + otherBox.Width, otherBox.Entity.Position.Y);
                            otherPoint = new Vector2(movingBox.Entity.Position.X - movingBox.Width, movingBox.Entity.Position.Y);
                        }
                        // If moving collided with the left of the other object
                        else
                        {
                            normal = new Vector2(-1, 0);
                            otherNormal = new Vector2(1, 0);

                            point = new Vector2(otherBox.Entity.Position.X - otherBox.Width, otherBox.Entity.Position.Y);
                            otherPoint = new Vector2(movingBox.Entity.Position.X + movingBox.Width, movingBox.Entity.Position.Y);
                        }
                    }

                    return new Tuple<CollisionInfo, CollisionInfo>(new CollisionInfo(otherObject, point, normal), new CollisionInfo(movingObject, otherPoint, otherNormal));
                }
            }
            else
            {
                throw new NotImplementedException("I don't got that collider shit made");
            }

            return null;
        }



    }
}
