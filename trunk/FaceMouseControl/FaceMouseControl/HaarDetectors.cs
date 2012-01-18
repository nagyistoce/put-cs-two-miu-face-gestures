using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace FaceController
{
    public class NoFaceDetectedException : Exception
    {
    }

    class HaarDetectors
    {
        private HaarCascade _faces;
        private HaarCascade _eyes;
        private HaarCascade _singleEyes;
        private HaarCascade _mouth;

        public HaarDetectors()
        {
            _faces = new HaarCascade("../../haar/haarcascade_frontalface_alt_tree.xml");
            _eyes = new HaarCascade("../../haar/haarcascade_mcs_eyepair_big.xml");
            _singleEyes = new HaarCascade("../../haar/haarcascade_eye.xml");
            _mouth = new HaarCascade("../../haar/Mouth.xml");
        }

        public MCvAvgComp DetectFace(FrameData data)
        {
            MCvAvgComp[] facesDetected = _faces.Detect(data.GrayFrame, 1.1, 1, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT, new Size(20, 20));
            if (facesDetected.Length == 1)
            {
                MCvAvgComp face = facesDetected[0];
                return face;
            }
            else if (facesDetected.Length > 1)
            {
                Console.WriteLine("HaarDetectors.DetectFace: more than two faces detected. Possible errors");
                MCvAvgComp face = facesDetected[0];
                return face;
            }
            throw new NoFaceDetectedException();
        }

        public MCvAvgComp[] DetectEyes(FrameData data)
        {
            MCvAvgComp face = data.Face;
            Int32 yCoordStartSearchEyes = face.rect.Top + (face.rect.Height * 3 / 11);
            Point startingPointSearchEyes = new Point(face.rect.X, yCoordStartSearchEyes);
            Size searchEyesAreaSize = new Size(face.rect.Width, (face.rect.Height * 3 / 11));
            Rectangle possibleROI_eyes = new Rectangle(startingPointSearchEyes, searchEyesAreaSize);

            data.GrayFrame.ROI = possibleROI_eyes;
            data.EyesROI = possibleROI_eyes;
            MCvAvgComp[] eyesDetected = _singleEyes.Detect(data.GrayFrame, 1.5, 3, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(20, 20));
            data.GrayFrame.ROI = Rectangle.Empty;

            return eyesDetected;
        }
    }
}
