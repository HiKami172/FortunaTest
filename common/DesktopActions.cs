using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FortunaTest.common
{
    public class DesktopHelper
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public static void ShowDesktop()
        {
            keybd_event(0x5B, 0, 0, 0);
            keybd_event(0x4D, 0, 0, 0);
            keybd_event(0x5B, 0, 0x2, 0);
        }

        public static void ExecuteAsAdmin(string fileFullPath)
        {
            Process execution = new Process();
            ProcessStartInfo options = new ProcessStartInfo()
            {
                FileName = fileFullPath,
                UseShellExecute = true,
                Verb = "runas"
            };
            execution.StartInfo = options;
            execution.Start();
            execution.WaitForExit();
        }
    }
}
