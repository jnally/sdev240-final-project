using System;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace WheelOfHangman
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WheelOfHangmanForm());
        }
    }
}