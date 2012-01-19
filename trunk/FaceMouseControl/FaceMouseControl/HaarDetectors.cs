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
    public class NoEyesDetectedException : Exception
    {
    }
    public class NoMouthDetectedException : Exception
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
            MCvAvgComp[] facesDetected = _faces.Detect(data.GrayFrame, 1.05, 1, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT, new Size(20, 20));
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
            MCvAvgComp[] eyesDetected = _eyes.Detect(data.GrayFrame, 1.15, 3, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(20, 20));
            data.GrayFrame.ROI = Rectangle.Empty;

            if (eyesDetected.Length != 0)
            {
                Rectangle eyeRect = eyesDetected[0].rect;

                eyeRect.Offset(possibleROI_eyes.X, possibleROI_eyes.Y);
                data.GrayFrame.ROI = eyeRect;

                data.GrayFrame.ROI = possibleROI_eyes;
                data.EyesROI = possibleROI_eyes;
                MCvAvgComp[] singleEyesDetected = _singleEyes.Detect(data.GrayFrame, 1.5, 3, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(20, 20));
                data.GrayFrame.ROI = Rectangle.Empty;

                return singleEyesDetected;
            }
            throw new NoEyesDetectedException();
        }

        public MCvAvgComp DetectMouth(FrameData data)
        {
            MCvAvgComp face = data.Face;
            Int32 yCoordStartSearchMouth = face.rect.Top + (face.rect.Height * 7 / 11);
            Point startingPointSearchMouth = new Point(face.rect.X, yCoordStartSearchMouth);
            Size searchMouthAreaSize = new Size(face.rect.Width, (face.rect.Height * 4 / 11));
            Rectangle possibleROI_mouth = new Rectangle(startingPointSearchMouth, searchMouthAreaSize);

            data.GrayFrame.ROI = possibleROI_mouth;
            data.MouthROI = possibleROI_mouth;
            MCvAvgComp[] mouthDetected = _mouth.Detect(data.GrayFrame, 1.15, 3, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(30, 20));
            data.GrayFrame.ROI = Rectangle.Empty;

            if (mouthDetected.Length > 0)
            {
                if (mouthDetected[0].rect.Height != 0 && mouthDetected[0].rect.Width != 0)
                {
                    var mouthRect = mouthDetected[0].rect;
                    mouthRect.Offset(possibleROI_mouth.X, possibleROI_mouth.Y);
                    data.GrayFrame.ROI = mouthRect;
                    return mouthDetected[0];
                }
            }
            throw new NoMouthDetectedException();
        }
    }
}
