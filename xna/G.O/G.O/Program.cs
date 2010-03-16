using System;

namespace ION
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ION game = new ION())
            {
                game.Run();
            }
        }
    }
}

