using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Winforms;

namespace AssetsAdvancedEditor
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [STAThread]
        private static void Main(string[] args)
        {
            var usesConsole = AttachConsole(-1);
            if (usesConsole)
            {
                var (left, top) = Console.GetCursorPosition();
                Console.SetCursorPosition(0, top);
                Console.Write(new string(' ', left));
                Console.SetCursorPosition(0, top);
            }
            if (args.Length > 0)
            {
                CommandLineParser.Parse(args);
            }
            else
            {
                if (usesConsole)
                {
                    CommandLineParser.PrintHelp();
                }
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }
    }
}
