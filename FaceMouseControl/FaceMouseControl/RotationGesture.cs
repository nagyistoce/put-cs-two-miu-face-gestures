using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceController
{
    public class RotationGesture
    {
        private const int TRESHOLD = 20;

        private GestureGatherer gatherer;

        public RotationGesture(FaceRecognition recognizer, GestureGatherer gatherer)
        {
            recognizer.EventListeners += new FrameReceiverFunc(Compute);
            this.gatherer = gatherer;
        }

        public void Compute(FrameData data)
        {
            if (data.Rotation < -TRESHOLD)
            {
                gatherer.PushGesture(GestureGatherer.GestureType.HEAD_ROTATION_LEFT, data.Epoch);
            }
            if (data.Rotation > TRESHOLD)
            {
                gatherer.PushGesture(GestureGatherer.GestureType.HEAD_ROTATION_RIGHT, data.Epoch);
            }
        }
    }
}
