using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    class RenderManager
    {
        public static Rectangle CutFrame(int FramX, int FramY, int row, int colum)
        {
            int X = FramX * colum;
            int Y = FramY * row;
            return new Rectangle(X, Y, FramX, FramY);
        }
    }
}
