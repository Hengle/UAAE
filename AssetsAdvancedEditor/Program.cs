using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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
                var (Left, Top) = Console.GetCursorPosition();
                Console.SetCursorPosition(0, Top);
                Console.Write(new string(' ', Left));
                Console.SetCursorPosition(0, Top);
            }
            if (args.Length > 0)
            {
                // todo
            }
            else
            {
                if (usesConsole)
                {
                    // todo
                }
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }
    }
}
