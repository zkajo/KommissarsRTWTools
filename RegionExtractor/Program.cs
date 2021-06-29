using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RegionEx
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
        }
    }

    class Province
    {
        public string RegionName;
        public string SettlementName;
    }

    class App
    {
        private string path;
        private List<Province> provinces = new List<Province>();

        public App()
        {
            Run();
        }

        private void Run()
        {
            Console.WriteLine("provide full path to descr_regions.txt:");
            path = Console.ReadLine();
            ReadRegions();
            SaveData();
        }

        private void ReadRegions()
        {
            provinces.Clear();

            List<string> lines = File.ReadAllLines(path).ToList();
            Province currentProvince = new Province();

            bool regionFound = false;
            foreach (var line in lines)
            {
                if (line.StartsWith(";") || line.Length == 0)
                {
                    continue;
                }

                if (regionFound)
                {
                    string settlement = line.Trim();
                    currentProvince.SettlementName = settlement;
                    provinces.Add(currentProvince);
                    regionFound = false;
                    continue;
                }

                if (line[0] == '\t')
                {
                    continue;
                }

                currentProvince = new Province();
                string region = line.Trim();
                currentProvince.RegionName = region;
                regionFound = true;
            }
        }

        private void SaveData()
        {
            string regionsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"RegionNames.txt");
            string settlementsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SettlementNames.txt");

            List<string> regionNames = new List<string>();
            List<string> settlementNames = new List<string>();
            foreach (var province in provinces)
            {
                regionNames.Add(province.RegionName);
                settlementNames.Add(province.SettlementName);
            }

            File.WriteAllLines(regionsPath, regionNames);
            File.WriteAllLines(settlementsPath, settlementNames);
        }

    }
}
