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
            TimeSpan timeToEnterCommand = TimeSpan.FromSeconds(1);

            string ScriptsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\")) + "Scripts\\";
            string installScriptCompilationCommand = @"Ahk2exe.exe /in " + @ScriptsPath + "install.ahk";
            string unistallScriptCompilationCommand = @"Ahk2exe.exe /in " + @ScriptsPath + "unistall.ahk";

            Process cmd = new Process();
            ProcessStartInfo cmdOptions = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\AutoHotkey\Compiler",
                RedirectStandardInput = true,
                UseShellExecute = false,
            };

            cmd.StartInfo = cmdOptions;
            cmd.Start();

            cmd.StandardInput.WriteLine(installScriptCompilationCommand);
            Thread.Sleep(timeToEnterCommand);

            cmd.StandardInput.WriteLine(unistallScriptCompilationCommand);
            Thread.Sleep(timeToEnterCommand);

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
            return key != null;
        }
    }
}
