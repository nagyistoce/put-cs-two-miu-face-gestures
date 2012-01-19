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
                    toDraw.Draw(eyeRect, new Bgr(Color.White), 2);
                }
            }
            if (data.Mouth.rect != null)
            {
                if (data.Mouth.rect.Height != 0 && data.Mouth.rect.Width != 0)
                {
                    Rectangle mouthRect = data.Mouth.rect;
                    mouthRect.Offset(data.MouthROI.X, data.MouthROI.Y);
                    toDraw.Draw(mouthRect, new Bgr(Color.White), 2);
                }
            }
            frame.Image = toDraw;
            DrawDiff(data);
            DrawEyesDifference(data);
            lastData = data;
        }

        private void DrawEyesDifference(FrameData data)
        {
            if (lastData != null)
            {
                var diff = lastData.GrayFrame.AbsDiff(data.GrayFrame);

                Emgu.CV.Image<Gray, byte> mouthArea = null;
                if (data.Mouth.rect.Size != new Size(0,0))
                {
                    var mouthRect = data.Mouth.rect;
                    mouthRect.X += data.MouthROI.X;
                    mouthRect.Y += data.MouthROI.Y;
                    mouthArea = diff.GetSubRect(mouthRect);
                    helperBox3.Image = mouthArea;
                }

                if (data.EyesCount < 1)
                {
                    return;
                }

                Rectangle rect = data.Eyes[0].rect;
                rect.X += data.EyesROI.X;
                rect.Y += data.EyesROI.Y;
                Emgu.CV.Image<Gray, byte> eye = diff.GetSubRect(rect);
                helperBox1.Image = eye;
                if (IsBlink(eye, mouthArea))
                {
                    MessageBox.Show("Blink");
                }

                if (data.EyesCount < 2)
                {
                    return;
                }
                diff = lastData.GrayFrame.AbsDiff(data.GrayFrame);
                rect = data.Eyes[1].rect;
                rect.X += data.EyesROI.X;
                rect.Y += data.EyesROI.Y;
                eye = diff.GetSubRect(rect);
                helperBox2.Image = eye;
                if (IsBlink(eye, mouthArea))
                {
                    MessageBox.Show("Blink");
                }
            }
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
            
        }

        private void DrawHelper1(FrameData data)
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
                helperBox1.Image = mouth;
            }
        }
        private bool IsBlink(Emgu.CV.Image<Gray, byte> eye, Emgu.CV.Image<Gray, byte> mouth)
        {
            if (mouth == null) 
            { 
                return false; 
            }
            Gray eyeAvg;
            MCvScalar eyeSc;
            eye.AvgSdv(out eyeAvg, out eyeSc);

            Gray mouthAvg;
            MCvScalar mouthSc;
            mouth.AvgSdv(out mouthAvg, out mouthSc);
            label1.Text = "Mouth: " + mouthAvg.Intensity.ToString();
            label2.Text = "Eyes:" + eyeAvg.Intensity.ToString();
            return eyeAvg.Intensity > 20.0 && mouthAvg.Intensity < 10.0;
        }

        private void DrawHelper2(FrameData data)
        {
            
        }

        INPUT[] inputs;
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
            INPUT tmpInput;
            List<INPUT> ins = new List<INPUT>();
            string text = "";
            if (e.Alt)
            {
                tmpInput = new INPUT();
                tmpInput.type = WindowsAPI.INPUT_KEYBOARD;
                tmpInput.ki.dwFlags = 0;
                tmpInput.ki.wScan = (ushort)(WindowsAPI.VK_ALT & 0xff);
                ins.Add(tmpInput);
                text += "ALT +";
            }

            if (e.Shift)
            {
                tmpInput = new INPUT();
                tmpInput.type = WindowsAPI.INPUT_KEYBOARD;
                tmpInput.ki.dwFlags = 0;
                tmpInput.ki.wScan = (ushort)(WindowsAPI.VK_LSHIFT & 0xff);
                ins.Add(tmpInput);
                text += "SHIFT +";
            }
            if (e.Control)
            {
                tmpInput = new INPUT();
                tmpInput.type = WindowsAPI.INPUT_KEYBOARD;
                tmpInput.ki.dwFlags = 0;
                tmpInput.ki.wScan = (ushort)(WindowsAPI.VK_LCONTROL & 0xff);
                ins.Add(tmpInput);
                text += "CTRL +";
            }
            KeysConverter a= new KeysConverter();
            text += a.ConvertToInvariantString(e.KeyCode);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey((ushort)e.KeyValue, 0);
            if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey) return;
            tmpInput = new INPUT();
            tmpInput.type = WindowsAPI.INPUT_KEYBOARD;
            tmpInput.ki.dwFlags = 0;
            tmpInput.ki.wScan = (ushort)(scanCode & 0xff);
            ins.Add(tmpInput);
            this.inputs = ins.ToArray();
            textBox1.Text = text;
        }

        private void doAction_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (inputs!= null && inputs.Length > 0)
            {
                uint intReturn = WindowsAPI.SendInput((uint)inputs.Length, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
                if (intReturn != inputs.Length)
                {
                    throw new Exception("Could not send all keys. Send only " + intReturn + " from " + inputs.Length);
                }
            }
        }
    }
}
