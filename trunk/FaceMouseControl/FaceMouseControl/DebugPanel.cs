using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace FaceController
{
    public partial class DebugPanel : Form
    {
        private FrameData lastData;
        public DebugPanel()
        {
            InitializeComponent();
            FaceRecognition recognizer = new FaceRecognition(75);
            recognizer.EventListeners += new FrameReceiverFunc(DrawImage);
            recognizer.EventListeners += new FrameReceiverFunc(DrawHelper1);
            recognizer.EventListeners += new FrameReceiverFunc(DrawHelper2);
            recognizer.Run();
        }

        private void DrawImage(FrameData data)
        {
            Emgu.CV.Image<Bgr, byte> toDraw = data.Frame.Clone();
            toDraw.Draw(data.Face.rect, new Bgr(Color.Yellow), 1);
            if (data.Eyes != null)
            {
                foreach (var eye in data.Eyes)
                {
                    Rectangle eyeRect = eye.rect;
                    eyeRect.Offset(data.EyesROI.X, data.EyesROI.Y);
                    toDraw.Draw(eyeRect, new Bgr(Color.DarkSeaGreen), 2);
                }
            }
            frame.Image = toDraw;
            DrawDiff(data);
        }

        private void DrawDiff(FrameData data)
        {
            if (lastData != null)
            {
                var diff = lastData.GrayFrame.AbsDiff(data.GrayFrame);
                diffFrame.Image = diff;
                //var diffSum = diff.GetSum();
                //var diffVal = (diffSum.Blue + diffSum.Red + diffSum.Green) / (diff.Height * diff.Width);
            }
            lastData = data;
        }

        private void DrawHelper1(FrameData data)
        {
        }

        private void DrawHelper2(FrameData data)
        {
        }
    }
}
