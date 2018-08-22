﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// need this for GameTime.
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;

namespace StickyHandGame_C9_RP7.Source.Entities.Core
{
    //
    public struct EntityAttribute{
        public Vector2 Origin;
        public int Width;
        public int Hight;
        public Vector2 Scale;
        public CollisionLayers collisionLayer;
        public EntityAttribute(int X, int Y, Vector2 Scale,CollisionLayers cl) {
            this.Scale = Scale;
            this.Width = (int)(X * Scale.X);
            this.Hight = (int)(Y * Scale.Y);
            this.Origin = new Vector2((X*Scale.X)/2,(Y*Scale.Y)/2);
            this.collisionLayer = cl;
        }
    }
    public struct AnimatedEntityAttribute {
        public EntityAttribute ea;
        public int[] framNumber;
        public int[] playSpeed;
        public String[] Names;
        public AnimatedEntityAttribute(int[] framNumber, int[] playSpeed, String[] Names, EntityAttribute ea) {
            this.ea = ea;
            this.framNumber = framNumber;
            this.playSpeed = playSpeed;
            this.Names = Names;
        }
    }

    public abstract class Entity
    {
        public CollisionComponent CollisionComponent { get; set; }

        protected int id = 0;
        protected GameManager g;
        private static int count = 0;
        public Vector2 position;



        protected Entity(GameManager g) {
            this.id = Entity.GetId();
            position = new Vector2(0, 0);
            this.g = g;
        }
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Reset();
        public static int GetId() {
            return ++count;
        }
        
    }
}
