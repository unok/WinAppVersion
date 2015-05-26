using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinAppVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WinAppVersion wav = new WinAppVersion(args);
                wav.Execute();
            } catch (WinAppVersionException e) {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
