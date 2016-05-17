using System;
using Emgu.CV;
using Emgu.CV.Util;
using Tesseract;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class TeseractOcrFilter : IFilter<Mat, char>
    {
        private readonly IDebugLogger _debugLogger;

        public TeseractOcrFilter(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<char> Process(Mat input)
        {
            try
            {
                var buff = new VectorOfByte();
                CvInvoke.Imencode(".tiff", input, buff);
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleChar;
                    engine.SetVariable("tessedit_char_whitelist", "0123456789ABEKMHOPCTYXDI");
                    using (var img = Pix.LoadTiffFromMemory(buff.ToArray()))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText()[0];

                            _debugLogger.Log(debugLogBuilder => debugLogBuilder.AddMessage("Letter").AddImage(input).AddMessage($"has been recognized as: {text}"));

                            return Result.Ok(text);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Result.Fail<char>(e.Message);
            }
        }
    }
}
