using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinWebServiceMonitor
{
    public static class Program
    {
        public static String[] args;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Program.args = args;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
