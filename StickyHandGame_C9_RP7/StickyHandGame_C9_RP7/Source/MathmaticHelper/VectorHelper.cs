using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.MathmaticHelper
{
    class VectorHelper
    {
        public static Vector2 projPtoN(Vector2 P ,Vector2 N) {
            return Vector2.Dot(P,N)/N.Length() * N/N.Length();
        }
        public static Vector2 perpPtoN(Vector2 P, Vector2 N) {
            return P - VectorHelper.projPtoN(P,N);
        } 
    }
}
