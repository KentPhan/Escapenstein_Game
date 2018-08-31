using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{
    public class TriangleColliderComponent : CollisionComponent
    {
        public enum Oritation {TR =0,TL=1,BL=2,BR=3}
        public static Vector2[] Normals = new Vector2[] {new Vector2(1,-1), new Vector2(1,1), new Vector2(-1,1), new Vector2(-1,-1) };
        public static float MagicNumber = 0.707106f;
        public Vector2 Position;
        public Oritation myOritation;
        public Vector2 NormalVector;
        public float size;// the height or width since it is half of a cube.
        public float Width { get; private set; }
        public float Height { get; private set; }
        public TriangleColliderComponent(Entity entity, float width, float height, CollisionLayers layer,Oritation oritation)
        {
            this.Width = width;
            this.Height = height;
            this.Entity = entity;
            this.Layer = layer;
            this.BoundaryType = CollisionBoundaryType.Triangle;
            this.myOritation = oritation;
            this.Position = entity.Position;
            this.size = entity.Height;
            this.NormalVector = TriangleColliderComponent.Normals[(int)this.myOritation];
            this.NormalVector.Normalize();
        }
        public static bool PlayerToTriangle(Vector2 PlayerPosition, TriangleColliderComponent tri)
        {
            Vector2 TriPositoin = tri.Position - tri.NormalVector * tri.size * TriangleColliderComponent.MagicNumber;
            Vector2 Difference = PlayerPosition - TriPositoin;
            if (Difference.X >= 0 && Difference.Y >= 0)
            {
                if (tri.myOritation == Oritation.BR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (Difference.X >= 0 && Difference.Y <= 0)
            {
                if (tri.myOritation == Oritation.TR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (Difference.X <= 0 && Difference.Y <= 0)
            {
                if (tri.myOritation == Oritation.TL)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (Difference.X <= 0 && Difference.Y >= 0)
            {
                if (tri.myOritation == Oritation.BL)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        public override void DebugDraw(GameTime gameTime)
        {
            return;
        }
    }
}
