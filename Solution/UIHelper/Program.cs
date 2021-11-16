using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BuildingNamer
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
        }
    }

    public class App
    {
        public App()
        {
            Console.WriteLine("1: BuildingMaker");
            Console.WriteLine("2: UnitMaker");

            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                BuildingMaker maker = new BuildingMaker();
                maker.Start();
            }
            else if (choice == 2)
            {
                UnitMaker maker = new UnitMaker();
                maker.Start();
            }
        }


    }

    public class UnitMaker
    {
        private List<Unit> _units = new List<Unit>();
        List<dynamic> Entries = new List<dynamic>();
        private string _unitMakerPath;
        private string _modFolder;

        public void Start()
        {
            Console.WriteLine("Before starting, ensure you have the correct folder structure. Each folder should have a file called \"faction list\" with the names of corresponding" +
                "folders, to which the card will be copied." +
                "\nexample structure can be found in \"Data\\UnitMaker\\Base\" folder.");
            Console.ReadLine();

            _unitMakerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\UnitMaker");
            _modFolder = File.ReadAllLines(Path.Combine(_unitMakerPath, "Setup.txt")).ToList()[0].Replace("Mod folder:", string.Empty).Trim();

            SetupCsv();
            GenerateUnitsFromCsvEntires();

            foreach (var unit in _units)
            {
                OutputUnit(unit);
            }
        }

        private void GenerateUnitsFromCsvEntires()
        {
            foreach (var csvEntry in Entries)
            {
                var entry = csvEntry as IDictionary<string, object>;
                object value;
                entry.TryGetValue("Unit", out value);
                string name = value as string;

                if (name != null && !name.StartsWith(";"))
                {
                    Unit newUnit = new Unit(Path.Combine(_unitMakerPath, name));
                    foreach (var key in entry.Keys)
                    {
                        object x;
                        entry.TryGetValue(key, out x);
                        string result = x as string;
                        if (result == "x")
                        {
                            newUnit.Factions.Add(key);
                        }
                    }
                    _units.Add(newUnit);
                }
            }
        }

        private void SetupCsv()
        {
            string csvPath = Path.Combine(_unitMakerPath, "Factions.csv");
            var csvFile = new FileInfo(csvPath ?? string.Empty);

            if (!csvFile.Exists || csvFile.Extension.ToLowerInvariant() != ".csv")
            {
                Console.WriteLine("Invalid file path or not pointing to an CSV-File!");
                Console.ReadKey();

                return;
            }

            using (var streamReader = new StreamReader(csvFile.FullName))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    Encoding = Encoding.UTF8
                };

                using (var csvReader = new CsvReader(streamReader, csvConfig))
                {
                    while (csvReader.Read())
                    {
                        Entries.Add(csvReader.GetRecord<dynamic>());
                    }
                }
            }
        }

        private void OutputUnit(Unit unit)
        {
            string unitsFolder = Path.Combine(_modFolder, "data\\ui\\units");
            string units_classicFolder = Path.Combine(_modFolder, "data\\ui\\units_classic");
            string unit_infoFolder = Path.Combine(_modFolder, "data\\ui\\unit_info");
            string unit_info_classicFolder = Path.Combine(_modFolder, "data\\ui\\unit_info_classic");

            foreach (var faction in unit.Factions)
            {
                foreach (var file in unit.Files)
                {
                    FileInfo info = new FileInfo(file);

                    if (info.Name.StartsWith("u_"))
                    {
                        PasteUnit(unitsFolder, file, faction, "u_");
                    }
                    else if (info.Name.StartsWith("uc_"))
                    {
                        PasteUnit(units_classicFolder, file, faction, "uc_");
                    }
                    else if (info.Name.StartsWith("ui_"))
                    {
                        PasteUnit(unit_infoFolder, file, faction, "ui_");
                    }
                    else if (info.Name.StartsWith("uic_"))
                    {
                        PasteUnit(unit_info_classicFolder, file, faction, "uic_");
                    }
                }
            }
        }

        private void PasteUnit(string unitTypeFolder, string file, string faction, string prefix)
        {
            string folder = Path.Combine(unitTypeFolder, faction);
            FileInfo fileInfo = new FileInfo(file);

            string newPath = Path.Combine(folder, fileInfo.Name);
            string modifiedName = fileInfo.Name.Replace(prefix, string.Empty);
            newPath = newPath.Replace(fileInfo.Name, modifiedName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.Copy(fileInfo.FullName, newPath, true);

        }

        private class Unit
        {
            public Unit(string path)
            {
                FolderPath = path;
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("path not found: " + path);
                    Console.WriteLine("This will result in unit graphics not being created!");
                }
                Files = Directory.GetFiles(path).ToList();
            }

            public string FolderPath;
            public List<string> Factions = new List<string>();
            public List<string> Files = new List<string>();
        }
    }

    public class BuildingMaker
    {
        public void Start()
        {
            Console.WriteLine("Before starting, make sure the files you want to rename are " +
                "\nlocated in \"Data\\BuildingMaker\\Base\" folder. For example \"#CULTUREWORD_buildingName.tga\"");
            Console.WriteLine();
            Console.WriteLine("The program will take anything located under the base directory," +
                "\nand rename it with appropraite tags, like barbarian, carthagianian and roman. " +
                "\nThese will be located in the Output folder, where you can delete as needed");

            Console.WriteLine();
            Console.WriteLine("Anything with a keyword \"CULTUREWORD\" is going to be replaced with a culture tag.");
            Console.ReadLine();

            Run();
        }

        private void Run()
        {
            string buildingMakerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\BuildingMaker");
            
            string culturesPath = Path.Combine(buildingMakerPath, "CulturesList.txt");
            List<string> cultures = File.ReadAllLines(culturesPath).ToList();

            string buildingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\BuildingMaker\base");
            List<string> fileInfo = Directory.GetFiles(buildingsPath, "*.*", SearchOption.AllDirectories).ToList();

            string outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Output");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            foreach (var file in fileInfo)
            {
                FileInfo info = new FileInfo(file);

                foreach (var culture in cultures)
                {
                    MoveToDirectory(info, culture);
                }
            }
        }

        private void MoveToDirectory(FileInfo info, string culture)
        {
            string directory = info.Directory.FullName;

            string pathName = info.FullName.Replace(@"Data\BuildingMaker\base", @"Output\" + culture);
            string modifiedName = info.Name.Replace("CULTUREWORD", culture);
            pathName = pathName.Replace(info.Name, modifiedName);

            string newDirectory = directory.Replace(@"Data\BuildingMaker\base", @"Output\" + culture);
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }

            Console.WriteLine("creating: " + pathName);
            File.Copy(info.FullName, pathName, true);
        }
    }
}

