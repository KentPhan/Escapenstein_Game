using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    public enum Tiles
    {
        Nothing,
        Tile_Dirt
    }

    public class LevelManager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static LevelManager _instance;
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelManager();
                }
                return _instance;
            }
        }

        private LevelManager()
        {
        }

        /// <summary>
        /// Generates the level.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        /// <returns></returns>
        public List<Entity> GenerateLevel( Tiles[][] tiles)
        {
            List<Entity> entities = new List<Entity>();

            for(int r = 0; r < tiles.Length; r++)
            {
                Tiles[] column = tiles[r];
                for (int c = 0; c < column.Length; c++)
                {
                    Tiles tile = column[c];
                    Entity entity = SpawnTile(c + 1, r + 1 , tile);
                    if(entity != null)
                        entities.Add(entity);
                }
            }

            return entities;
        }

        /// <summary>
        /// Spawns the tile.
        /// </summary>
        /// <param name="rowIndex">The rowIndex.</param>
        /// <param name="colIndex">The colIndex.</param>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        private Entity SpawnTile(int rowIndex, int colIndex, Tiles tile)
        {
            Vector2 origin = new Vector2(rowIndex * 32 - 16, colIndex * 32 - 16);
            switch (tile)
            {
                case Tiles.Tile_Dirt:
                    return new PlatformEntity(nameof(Tiles.Tile_Dirt), origin);
                case Tiles.Nothing:
                default:
                    return null;
            }
        }


    }
}
