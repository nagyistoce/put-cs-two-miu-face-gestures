using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceController
{
    public class FrameData
    {
        public Image<Bgr, Byte> Frame { get; set; }

        public Image<Gray, Byte> GrayFrame { get; set; }

        public MCvAvgComp Face { get; set; }

        public int EyesCount { get; set; }

        public MCvAvgComp[] Eyes { get; set; }

        public System.Drawing.Rectangle EyesROI { get; set; }

        public MCvAvgComp Mouth { get; set; }

        public System.Drawing.Rectangle MouthROI { get; set; }

        public Boolean Empty { get; set; }

        public bool Rotated { get; set; }

        public double Rotation { get; set; }
    }
}
