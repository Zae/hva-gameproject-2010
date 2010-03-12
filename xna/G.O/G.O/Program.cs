using System;

namespace GO
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
