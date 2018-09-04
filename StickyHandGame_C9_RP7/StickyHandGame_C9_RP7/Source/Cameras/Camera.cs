using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Managers.Classes;

namespace StickyHandGame_C9_RP7.Source.Cameras
{
    public enum CameraType
    {
        Screen,
        Follow
    }


    public class Camera
    {
        // Type
        public CameraType Type { get; private set; }


        // Zoom
        public float Zoom { get; set; }

        public readonly float MaxZoom = 3.0f;

        public readonly float MinZoom = 1.00f;


        public Vector2 Position { get; set; }
        public Matrix Transform { get; protected set; }

        private float currentMouseWheelValue, previousMouseWheelValue;


        public Camera(Viewport viewport, CameraType type)
        {
            Zoom = 3f;
            Position = Vector2.Zero;
            this.Type = type;
        }


        private void UpdateMatrix(Rectangle bounds)
        {
            switch (Type)
            {
                case CameraType.Screen:
                    break;
                case CameraType.Follow:
                    Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * // Translation to player
                                Matrix.CreateScale(Zoom) * // Zoom
                                Matrix.CreateTranslation(new Vector3(bounds.Width * 0.5f, bounds.Height * 0.5f, 0));// Translation to center of camera
                    break;
            }


        }

        public void MoveCamera(Vector2 movePosition)

        {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }


        private void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < MinZoom)
            {
                Zoom = MinZoom;
            }
            if (Zoom > MaxZoom)
            {
                Zoom = MaxZoom;
            }
        }


        public void UpdateCamera(Viewport bounds)
        {


            switch (this.Type)
            {
                case CameraType.Screen:
                    UpdateMatrix(bounds.Bounds);
                    this.Position = new Vector2(0, 0);
                    break;
                case CameraType.Follow:
                    UpdateMatrix(bounds.Bounds);
                    this.Position = LevelManager.Instance.GetCurrentPlayerInLevel().Position;

                    // Mouse scroll zoom
                    previousMouseWheelValue = currentMouseWheelValue;
                    currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

                    if (currentMouseWheelValue > previousMouseWheelValue)
                    {
                        AdjustZoom(.05f);
                    }

                    if (currentMouseWheelValue < previousMouseWheelValue)
                    {
                        AdjustZoom(-.05f);
                    }
                    break;
            }


        }

    }
}
