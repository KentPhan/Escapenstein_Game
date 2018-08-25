using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Core
{
    [Flags]
    public enum CollisionLayers
    {
        Player = 1,
        Static = 2,
        None
    }
}
