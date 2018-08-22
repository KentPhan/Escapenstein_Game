using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.Entities.Core
{
    [Flags]
    public enum CollisionLayers
    {
        Player = 1,
        Platform = 2
    }
}
