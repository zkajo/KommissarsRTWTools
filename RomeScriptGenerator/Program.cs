using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RomeScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
        }
    }

    class App
    {
        public App()
        {
            Run();
        }

        private void Run()
        {
            string regionsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"RegionNames.txt");
            string settlementsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SettlementNames.txt");
            string scriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SampleScript.txt");
            string newScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"NewScript.txt");

            List<string> settlements = File.ReadAllLines(settlementsPath).ToList();
            List<string> script = File.ReadAllLines(scriptPath).ToList();

            List<string> newScript = new List<string>();
            foreach (var name in settlements)
            {
                foreach (var line in script)
                {
                    string newLine = line.Replace("SETTLEMENT", name);
                    newScript.Add(newLine);
                }

                newScript.Add(string.Empty);
            }

            File.WriteAllLines(newScriptPath, newScript);
        }
    }
}
