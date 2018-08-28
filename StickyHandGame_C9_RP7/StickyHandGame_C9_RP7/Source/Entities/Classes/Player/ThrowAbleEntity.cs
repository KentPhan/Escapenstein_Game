using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Arm;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public struct HandEntry
    {
        public static int[] lengthnumber = { 2, 6, 14 };
        public static float speed = 0.1f; //multiply this value with millionseconds to get distance
        public static Vector2 Ringpivot = new Vector2(15, 16);
        public static int ringLenght = 3;
        public static Vector2 Scale = new Vector2(1, 1);
        public static Vector2 HndPositionOffSet = new Vector2(0, 0);
        public static Vector2 HandOrigin = new Vector2(0, 16);
    }

    public class ThrowAbleEntity : Entity
    {

        private Stack<Entity> chain;
        //public RenderComponent MyrenderComponent;
        public float length;
        public float previouslength;
        public float deltalength;
        public Vector2 direction;
        private HandEntity.HandState myState;
        private int currentGate = 0;
        private int count = 0;
        private float angle = 0;
        private Entity Myplayer;
        public ThrowAbleEntity(Vector2 Position, Entity player)
        {
            //Myplayer = player;
            //MyrenderComponent = new RenderComponent("Bread", this, HandEntry.HandOrigin);
            //MyrenderComponent.LoadContent();
            //MyrenderComponent.Scale = HandEntry.Scale;
            //this.Position = Position;
            //myState = HandEntity.HandState.OnPlayer;
            chain = new Stack<Entity>();
        }

        public override void Draw(GameTime gameTime)
        {
            //MyrenderComponent.Draw(gameTime);
            if (count > 1)
            {
                foreach (ChainEntity ring in chain)
                {
                    ring.Draw(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime, Vector2 deltaMovement)
        {
            //this.Position += deltaMovement;
            //if (this.myState == HandEntity.HandState.OnPlayer)
            //{
            //    //AimTo(Mouse.GetState().Position);
            //    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //    {
            //        myState = HandEntity.HandState.Shooting;
            //    }
            //}
            //else if (myState == HandEntity.HandState.Shooting)
            //{
            //    if (count > HandEntry.lengthnumber[currentGate])
            //    {
            //        if (currentGate < HandEntry.lengthnumber.Length - 1)
            //        {
            //            currentGate++;
            //        }
            //        else
            //        {
            //            myState = HandEntity.HandState.Retreating;
            //        }
            //    }

            //    if (this.length - this.previouslength > 3)
            //    {
            //        count++;
            //        chain.Push(new ChainEntity(this.Position, this.angle));
            //        this.previouslength = this.length;
            //    }
            //    deltalength = HandEntry.speed * gameTime.ElapsedGameTime.Milliseconds;
            //    length += deltalength;
            //    this.Position += deltalength * HandEntry.Scale.X * this.direction;
            //}
            //else if (myState == HandEntity.HandState.Retreating)
            //{
            //    if (count < HandEntry.lengthnumber[currentGate])
            //    {
            //        if (currentGate > 0)
            //        {
            //            currentGate--;
            //        }
            //        else
            //        {
            //            myState = HandEntity.HandState.OnPlayer;
            //            this.Position = Myplayer.Position + HandEntry.HndPositionOffSet;
            //        }
            //    }

            //    if (this.previouslength - this.length > 3)
            //    {
            //        count--;
            //        chain.Pop();
            //        this.previouslength = this.length;
            //    }
            //    deltalength = HandEntry.speed * gameTime.ElapsedGameTime.Milliseconds;
            //    length -= deltalength;
            //    this.Position -= deltalength * HandEntry.Scale.X * this.direction;
            //}
            //if (count > 1)
            //{
            //    foreach (ChainEntity ring in chain)
            //    {
            //        ring.Position += deltaMovement;
            //    }
            //}
        }



        public void Shoot()
        {

        }
        public void enlong()
        {

        }
        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
