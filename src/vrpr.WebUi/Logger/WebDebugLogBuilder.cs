using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Util;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.WebUi.Logger
{
#pragma warning disable 618
    public class WebDebugLogBuilder : IDebugLogBuilder, IDebugLogProvier
    {
        private List<DebugLogModel> _logs = new List<DebugLogModel>();

        public IDebugLogBuilder AddMessage(string message)
        {
            _logs.Add(new DebugLogModel {Type = DebugLogModel.LogType.String, Content = message });
            return this;
        }

        public IDebugLogBuilder AddImage(Mat image)
        {
            var buff = new VectorOfByte();
            CvInvoke.Imencode(".png", image, buff);
            _logs.Add(new DebugLogModel { Type = DebugLogModel.LogType.Base64Image, Content = Convert.ToBase64String(buff.ToArray()) });
            return this;
        }

        public IList<DebugLogModel> GetLog()
        {
            return _logs.AsReadOnly();
        } 
    }
#pragma warning restore 618
}
