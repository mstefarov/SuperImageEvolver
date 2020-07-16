using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperImageEvolver
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].StartsWith("pipe="))
            {
                //Debugger.Launch();
                Task.Run(() => WorkClient.Run(args[0].Substring(5))).Wait();
            }
            else
            {
                Task.Run(WorkServer.Start);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(args));
            }
        }
    }
}