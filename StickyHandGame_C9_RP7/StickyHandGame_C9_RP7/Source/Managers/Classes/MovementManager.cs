using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Managers
{
    public class MovementManager : Manager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static MovementManager _instance;
        public static MovementManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MovementManager();
                }
                return _instance;
            }
        }

        private MovementManager()
        {

        }


        public void Initialize(List<Entity> entities)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
