using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Drawing;
using OpenCvSharp;

namespace ImageConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];
            string method = args[1];
            string action = args[2];

            Mat image = Cv2.ImRead(path);
            Mat result = new Mat();

            if(method == "canny")
            {
                result = MakeCanny(image);
            }
            else if(method == "laplasian")
            {
                result = MakeLaplasian(image);
            }
            else if(method == "thresholding")
            {
                result = MakeAdaptiveThresholding(image);
            }
            else
            {
                Console.WriteLine("Type 'canny' or 'laplasian', 'thresholding' to choose a method to make an edged image.");
            }

            if(action == "save")
            {
                string savePath = Path.GetPathRoot(path);
                savePath += "result.png";
                Cv2.ImWrite(savePath, result);
            }
            else if(action == "show")
            {
                Cv2.ImShow("Result", result);
            }
            else
            {
                Console.WriteLine("Type 'save' or 'show' to do an action.");
            }

        }

        static Mat MakeLaplasian(Mat image)
        {
            var tmp = new Mat();
            Cv2.Laplacian(image, tmp, MatType.CV_8U);
            return tmp;
        }

        static Mat ResizeImage(Mat image, float x, float y)
        {
            var tmp = new Mat();
            Cv2.Resize(image, tmp, new OpenCvSharp.Size(x, y));
            return tmp;
        }

        static Mat MakeCanny(Mat image)
        {
            Mat tmp = new Mat();
            Mat ret = new Mat();
            Mat srcGray = new Mat();

            Cv2.CvtColor(image, srcGray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(srcGray, tmp);

            double val = tmp.Mean().Val0;

            Cv2.Canny(image, ret, val * 0.66, val * 1.33);

            tmp = ret;

            ret = new Scalar(255, 255, 255, 255) - tmp;

            return ret;
        }

        static Mat MakeAdaptiveThresholding(Mat image)
        {
            Mat tmp = new Mat();
            Mat ret = new Mat();

            Mat srcGray = new Mat();

            Cv2.CvtColor(image, srcGray, ColorConversionCodes.BGR2GRAY);

            Cv2.AdaptiveThreshold(srcGray, tmp, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 5, 3);
            Cv2.MedianBlur(tmp, ret, 3);
            return ret;
        }
    }
}
