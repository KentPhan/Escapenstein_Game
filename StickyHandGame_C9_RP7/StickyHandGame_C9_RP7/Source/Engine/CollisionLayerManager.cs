using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Engine
{
    public class CollisionLayerManager
    {
        // TODO Make this later
        /// <summary>
        /// The instance
        /// </summary>
        private static CollisionLayerManager _instance;
        public static CollisionLayerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollisionLayerManager();
                }
                return _instance;
            }
        }

        private CollisionLayerManager()
        {
            this.StaticLayers = new Dictionary<int, Entity>();
        }

        public Entity PlayerEntity { get; set; }

        private Dictionary<int, Entity> StaticLayers;





    }
}
