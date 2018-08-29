using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Cameras;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    public class CameraManager
    {
        private static CameraManager instance;
        public static CameraManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CameraManager();
                }
                return instance;
            }
        }
        public Camera camera;
        public CameraManager()
        {

        }
        public void LoadContent()
        {
            camera = Camera.Instance;
            camera.Position = new Vector2(500, 500);
        }
        public void Update()
        {
            if (GameManager.Instance.PlayerEntity != null)
                camera.Position = GameManager.Instance.PlayerEntity.Position;
            //camera.Position = GameManager.Instance.PlayerEntity.Position - new Vector2(-64, 0);
            else
                camera.Position = new Vector2(0, 0);
            camera.UpdateCamera(GameManager.Instance.GraphicsDevice.Viewport);
        }


    }
}
