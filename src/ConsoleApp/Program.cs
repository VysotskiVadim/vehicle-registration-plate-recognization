using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;

namespace ConsoleApp
{
    class Program
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
            var image = CvInvoke.Imread(fileName, LoadImageType.AnyColor);
            var grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
            var numberCascade = new CascadeClassifier("haarcascade_russian_plate_number.xml");
            var rectangles = numberCascade.DetectMultiScale(grayImage);
            foreach (var rectangle in rectangles)
            {
                CvInvoke.Rectangle(image, rectangle, new MCvScalar(255, 0, 0), 3, LineType.FourConnected);
                var cropped = new Mat(grayImage, rectangle);
                RecognizeSymbols(cropped);
            }
            if (rectangles.Any())
            {
                var newImageName = Guid.NewGuid() + ".png";
                image.Save(Path.Combine(@"d:\testimage\done\", newImageName));
                Console.WriteLine($"Number was found, see result in {newImageName}");
            }
            else
            {
                Console.WriteLine("No numbers found");
            }
        }

        private static void RecognizeSymbols(Mat cropped)
        {
            var buff = new VectorOfByte();
            CvInvoke.Imencode(".tiff", cropped, buff);
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadTiffFromMemory(buff.ToArray()))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("eng text (GetText): \r\n{0}", text);
                        }
                    }
                }
                using (var engine = new TesseractEngine(@"./tessdata", "rus", EngineMode.Default))
                {
                    using (var img = Pix.LoadTiffFromMemory(buff.ToArray()))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("ru text (GetText): \r\n{0}", text);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
