using System;
using Emgu.CV;
using Emgu.CV.Util;
using Tesseract;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class TeseractOcrProcessor : IProcessor<Mat, string>
    {
        public Result<string> Process(Mat input)
        {
            try
            {
                var buff = new VectorOfByte();
                CvInvoke.Imencode(".tiff", input, buff);
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadTiffFromMemory(buff.ToArray()))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            return Result.Ok(text);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }
        }
    }
}
