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
        Level2,
        Credits
    }

    public class Level
    {
        public readonly string BackgroundPath;
        public readonly string ForegroundPath;
        public readonly string BackBackGroundName;

        public readonly List<Entity> ForegroundEntities;
        public readonly List<Entity> BackgroundEntities;
        public readonly Dictionary<int, Entity> TempEntities;
        public readonly List<Entity> ToDelete;
        public readonly Texture2D BackgroundTexture;
        public readonly Rectangle BackgroundFrame;

        public readonly Entity PlayerEntity;

        public readonly Entity PlayerStart;

        public readonly LevelEnum Enum;

        //public static Levels Level1 = new Levels(Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\NewMap.csv");
        //public static Levels Level1 = new Levels(Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\Test_Map_RPT_Foreground.csv");
        public static Level Start() => new Level(LevelEnum.Start, "TitleScreen");

        public static Level Level_1() => new Level(@"\Content\Foreground\Level1_Foreground.csv", @"\Content\Background\Level1_Background.csv", "Background", LevelEnum.Level1);
        public static Level Level_2() => new Level(@"\Content\Foreground\Level2_Foreground.csv", @"\Content\Background\Level2_Background.csv", "Background", LevelEnum.Level2);
        public static Level Credits() => new Level(LevelEnum.Credits, "Credits");



        private Level(LevelEnum levelEnum, string backBackGroundName)
        {
            this.Enum = levelEnum;
            this.BackBackGroundName = backBackGroundName;

            if (Enum == LevelEnum.Start)
            {
                this.BackgroundTexture = GameManager.Instance.Content.Load<Texture2D>($@"BackBackground\{BackBackGroundName}");
            }
            else if (Enum == LevelEnum.Credits)
            {
                this.BackgroundTexture = GameManager.Instance.Content.Load<Texture2D>($@"BackBackground\{BackBackGroundName}");
            }

            this.BackgroundFrame = new Rectangle(0, 0, GameManager.Instance.GraphicsDevice.Viewport.Width, GameManager.Instance.GraphicsDevice.Viewport.Height);
        }

        private Level(string foregroundPath, string backgroundPath, string backBackGroundName, LevelEnum levelEnum)
        {
            // Set up BackBackGround
            this.BackBackGroundName = backBackGroundName;
            this.BackgroundTexture = GameManager.Instance.Content.Load<Texture2D>($@"BackBackground\{BackBackGroundName}");
            this.BackgroundFrame = new Rectangle(0, 0, GameManager.Instance.GraphicsDevice.Viewport.Width, GameManager.Instance.GraphicsDevice.Viewport.Height);

            // Set up Paths
            this.ForegroundPath = Environment.CurrentDirectory + foregroundPath;
            this.BackgroundPath = Environment.CurrentDirectory + backgroundPath;

            // Load Foreground Entities
            this.ForegroundEntities = BuildLevelMapOffOfCSVFile(this.ForegroundPath);

            // Get Player Start Position
            this.PlayerStart = this.ForegroundEntities.First(entity => entity?.CollisionComponent?.Tag == Tags.PlayerStart);
            this.PlayerEntity = new PlayerEntity();
            MovePlayerToStartPosition();

            // Load Background Entities
            this.BackgroundEntities = BuildLevelMapOffOfCSVFile(this.BackgroundPath);

            // Create List of Temp Entites
            this.TempEntities = new Dictionary<int, Entity>();
            this.ToDelete = new List<Entity>();

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


            // Temp Updates
            foreach (Entity mustDie in ToDelete)
            {
                TempEntities.Remove(mustDie.Id);
            }

            ToDelete.Clear();
            foreach (KeyValuePair<int, Entity> e in TempEntities)
            {
                e.Value.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws level.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {

            // Draw BackBackground
            if (this.Enum == LevelEnum.Start || this.Enum == LevelEnum.Credits)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, null);
                GameManager.Instance.SpriteBatch.Draw(this.BackgroundTexture, this.BackgroundFrame, Color.White);
                GameManager.Instance.SpriteBatch.End();
                return;
            }
            else
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, null);
                GameManager.Instance.SpriteBatch.Draw(this.BackgroundTexture, this.BackgroundFrame, Color.White);
                GameManager.Instance.SpriteBatch.End();
            }

            if (ForegroundEntities == null || BackgroundEntities == null)
            {
                return;
            }

            // Draw Back Tiles
            foreach (Entity e in BackgroundEntities)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, LevelManager.Instance.CurrentCamera.Transform);
                e.Draw(gameTime);
                GameManager.Instance.SpriteBatch.End();
            }

            // Draw Fore Tiles
            foreach (Entity e in ForegroundEntities)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, LevelManager.Instance.CurrentCamera.Transform);
                e.Draw(gameTime);
                GameManager.Instance.SpriteBatch.End();
            }

            // Draw Player and Chain
            GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, LevelManager.Instance.CurrentCamera.Transform);
            PlayerEntity.Draw(gameTime);
            GameManager.Instance.SpriteBatch.End();

            // Draw Temps
            foreach (KeyValuePair<int, Entity> e in TempEntities)
            {
                GameManager.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, LevelManager.Instance.CurrentCamera.Transform);
                e.Value.Draw(gameTime);
                GameManager.Instance.SpriteBatch.End();
            }

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
                else if (tileName.Contains("G"))
                {
                    return new PlatformEntity(tileName, origin, Layers.HandOnlyStatic, Tags.None);
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
