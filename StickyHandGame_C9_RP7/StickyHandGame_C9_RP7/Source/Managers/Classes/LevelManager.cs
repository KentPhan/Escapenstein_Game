using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{
    public class Levels
    {
        public string Path { get; private set; }

        public static Levels Level1 = new Levels(Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\Test_Map_RPT_Foreground.csv");

        private Levels(string filePath)
        {
            this.Path = filePath;
        }
    }
    public enum Tiles
    {
        Nothing = -1,
        Tile_0_C = 0,
        Tile_1_C = 1,
        Tile_2_C = 2,
        Tile_3_C = 3,
        Tile_4_C_S = 4,
        Tile_5_C = 5,
        Tile_6_C = 6,
        Tile_7_C = 7,
        Tile_8_C = 8,
        Tile_9_C = 9,
        Tile_16_NC = 16,
        Tile_17_NC = 17,
        Tile_18_NC = 18,
        Tile_19_NC = 19,
        Tile_20_NC = 20,
        Tile_21_NC = 21,
        Tile_32_C = 32,
        Tile_33_C = 33,
        Tile_34_C = 34,
        Tile_35_C = 35,
        Tile_36_C = 36,
        Tile_37_C = 37,
        Tile_38_C = 38,
        Tile_39_C = 39,
        Tile_48_C = 48,
        Tile_49_C = 49,
        Tile_50_C = 50,
        Tile_51_C = 51,
        Tile_52_C = 52,
        Tile_53_C = 53,
        Tile_54_C = 54,
        Tile_55_C = 55,
        Tile_64_NC_W = 64,
        Tile_65_NC_K = 65,
        //TC for TriangleCollisions
        Tile_91_TC_TL = 91,
        Tile_92_TC_TR = 92,
        Tile_93_TC_BR = 93,
        Tile_94_TC_BL = 94
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

        private Vector2 _playerStart;

        private LevelManager()
        {
        }

        public void Update(GameTime gameTime)
        {

        }


        public void LoadLevel(Levels level)
        {

        }

        /// <summary>
        /// Builds the level off of CSV file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
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
                {
                    _playerStart = new Vector2(x, y - 34);
                    GameManager.Instance.PlayerEntity = new PlayerEntity { Position = _playerStart };
                }
                else if (tileName.Contains("NC"))
                {
                    if (tileName.Contains("W"))
                        return new TriggerEntity(tileName, origin, TriggerEntity.TriggerType.Restart);
                    else if (tileName.Contains("K"))
                        return new TriggerEntity(tileName, origin, TriggerEntity.TriggerType.Victory, true);
                    else
                        return new EmptyEntity(tileName, origin);
                }
                return new PlatformEntity(tileName, origin);
            }

            return null;
        }

        /// <summary>
        /// Resets the player position.
        /// </summary>
        public void ResetPlayerPosition()
        {
            Entity player = GameManager.Instance.PlayerEntity;
            player.Position = _playerStart;
            player.Reset();
        }

    }
}

