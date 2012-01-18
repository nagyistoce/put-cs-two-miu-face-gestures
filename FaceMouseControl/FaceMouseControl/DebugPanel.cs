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
            if (data.EyesCount < 1)
            {
                return;
            }
            var diff = lastData.GrayFrame.AbsDiff(data.GrayFrame);
            Rectangle rect = data.Eyes[0].rect;
            rect.X += data.EyesROI.X;
            rect.Y += data.EyesROI.Y;
            Emgu.CV.Image<Gray, byte> eye = diff.GetSubRect(rect);
            helperBox1.Image = eye;
        }

        private void DrawHelper2(FrameData data)
        {
            if (data.EyesCount < 2)
            {
                return; 
            }
            var diff = lastData.GrayFrame.AbsDiff(data.GrayFrame);
            Rectangle rect = data.Eyes[1].rect;
            rect.X += data.EyesROI.X;
            rect.Y += data.EyesROI.Y;
            Emgu.CV.Image<Gray, byte> eye = diff.GetSubRect(rect);
            helperBox2.Image = eye;
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
            if (inputs.Length > 0)
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
