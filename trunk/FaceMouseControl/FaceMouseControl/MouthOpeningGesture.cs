using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;

namespace FaceController
{
    public class MouthOpeningGesture
    {
        private GestureGatherer gatherer;

        public MouthOpeningGesture(FaceRecognition recognizer, GestureGatherer gatherer)
        {
            recognizer.EventListeners += new FrameReceiverFunc(Compute);
            this.gatherer = gatherer;
        }

        public void Compute(FrameData data)
        {
            Rectangle rect = data.Mouth.rect;
            if (rect.Height != 0 && rect.Width != 0)
            {
                rect.X += data.MouthROI.X;
                rect.Y += data.MouthROI.Y;
                Emgu.CV.Image<Gray, byte> mouth = data.GrayFrame.GetSubRect(rect);
                mouth._ThresholdToZero(new Gray(100));
                mouth._SmoothGaussian(3);
                mouth._Erode(4);
                Gray avg = new Gray();
                MCvScalar sdv = new MCvScalar();
                mouth.AvgSdv(out avg, out sdv);
                if (avg.Intensity < 150)
                {
                    gatherer.PushGesture(GestureGatherer.GestureType.MOUTH_OPENED, data.Epoch);
                }
            }
        }
    }
}
