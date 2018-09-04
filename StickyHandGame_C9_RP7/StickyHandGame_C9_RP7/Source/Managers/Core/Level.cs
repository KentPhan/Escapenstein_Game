using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StickyHandGame_C9_RP7.Source.Managers.Core
{
    public enum LevelEnum
    {
        Start,
        Level1,
        Credits
    }

    public class Level
    {
        public readonly string BackgroundPath;
        public readonly string ForegroundPath;

        public readonly List<Entity> ForegroundEntities;
        public readonly List<Entity> BackgroundEntities;

        public readonly Entity PlayerEntity;

        public readonly Entity PlayerStart;

        public readonly LevelEnum Enum;

        //public static Levels Level1 = new Levels(Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\NewMap.csv");
        //public static Levels Level1 = new Levels(Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\Test_Map_RPT_Foreground.csv");
        public static Level Start() => new Level(LevelEnum.Start);

        public static Level Level_1() => new Level(@"\Content\Foreground\Level1_Foreground.csv", @"\Content\Background\Level1_Background.csv", LevelEnum.Level1);
        public static Level Credits() => new Level(LevelEnum.Credits);



        private Level(LevelEnum levelEnum)
        {
            this.Enum = levelEnum;


            if (Enum == LevelEnum.Start)
            {

            }
            else if (Enum == LevelEnum.Credits)
            {

            }
        }

        private Level(string foregroundPath, string backgroundPath, LevelEnum levelEnum)
        {
            this.ForegroundPath = Environment.CurrentDirectory + foregroundPath;
            this.BackgroundPath = Environment.CurrentDirectory + backgroundPath;

            this.ForegroundEntities = BuildLevelMapOffOfCSVFile(this.ForegroundPath);

            this.PlayerStart = this.ForegroundEntities.First(entity => entity?.CollisionComponent?.Tag == Tags.PlayerStart);


            this.PlayerEntity = new PlayerEntity();
            MovePlayerToStartPosition();

            this.BackgroundEntities = BuildLevelMapOffOfCSVFile(this.BackgroundPath);

            this.Enum = levelEnum;
        }



        /// <summary>
        /// Update level.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (ForegroundEntities == null || BackgroundEntities == null)
            {
                return;
            }

            // Entity Updates
            this.PlayerEntity.Update(gameTime);
            foreach (Entity e in ForegroundEntities)
            {
                e.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws level.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {


            if (this.Enum == LevelEnum.Start || this.Enum == LevelEnum.Credits)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
                GameManager.Instance.SpriteBatch.DrawString(GameManager.Instance.Font, "Press Enter To Start", new Vector2(-80, 0), Color.Black);
                GameManager.Instance.SpriteBatch.End();
                return;
            }

            // Draw Background TODO

            if (ForegroundEntities == null || BackgroundEntities == null)
            {
                return;
            }

            // Draw Tiles
            foreach (Entity e in BackgroundEntities)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
                e.Draw(gameTime);
                GameManager.Instance.SpriteBatch.End();
            }

            // Draw Tiles
            foreach (Entity e in ForegroundEntities)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
                e.Draw(gameTime);
                GameManager.Instance.SpriteBatch.End();
            }


            // Draw Player and Chain
            GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
            PlayerEntity.Draw(gameTime);
            GameManager.Instance.SpriteBatch.End();

        }

        public void MovePlayerToStartPosition()
        {
            this.PlayerEntity.Position = this.PlayerStart.Position + new Vector2(0, -34);
            this.PlayerEntity.Reset();
        }


        #region -= Level Generation =-
        /// <summary>
        /// Builds the level off of CSV file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static List<Entity> BuildLevelMapOffOfCSVFile(string filePath)
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

            return GenerateTiles(tileGrid);
        }

        /// <summary>
        /// Generates the tiles.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        /// <returns></returns>
        private static List<Entity> GenerateTiles(List<List<Tiles>> tiles)
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
        private static Entity SpawnTile(float x, float y, Tiles tile)
        {
            Vector2 origin = new Vector2(x, y);

            string tileName = tile.ToString();
            if (tileName.Contains("TC"))
            {
                if (tileName.Contains("TR"))
                {
                    return new PlatformEntity(tileName, origin, TriangleColliderComponent.Oritation.TR);
                }
                if (tileName.Contains("TL"))
                {
                    return new PlatformEntity(tileName, origin, TriangleColliderComponent.Oritation.TL);
                }
                if (tileName.Contains("BL"))
                {
                    return new PlatformEntity(tileName, origin, TriangleColliderComponent.Oritation.BL);
                }
                if (tileName.Contains("BR"))
                {
                    return new PlatformEntity(tileName, origin, TriangleColliderComponent.Oritation.BR);
                }
            }
            else if (tileName.Contains("C"))
            {
                if (tileName.Contains("S"))
                {
                    return new PlatformEntity(tileName, origin, Layers.Static, Tags.PlayerStart);
                }
                else if (tileName.Contains("Tile_5_C"))
                {
                    return new PlatformEntity(tileName, origin, Layers.Static, Tags.CantLatch);
                }
                else if (tileName.Contains("NC"))
                {
                    if (tileName.Contains("W"))
                        return new TriggerEntity(tileName, origin, Tags.Hazard);
                    else if (tileName.Contains("K"))
                        return new TriggerEntity(tileName, origin, Tags.Goal, true);
                    else if (tileName.Contains("V"))
                        return new TriggerEntity(tileName, origin, Tags.Hazard, true);
                    else
                        return new EmptyEntity(tileName, origin);
                }
                return new PlatformEntity(tileName, origin);
            }
            return null;
        }
        #endregion
    }
}
