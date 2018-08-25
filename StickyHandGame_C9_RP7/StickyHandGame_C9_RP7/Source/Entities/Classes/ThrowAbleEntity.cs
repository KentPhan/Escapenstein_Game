using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Components.Render;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Components;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    // 15 18
    public struct HandEntry {
        public static int[] lengthnumber = {2,6,14};
        public static float speed = 0.1f; //multiply this value with millionseconds to get distance
        public static Vector2 Ringpivot = new Vector2(15,16);
        public static int ringLenght = 3;
        public static Vector2 Scale = new Vector2(1, 1);
        public static Vector2 HndPositionOffSet = new Vector2(0, 0);
        public static Vector2 HandOrigin = new Vector2(0, 16);
    }
    class ChainEntity : Entity
    {
        public RenderComponent MyrenderComponent;
        public ChainEntity(Vector2 position,float angle) {
            this.Position = position;
            this.MyrenderComponent = new RenderComponent("Chain", this);
            this.MyrenderComponent.Scale = HandEntry.Scale;
            this.MyrenderComponent.Rotation = angle;
            this.MyrenderComponent.Origin = HandEntry.Ringpivot;
            this.MyrenderComponent.LoadContent();
        }

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            this.MyrenderComponent.Draw(gameTime);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
    class ThrowAbleEntity: Entity
    {
        private enum HandState {
            Default,
            Shoot,
            Engage,
            Retreat,
        }
        private Stack<Entity> chain;
        public RenderComponent MyrenderComponent;
        public float length;
        public float previouslength;
        public float deltalength;
        public Vector2 direction;
        private HandState myState;
        private int currentGate = 0;
        private int count = 0;
        private float angle = 0;
        private Entity Myplayer;
        public ThrowAbleEntity(Vector2 Position, Entity player) {
            Myplayer = player;
            MyrenderComponent = new RenderComponent("Bread", this, HandEntry.HandOrigin);
            MyrenderComponent.LoadContent();
            MyrenderComponent.Scale = HandEntry.Scale;
            this.Position = Position;
            myState = HandState.Default;
            chain = new Stack<Entity>();
        }

        public override void Draw(GameTime gameTime)
        {
            MyrenderComponent.Draw(gameTime);
            if (count > 1)
            {
                foreach (ChainEntity ring in chain)
                {
                    ring.Draw(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime,Vector2 deltaMovement)
        {
            this.Position += deltaMovement;
            if (this.myState == HandState.Default)
            {
                AimTo(Mouse.GetState().Position);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    myState = HandState.Shoot;
                }
            }
            else if (myState == HandState.Shoot)
            {
                if (count > HandEntry.lengthnumber[currentGate])
                {
                    if (currentGate < HandEntry.lengthnumber.Length - 1)
                    {
                        currentGate++;
                    }
                    else
                    {
                        myState = HandState.Retreat;
                    }
                }

                 if(this.length - this.previouslength > 3)
                {
                    count++;
                    chain.Push(new ChainEntity(this.Position, this.angle));
                    this.previouslength = this.length;
                }
                deltalength = HandEntry.speed * gameTime.ElapsedGameTime.Milliseconds;
                length += deltalength;
                this.Position += deltalength * HandEntry.Scale.X * this.direction;
            }
            else if (myState == HandState.Retreat) {
                if (count < HandEntry.lengthnumber[currentGate])
                {
                    if (currentGate > 0)
                    {
                        currentGate--;
                    }
                    else
                    {
                        myState = HandState.Default;
                        this.Position = Myplayer.Position + HandEntry.HndPositionOffSet;
                    }
                }

                if (this.previouslength - this.length > 3)
                {
                    count--;
                    chain.Pop();
                    this.previouslength = this.length;
                }
                deltalength = HandEntry.speed * gameTime.ElapsedGameTime.Milliseconds;
                length -= deltalength;
                this.Position -= deltalength  * HandEntry.Scale.X * this.direction;
            }
            if (count > 1)
            {
                foreach (ChainEntity ring in chain)
                {
                    ring.Position+=deltaMovement;
                }
            }
        }

        public void AimTo(Point EndPoint) {
            Vector2 EndVector = new Vector2(EndPoint.X, EndPoint.Y);
            Vector2 WorldVector = Vector2.Transform(EndVector, Matrix.Invert(Camera.Instance.Transform));
            angle = CalculateRotation(WorldVector);
            this.MyrenderComponent.Rotation = angle;
        }
        public float CalculateRotation(Vector2 EndPoint) {
            Vector2 Direction = EndPoint - this.Position;
            Direction.Normalize();
            this.direction = Direction;
            //return (float)(Math.Atan2(Direction.Y, Direction.X) / (2 * Math.PI));
            return (float)Math.Atan2(Direction.Y, Direction.X);
        }
        public void Shoot() {
            
        }
        public void enlong() {

        }
        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
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
