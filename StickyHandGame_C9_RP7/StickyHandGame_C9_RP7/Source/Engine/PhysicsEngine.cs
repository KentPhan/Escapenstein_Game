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
        private const float GravitationalAcceleration = 100f;
        private const float Drag = 2.0f;
        //private const float DragUpeerBound

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
                return Vector2.Zero;

            // TODO do something smarter with Layers later
            if (entity.CollisionComponent.Layer == CollisionLayers.Ghost)
            {
                entity.Position += velocity * deltaTime;
                return entity.Position;
            }

            if (entity.CollisionComponent.Layer != CollisionLayers.Static)
            {
                // Gravity
                velocity += new Vector2(0, 1) * GravitationalAcceleration * deltaTime;


                // drag
                if (Math.Abs(velocity.X) > 0)
                {
                    Vector2 dragVector = velocity * -1;
                    dragVector.Normalize();
                    velocity.X += dragVector.X * Drag;

                }
            }

            Vector2 unitDirection = new Vector2(velocity.X, velocity.Y);
            unitDirection.Normalize();
            float distanceFull = (velocity * deltaTime).Length();
            float distanceMoved = 0.0f;
            float stepDistance = 0.04f;

            var possibleCollisions = GameManager.Instance.CollidableNonPlayerEntityList;
            Vector2 nextMovement;
            Vector2 returnVelocity = velocity;

            while (distanceMoved < distanceFull)
            {

                // if step movement would be greater than full movement, limit to full movement
                if ((distanceMoved + stepDistance) > distanceFull)
                    nextMovement = unitDirection * (distanceFull - distanceMoved);
                else
                    nextMovement = unitDirection * stepDistance;

                // Get all collisions
                var collisions = CheckPossibleCollisions(entity, nextMovement, possibleCollisions);

                // For each collision react
                foreach (var collision in collisions)
                {
                    // Found that this worked. With static collisions move the entity a bit away from the colliding object based upon
                    // what side it collided with
                    if (collision.CollisionComponent.Entity.CollisionComponent.Layer == CollisionLayers.Static)
                    {
                        // Bounce the item back a bit.
                        nextMovement += (collision.NormalVector * 0.1f);

                        // On bounce back calculate reflect with less force
                        Vector2 normal = collision.NormalVector;
                        Vector2.Reflect(ref velocity, ref normal, out returnVelocity);

                        // If colliding with ground or not, apply different slow down coefficients.
                        if (normal.Y == -1)
                        {
                            returnVelocity = new Vector2(returnVelocity.X * 0.9f, returnVelocity.Y * 0.1f);
                            if (returnVelocity.Length() < 1.0f)
                                return Vector2.Zero;
                        }
                        else
                        {
                            returnVelocity = returnVelocity * 0.3f;
                        }
                    }
                }

                // If next position wouldn't move, that means movement is over
                if (nextMovement.Length() <= 0)
                    break;

                entity.Position += nextMovement;

                distanceMoved += nextMovement.Length();
            }

            return returnVelocity;
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
                Tuple<CollisionInfo, CollisionInfo> currentTuple = AABBCollision(movingObject, movement, otherObject, origin, otherOrigin);
                if(currentTuple != null){
                    return currentTuple;
                }
            }
            else if (movingObject.BoundaryType == CollisionComponent.CollisionBoundaryType.Square && otherObject.BoundaryType == CollisionComponent.CollisionBoundaryType.Triangle)
            {
                TriangleColliderComponent theotherobject = (TriangleColliderComponent)otherObject;
                BoxColliderComponent playerobject = (BoxColliderComponent)movingObject;
                if (TriangleColliderComponent.PlayerToTriangle(origin, (TriangleColliderComponent)otherObject))
                {
                    Vector2 P = origin - otherOrigin;
                    Vector2 N = theotherobject.NormalVector;
                    float L2limites = playerobject.Height * TriangleColliderComponent.MagicNumber;
                    float L1limites = theotherobject.size * TriangleColliderComponent.MagicNumber;
                    Vector2 L2 = MathmaticHelper.VectorHelper.projPtoN(P, N);
                    Vector2 L1 = MathmaticHelper.VectorHelper.perpPtoN(P, N);
                    if (L2.Length() < L2limites && L1.Length() < L1limites) {
                        Vector2 CollisionPoint = origin - theotherobject.NormalVector * L2limites;  
                        return new Tuple<CollisionInfo, CollisionInfo>(new CollisionInfo(otherObject, CollisionPoint, theotherobject.NormalVector*(-1)), new CollisionInfo(movingObject, CollisionPoint, theotherobject.NormalVector));
                    }
                }
                else {
                    Tuple<CollisionInfo, CollisionInfo> currentTuple = AABBCollision(movingObject, movement, otherObject, origin, otherOrigin);
                    if (currentTuple != null)
                    {
                        return currentTuple;
                    }
                }
            }
            else
            {
                throw new NotImplementedException("I don't got that collider shit made");
            }

            return null;
        }
        private static Tuple<CollisionInfo, CollisionInfo> AABBCollision(CollisionComponent movingObject, Vector2 movement, CollisionComponent otherObject, Vector2 origin, Vector2 otherOrigin) {
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
            return null;
        }


    }
}
