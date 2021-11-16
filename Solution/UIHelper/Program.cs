using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

