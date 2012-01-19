using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceController
{
    public class GestureGatherer
    {
        public enum GestureType { LEFT_EYE_CLOSED, RIGHT_EYE_CLOSED, MOUTH_OPENED, HEAD_ROTATION_LEFT, HEAD_ROTATION_RIGHT }

        private Dictionary<GestureType, long> lastGestureTime = new Dictionary<GestureType, long>();

        private const int TRESHOLD = 10;

        public void PushGesture(GestureType type, long epoch)
        {
            bool shouldBeServed = true;
            if (lastGestureTime.ContainsKey(type))
            {
                long lastEpoch = lastGestureTime[type];
                if (epoch - lastEpoch < TRESHOLD)
                {
                    shouldBeServed = false;
                }
            }

            if (shouldBeServed)
            {
                //send an action
                Console.WriteLine(type.ToString());
                lastGestureTime[type] = epoch;
            }
        }
    }
}
