// need this for GameTime.
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Entities.Core
{


    //
    public struct EntityAttribute
    {
        public Vector2 Origin;
        public int Width;
        public int Height;
        public Vector2 Scale;
        public Layers Layer;
        public EntityAttribute(int X, int Y, Vector2 Scale, Layers cl)
        {
            this.Scale = Scale;
            this.Width = (int)(X * Scale.X);
            this.Height = (int)(Y * Scale.Y);
            this.Origin = new Vector2((X * Scale.X) / 2, (Y * Scale.Y) / 2);
            this.Layer = cl;
        }
    }
    public struct AnimatedEntityAttribute
    {
        public EntityAttribute ea;
        public int[] framNumber;
        public int[] playSpeed;
        public String[] Names;
        public AnimatedEntityAttribute(int[] framNumber, int[] playSpeed, String[] Names, EntityAttribute ea)
        {
            this.ea = ea;
            this.framNumber = framNumber;
            this.playSpeed = playSpeed;
            this.Names = Names;
        }
    }







    public abstract class Entity : ICloneable
    {
        public CollisionComponent CollisionComponent { get; set; }

        // Anchor stuff
        public List<Entity> Anchors { get; set; }
        public bool IsActiveAnchor = false;
        public float AnchorDistance { get; set; }


        public readonly int Id = 0;
        private static int _count = 0;
        public Vector2 Position;
        public float Width;
        public float Height;
        public bool Hide;

        protected Entity()
        {
            this.Id = Entity.GetId();
            Position = new Vector2(0, 0);
            Width = 0;
            Height = 0;
            Hide = false;
        }
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Reset();
        public static int GetId()
        {
            return ++_count;
        }

        /// <summary>
        /// When a collision is triggered
        /// </summary>
        /// <param name="collided">The collided.</param>
        public abstract void CollisionTriggered(CollisionInfo collided);

        public abstract object Clone();
    }
}
