using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StickyHandGame_C9_RP7.Source.Cameras;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    public class CameraManager
    {
        private static CameraManager instance;
        public static CameraManager Instance { get {
                if (instance == null) {
                    instance = new CameraManager();
                }
                return instance;
            } }
        public Camera camera;
        public CameraManager() {
       
        }
        public void LoadContent() {
            camera = Camera.Instance;
            camera.Position = new Vector2(500,500);
        }
        public void Update() {
            camera.UpdateCamera(GameManager.Instance.GraphicsDevice.Viewport);
        }

        
    }
}
