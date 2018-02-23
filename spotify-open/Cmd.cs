using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace spotify_open
{
    public class Cmd
    {
        public static void RunCommandHidden(string Command)
        {
            var StartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/C {Command}"
            };

            var Process = new Process
            {
                StartInfo = StartInfo
            };
            Process.Start();
        }
    }
}
