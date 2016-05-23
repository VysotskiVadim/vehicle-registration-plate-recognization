using Emgu.CV;

namespace vrpr.Core
{
    public interface IRecongnitionResultHander
    {
        void HandleSuccess(string result, Mat image);
        void HandleFaulure(Mat image);
    }
}
