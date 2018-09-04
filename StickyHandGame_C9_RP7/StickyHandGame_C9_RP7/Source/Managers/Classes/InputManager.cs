using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;

namespace StickyHandGame_C9_RP7.Source.Managers
{
    public class InputManager
    {

        private static InputManager instance;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }


        /// <summary>
        /// Controllers State
        /// </summary>
        public enum ControllerState
        {
            KeyboardAndMouse,
            Controller
        }
        private ControllerState _cState;
        private Texture2D _assistLines;


        private InputManager()
        {
            var padState = GamePad.GetState(PlayerIndex.One);
            if (padState.IsConnected)
            {
                this._cState = ControllerState.Controller;
            }
            else
            {
                this._cState = ControllerState.KeyboardAndMouse;
            }
            this._assistLines = new Texture2D(GameManager.Instance.GraphicsDevice, 1, 1);
            _assistLines.SetData<Color>(new Color[] { Color.White });
        }


        public bool GetLeftShootTrigger()
        {
            switch (_cState)
            {

                case ControllerState.Controller:
                    return getGamePadState().Triggers.Left > 0.0f;
                default:
                    return (Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
            return false;
        }

        public bool GetLeftReelTrigger()
        {
            switch (_cState)
            {

                case ControllerState.Controller:
                    return getGamePadState().Buttons.LeftShoulder == ButtonState.Pressed;
                default:
                    return (Keyboard.GetState().IsKeyDown(Keys.Space));
            }
            return false;
        }

        public bool GetRightShootTrigger()
        {
            switch (_cState)
            {

                case ControllerState.Controller:
                    return getGamePadState().Triggers.Right > 0.0f;
                default:
                    return (Mouse.GetState().RightButton == ButtonState.Pressed);
            }
            return false;
        }

        public bool GetRightReelTrigger()
        {
            switch (_cState)
            {
                case ControllerState.Controller:
                    return getGamePadState().Buttons.RightShoulder == ButtonState.Pressed;
                default:
                    return (Keyboard.GetState().IsKeyDown(Keys.S));
            }
            return false;
        }


        public Vector2 GetLeftDirection()
        {
            switch (_cState)
            {
                case ControllerState.Controller:
                    Vector2 direction = getGamePadState().ThumbSticks.Left;
                    direction.Y = direction.Y * -1;
                    return direction;
                default:
                    return GetMouseDirection();
            }
        }

        public Vector2 GetRightDirection()
        {
            switch (_cState)
            {
                case ControllerState.Controller:
                    Vector2 direction = getGamePadState().ThumbSticks.Right;
                    direction.Y = direction.Y * -1;
                    return direction;
                default:
                    return GetMouseDirection();
            }
        }


        private Vector2 GetMouseDirection()
        {
            Point mouseLocation = Mouse.GetState().Position;
            Vector2 worldPosition = Vector2.Transform(new Vector2(mouseLocation.X, mouseLocation.Y), Matrix.Invert(LevelManager.Instance.CurrentCamera.Transform));
            Vector2 direction = worldPosition - LevelManager.Instance.GetCurrentPlayerInLevel().Position;
            direction.Normalize();
            return direction;
        }



        private float distanceIn = 20.0f;
        private float distanceOut = 40.0f;


        public void DrawAssist(GameTime gameTime)
        {
            if (this._cState == ControllerState.Controller)
            {
                var player = LevelManager.Instance.GetCurrentPlayerInLevel();
                var spriteBatch = GameManager.Instance.SpriteBatch;

                var leftDirection = GetLeftDirection();
                DrawLine(spriteBatch, player.Position + (leftDirection * distanceIn), player.Position + (leftDirection * distanceOut), Color.Red);

                var rightDirection = GetRightDirection();
                DrawLine(spriteBatch, player.Position + (rightDirection * distanceIn), player.Position + (rightDirection * distanceOut), Color.Blue);
            }
        }


        /// <summary>
        /// Draws the line. Some code I stole from online. From  https://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(_assistLines,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }


        private GamePadState getGamePadState()
        {
            return GamePad.GetState(PlayerIndex.One);
        }

        ///// <summary>
        ///// Gets the pressed input.
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException"></exception>
        //private bool GetPressedInput()
        //{

        //    switch (this.HandId)
        //    {
        //        case HandID.First:
        //            if (Keyboard.GetState().IsKeyDown(Keys.A))
        //                return true;
        //            break;
        //        case HandID.Second:
        //            if (Keyboard.GetState().IsKeyDown(Keys.S))
        //                return true;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Gets the released input.
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException"></exception>
        //private bool GetReleasedInput()
        //{
        //    switch (this.HandId)
        //    {
        //        case HandID.First:
        //            if (Mouse.GetState().LeftButton == ButtonState.Released)
        //                return true;
        //            break;
        //        case HandID.Second:
        //            if (Mouse.GetState().RightButton == ButtonState.Released)
        //                return true;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    return false;
        //}



    }
}
