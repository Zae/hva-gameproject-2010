using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BouncingBallApplication
{
    //
    //Start the [FluorineFx InstallDir]\Samples\Flex\SharedObject\SharedBall sample, run this application and press Connect
    //

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}