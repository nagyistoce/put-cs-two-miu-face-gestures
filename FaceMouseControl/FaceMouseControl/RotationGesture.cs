using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceController
{
    public class RotationGesture
    {
        private const int TRESHOLD = 20;

        public RotationGesture(FaceRecognition recognizer)
        {
            recognizer.EventListeners += new FrameReceiverFunc(Compute);
        }

        public void Compute(FrameData data)
        {
            if (data.Rotation < -TRESHOLD)
            {
                Console.WriteLine("LEFT rotation");
            }
            if (data.Rotation > TRESHOLD)
            {
                Console.WriteLine("RIGHT rotation");
            }
        }
    }
}
