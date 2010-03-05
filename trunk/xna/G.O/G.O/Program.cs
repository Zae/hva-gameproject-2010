using System;

namespace G.O
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GO game = new GO())
            {
                game.Run();
            }
        }
    }
}

