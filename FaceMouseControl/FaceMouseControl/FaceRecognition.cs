using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using System.Windows.Forms;
using Emgu.CV.Structure;
using System.Drawing;

namespace FaceController
{
    public delegate void FrameReceiverFunc(FrameData data);

    public class FaceRecognition
    {
        private static long currentEpoch = 0;

        private static FrameData EMPTY = new FrameData() { Empty = true };

        private Capture _capture;
        
        private int msInterval;

        private double rotateAngle = 0.0f;

        private const int FRAMES_TO_RESET_ROTATION = 20;

        private int framesWithoutEyes = 0;

        private HaarDetectors detectors;

        private const double ROTATION_TRESHOLD = 1.0f;

        public FrameReceiverFunc EventListeners { get; set; }

        public FaceRecognition(int ms)
        {
            this.msInterval = ms;
            try
            {
                _capture = new Capture();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Initialization error");
                return;
            }
            this.detectors = new HaarDetectors();

            GestureGatherer gestureProcessor = new GestureGatherer();
            RotationGesture rotationGesture = new RotationGesture(this, gestureProcessor);
            MouthOpeningGesture mouthGesture = new MouthOpeningGesture(this, gestureProcessor);
        }

        public void Run()
        {
            Timer timer = new Timer();
            timer.Interval = msInterval;
            timer.Tick += new EventHandler(Process);
            timer.Enabled = true;
            timer.Start();
        }

        void Process(object sender, EventArgs e)
        {
            FrameData data = CaptureFrame();
            Boolean processed = false;
            if (data != null)
            {
                data.Epoch = currentEpoch++;
                try
                {
                    MCvAvgComp face = detectors.DetectFace(data);
                    data.Face = face;
                    MCvAvgComp mouth = detectors.DetectMouth(data);
                    data.Mouth = mouth;
                    MCvAvgComp[] eyes = detectors.DetectEyes(data);
                    data.Eyes = eyes;
                    data.EyesCount = eyes.Length;
                    if (data.EyesCount == 2)
                    {
                        framesWithoutEyes = 0;
                        bool rotated = UpdateRotation(data.Eyes);
                        data.Rotated = rotated;
                        data.Rotation = rotateAngle;
                        EventListeners(data);
                        processed = true;
                    }
                    else if (data.EyesCount > 2)
                    {
                        Console.WriteLine("More than three eyes detected. Possible errors");
                    }
                    else
                    {
                        framesWithoutEyes++;
                        if (framesWithoutEyes > FRAMES_TO_RESET_ROTATION)
                        {
                            framesWithoutEyes = 0;
                            this.rotateAngle = 0.0;
                        }
                    }
                }
                catch (NoFaceDetectedException)
                {
                }
            }            
            if (!processed)
            {
                data.Empty = true;
                //EventListeners(EMPTY);
                EventListeners(data);
            }
        }

        private bool UpdateRotation(MCvAvgComp[] eyes)
        {
            var rect1 = eyes[0].rect;
            var rect2 = eyes[1].rect;
            double tg = -(double)((rect1.Y + rect1.Height / 2) - (rect2.Y + rect2.Height / 2)) /
                (double)((rect1.X + rect1.Width / 2) - (rect2.X + rect2.Width / 2));
            double difference = Math.Atan(tg) * (180.0 / Math.PI);
            if (Math.Abs(difference) > ROTATION_TRESHOLD)
            {
                this.rotateAngle += difference;
                return true;
            }
            return false;
        }

        private FrameData CaptureFrame()
        {
            FrameData data = null;
            Image<Bgr, Byte> frame = _capture.QueryFrame();
            if (frame != null)
            {
                frame = frame.Rotate(this.rotateAngle, new Bgr(Color.Black));
                data = new FrameData();
                Image<Gray, Byte> grayFrame = frame.Convert<Gray, Byte>();
                grayFrame._EqualizeHist();
                data.Frame = frame;
                data.GrayFrame = grayFrame;
            }
            return data;
        }
    }
}
