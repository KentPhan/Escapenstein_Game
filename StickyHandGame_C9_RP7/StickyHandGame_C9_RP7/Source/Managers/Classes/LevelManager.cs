using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System.Collections.Generic;
using System.IO;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    public enum Tiles
    {
        Nothing = -1,
        Tile_0_C = 0,
        Tile_1_C = 1,
        Tile_2_C = 2,
        Tile_3_C = 3,
        Tile_4_C = 4,
        Tile_5_C = 5,
        Tile_6_C = 6,
        Tile_7_C = 7,
        Tile_8_C = 8,
        Tile_9_C = 9,
        Tile_16_C = 16,
        Tile_17_C = 17,
        Tile_19_NC = 19,
        Tile_20_NC = 20,
        Tile_21_NC = 21,
        Tile_32_C = 32,
        Tile_33_C = 33,
        Tile_34_C = 34,
        Tile_35_NC = 35,
        Tile_36_NC = 36,
        Tile_48_C_S = 48,
        Tile_49_C_W = 49
    }

    public class LevelManager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static LevelManager _instance;

        private Dictionary<int, string> fileMappings;
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


        public List<Entity> BuildLevelOffOfCSVFile(string filePath)
        {
            List<List<Tiles>> tileGrid = new List<List<Tiles>>();
            using (StreamReader reader = File.OpenText(filePath))
            {
                string currentLine;

                while ((currentLine = reader.ReadLine()) != null)
                {
                    List<Tiles> currentRow = new List<Tiles>();
                    string[] columns = currentLine.Split(',');
                    foreach (string cell in columns)
                    {
                        Tiles tile = (Tiles)int.Parse(cell.Trim());
                        currentRow.Add(tile);
                    }
                    tileGrid.Add(currentRow);
                }
            }

            return GenerateLevel(tileGrid);
        }

        /// <summary>
        /// Generates the level.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        /// <returns></returns>
        public List<Entity> GenerateLevel(List<List<Tiles>> tiles)
        {
            List<Entity> entities = new List<Entity>();

            for (int r = 0; r < tiles.Count; r++)
            {
                List<Tiles> column = tiles[r];
                for (int c = 0; c < column.Count; c++)
                {
                    Tiles tile = column[c];
                    Entity entity = SpawnTile((c + 1) * 32 - 16, (r + 1) * 32 - 16, tile);
                    if (entity != null)
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
        private Entity SpawnTile(float x, float y, Tiles tile)
        {
            Vector2 origin = new Vector2(x, y);

            string tileName = tile.ToString();
            if (tileName.Contains("C"))
            {
                if (tileName.Contains("S"))
                    GameManager.Instance.PlayerEntity = new PlayerEntity { Position = new Vector2(x, y - 34) };
                else if (tileName.Contains("NC"))
                {
                    return new EmptyEntity(tileName, origin);
                }
                return new PlatformEntity(tileName, origin);
            }

            return null;
            //switch (tile)
            //{
            //    case Tiles.Tile_Dirt:

            //    case Tiles.PlayerSpawn:
            //        GameManager.Instance.PlayerEntity = new PlayerEntity { Position = new Vector2(x, y + 34) };
            //        return new PlatformEntity(nameof(Tiles.Tile_Dirt), origin);
            //    case Tiles.Tile_0_C:
            //        break;
            //    case Tiles.Tile_1_C:
            //        break;
            //    case Tiles.Tile_2_C:
            //        break;
            //    case Tiles.Tile_3_C:
            //        break;
            //    case Tiles.Tile_4_C:
            //        break;
            //    case Tiles.Tile_5_C:
            //        break;
            //    case Tiles.Tile_6_C:
            //        break;
            //    case Tiles.Tile_7_C:
            //        break;
            //    case Tiles.Tile_8_C:
            //        break;
            //    case Tiles.Tile_9_C:

            //        break;
            //    case Tiles.Tile_16_C:
            //        break;
            //    case Tiles.Tile_17_C:
            //        break;
            //    case Tiles.Tile_19_NC:
            //        break;
            //    case Tiles.Tile_20_NC:
            //        break;
            //    case Tiles.Tile_21_NC:
            //        break;
            //    case Tiles.Tile_32_C:
            //        break;
            //    case Tiles.Tile_33_C:
            //        break;
            //    case Tiles.Tile_34_C:
            //        break;
            //    case Tiles.Tile_35_NC:
            //        break;
            //    case Tiles.Tile_36_NC:
            //        break;
            //    case Tiles.Tile_48_C_S:
            //        break;
            //    case Tiles.Tile_49_C_W:
            //        break;
            //    case Tiles.Nothing:
            //    default:
            //        return null;
            //}
        }


    }
}

