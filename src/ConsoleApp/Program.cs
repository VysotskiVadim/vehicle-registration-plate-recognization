using System;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.Practices.Unity;
using vrpr.DesktopCore;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    LoadFromFIleAndFindPlate(arg);
                }
            }
            else
            {
                Console.WriteLine("Please specify file names in arguments");
            }
            Console.ReadKey();
        }

        private static void LoadFromFIleAndFindPlate(string arg)
        {
            string fileName = arg;
            if (File.Exists(fileName))
            {
                try
                {
                    FindVehiclePlate(fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("File with given name not exist");
            }
        }

        private static void FindVehiclePlate(string fileName)
        {
            var containder = new UnityContainer();
            containder.RegisterInstance<IUnityContainer>(containder);
            new Registry().Register(containder);

            var registrationPlateRecognizer = containder.Resolve<IVehicleRegistrationPlateRecognizer>();

            var imageData = File.ReadAllBytes(fileName);
            
            var image = new Mat();
            CvInvoke.Imdecode(imageData, LoadImageType.AnyColor, image);

            var numbers = registrationPlateRecognizer.Process(image);
            if (numbers.Success && numbers.Value.Any())
            {
                var newImageName = Guid.NewGuid() + ".png";
                image.Save(Path.Combine(@"d:\testimage\done\", newImageName));
                Console.WriteLine($"Number was found: {string.Join(" ,",  numbers.Value)}, see result in {newImageName}");
            }
            else
            {
                Console.WriteLine("No numbers found");
            }
        }
    }
}
