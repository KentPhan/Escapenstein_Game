using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    class RenderManager
    {
        public struct PlayerAnimatedAttribute
        {
            public static int Width = 36;
            public static int Height = 36;
            public static Vector2 Scale = new Vector2(4,4);
            public static Vector2 Origin = new Vector2(Width/2,Height/2);
            public static int[] framNumber = new int[] { 4, 6, 4 };
            public static int[] playSpeed = new int[] { 500, 500, 500 };
            public static String[] Names = new string[] { "Idle", "Run", "Attack" };
        }
        public struct zombieAnimatedAttribute
        {
            public static int Width = 32;
            public static int Height = 32;
            public static Vector2 Scale = new Vector2(1, 1);
            public static Vector2 Origin = new Vector2(Width / 2, Height / 2);
            public static int[] framNumber = new int[] { 6};
            public static int[] playSpeed = new int[] { 60 };
            public static String[] Names = new string[] { "Idle" };
        }
        public static Rectangle CutFrame(int FramX, int FramY, int row, int colum)
        {
            int X = FramX * colum;
            int Y = FramY * row;
            return new Rectangle(X, Y, FramX, FramY);
        }
    }
}
