using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FortunaTest.common
{
    class SpecialFunctions
    {
        public static void CompileScripts()
        {
            string ScriptsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\")) + "Scripts\\";
            string installScriptCompilationCommand = @"Ahk2exe.exe /in " + @ScriptsPath + "install.ahk";
            string unistallScriptCompilationCommand = @"Ahk2exe.exe /in " + @ScriptsPath + "unistall.ahk";
            Process cmd = new Process();
            ProcessStartInfo cmdOptions = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                WorkingDirectory = @"C:\Program Files\AutoHotkey\Compiler",
                RedirectStandardInput = true,
                UseShellExecute = false,
            };
            cmd.StartInfo = cmdOptions;
            cmd.Start();
            cmd.StandardInput.WriteLine(installScriptCompilationCommand);
            Thread.Sleep(1000);
            cmd.StandardInput.WriteLine(unistallScriptCompilationCommand);
            Thread.Sleep(1000);
            cmd.Close();
        }

        public static string ConvertToHex(string handler)
        {
            return (int.Parse(handler)).ToString("x");
        }

        public static bool IsInstalled()
        {
            string registryKey = @"SOFTWARE\Classes\ctfile\shell\open\command";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
                return true;
            else
                return false;
        }
    }
}
