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
            Console.WriteLine("Before starting, make sure the files you want to rename are " +
                "\nlocated in \"Data\\Base\" folder. For example \"#base_buildingName.tga\"");
            Console.WriteLine();
            Console.WriteLine("The program will take anything located under the base directory," +
                "\nand rename it with appropraite tags, like barbarian, carthagianian and roman. " +
                "\nThese will be located in the Output folder, where you can delete as needed");
            Console.ReadLine();
            App app = new App();
        }
    }

    public class App
    {
        public App()
        {
            Run();
        }

        private void Run()
        {
            string buildingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\base\buildings");
            List<string> fileInfo = Directory.GetFiles(buildingsPath, "*.*", SearchOption.AllDirectories).ToList();

            string outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Output");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            foreach (var file in fileInfo)
            {
                FileInfo info = new FileInfo(file);

                MoveToDirectory(info, "barbarian");
                MoveToDirectory(info, "carthaginian");
                MoveToDirectory(info, "eastern");
                MoveToDirectory(info, "egyptian");
                MoveToDirectory(info, "greek");
                MoveToDirectory(info, "roman");
            }            
        }

        private void MoveToDirectory(FileInfo info, string culture)
        {
            string directory = info.Directory.FullName;

            string pathName = info.FullName.Replace(@"Data\base", @"Output\" + culture);
            string modifiedName = info.Name.Replace("base", culture);
            pathName = pathName.Replace(info.Name, modifiedName);

            string newDirectory = directory.Replace(@"Data\base", @"Output\" + culture);
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }

            string display = pathName.Split("Output")[1];
            Console.WriteLine("creating: " + display);
            File.Copy(info.FullName, pathName, true);
        }
    }
}
