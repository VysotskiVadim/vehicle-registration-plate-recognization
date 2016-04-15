using System;
using Emgu.CV;
using Emgu.CV.Util;
using Tesseract;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class TeseractOcrProcessor : IProcessor<Mat, string>
    {
        private readonly IDebugLogger _debugLogger;

        public TeseractOcrProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

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

                            _debugLogger.Log(debugLogBuilder => debugLogBuilder.AddMessage($"found text {text}"));

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
