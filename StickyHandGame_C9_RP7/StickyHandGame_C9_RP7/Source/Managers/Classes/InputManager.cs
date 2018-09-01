using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Managers.Classes;

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


        public enum ControllerState
        {
            KeyboardAndMouse,
            Controller
        }

        private ControllerState _cState;


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
                    return (Keyboard.GetState().IsKeyDown(Keys.A));
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
            Vector2 worldPosition = Vector2.Transform(new Vector2(mouseLocation.X, mouseLocation.Y), Matrix.Invert(Camera.Instance.Transform));
            Vector2 direction = worldPosition - GameManager.Instance.PlayerEntity.Position;
            direction.Normalize();
            return direction;
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
