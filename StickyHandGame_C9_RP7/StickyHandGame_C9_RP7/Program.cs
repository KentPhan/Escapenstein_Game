using System;

namespace StickyHandGame_C9_RP7
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameManager())
                game.Run();
        }
    }
}
