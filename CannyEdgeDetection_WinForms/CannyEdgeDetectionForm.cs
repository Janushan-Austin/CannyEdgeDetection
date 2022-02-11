using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CannyEdgeDetection
{
    public enum CannyPixelEdgeState
    {
        NotAnEdge = 0,
        PotentialEdge,
        IsAnEdge,
    }
    public class CannyEdgeCandidate
    {
        public PixelGradient Gradient;
        public CannyPixelEdgeState EdgeState;

        public CannyEdgeCandidate(PixelGradient pixelGradient)
        {
            Gradient = pixelGradient;
        }
    }
    public struct PixelGradient
    {
        public int Dx;
        public int Dy;
        public int ManhattanDistance;

        public PixelGradient(int dx, int dy)
        {
            Dx = dx;
            Dy = dy;
            ManhattanDistance = Math.Abs(dx) + Math.Abs(dy);
        }

    }

    public partial class CannyEdgeDetectionForm : Form
    {
        string OpenImageFilename = "";
        public CannyEdgeDetectionForm()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog imageFileDialog = new OpenFileDialog();
            imageFileDialog.Filter = "(*.jpg;*.jpeg; *.gif; *.bmp; *.png)| *.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (imageFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OpenImageFilename = new System.IO.FileInfo(imageFileDialog.FileName).Name;

                    //var bm = new Bitmap(800, 600);
                    //for (int y = 0; y < 600; y++)
                    //    for (int x = 0; x < 800; x++)
                    //        bm.SetPixel(x,y, Color.FromArgb(0,0,255));
                    //ImagePictureBox.Image = bm;
                    ImagePictureBox.Image = new Bitmap(imageFileDialog.FileName);
                    ImagePictureBox.Size = ImagePictureBox.Image.Size;
                    //SetColorComponentButtonsActive(true);
                }
                catch
                {
                    OpenImageFilename = "";
                    ImagePictureBox.Image = null;
                    //SetColorComponentButtonsActive(false);
                }
            }
        }

        private void DetectEdgesButton_Click(object sender, EventArgs e)
        {
            if(ImagePictureBox.Image == null)
            {
                return;
            }

            Bitmap copyBitmap = new Bitmap((ImagePictureBox.Image));
            copyBitmap = ConvertToGrayScale(copyBitmap);
            copyBitmap = SmoothImage(copyBitmap);
            CannyEdgeCandidate[,] candidates = CalculateGradients(copyBitmap);

            //use manhattan distance to determine if p(x,y) is edge and not actual magnitude
            NonMaximalSuppression(candidates); 
            //ThresholdingHysterisis(candidates);
            //DrawBitMapWithEdges(copyBitmap, candidates);

        }

        private void NonMaximalSuppression(CannyEdgeCandidate[,] candidates)
        {
            for(int row = 0; row < candidates.GetLength(0); row++)
            {
                for (int col = 0; col < candidates.GetLength(1); col++)
                {
                    int rightMagnitude = -1;
                    int leftMagnitude = -1;
                    CannyEdgeCandidate candidate = candidates[row, col];
                    if(candidate.Gradient.Dx * candidate.Gradient.Dy >= 0)
                    {
                        if(candidate.Gradient.Dx > candidate.Gradient.Dy)
                        {
                            if (candidate.Gradient.Dy == 0)
                            {
                                if (col + 1 < candidates.GetLength(1))
                                {
                                    rightMagnitude = candidates[row, col + 1].Gradient.ManhattanDistance;
                                }
                                if(col - 1 < 0)
                                {
                                    leftMagnitude = candidates[row, col - 1].Gradient.ManhattanDistance;
                                }
                            }
                            else
                            {
                                if (col + 1 < candidates.GetLength(1))
                                {
                                    rightMagnitude = candidates[row, col + 1].Gradient.ManhattanDistance;
                                }
                                if (col - 1 < 0)
                                {
                                    leftMagnitude = candidates[row, col - 1].Gradient.ManhattanDistance;
                                }
                            }
                        }
                        else if(candidate.Gradient.Dx < candidate.Gradient.Dy)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private CannyEdgeCandidate[,] CalculateGradients(Bitmap original)
        {
            CannyEdgeCandidate[,] candidates = new CannyEdgeCandidate[original.Height, original.Width];
            var originalData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, original.PixelFormat);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(original.PixelFormat) / 8;

            int height = originalData.Height;
            int width = originalData.Width * bytesPerPixel;
            int stride = Math.Abs(originalData.Stride);

            byte[] transformationBytes = new byte[stride * height];
            Marshal.Copy(originalData.Scan0, transformationBytes, 0, transformationBytes.Length);
            original.UnlockBits(originalData);
            int y = 0;
            int prevY = -stride;
            int nextY = stride;
            int heightStride = height * stride;
            for (int row = 0; y < heightStride; prevY = y, y  = nextY, nextY += stride, row++)
            {
                int prevX = -bytesPerPixel;
                int x = 0;
                int nextX = bytesPerPixel;
                for (int col = 0; x < width; prevX = x, x =nextX, nextX += bytesPerPixel, col++)
                {
                    int dx, dy;
                    if(prevX >= 0 && nextX < width)
                    {
                        dx = (transformationBytes[y + nextX] - transformationBytes[y + prevX]) / 2;
                    }
                    if(prevX >=0)
                    {
                        dx = -transformationBytes[y + prevX];
                    }
                    else
                    {
                        dx = transformationBytes[y + nextX];
                    }

                    if (prevY >= 0 && nextY < heightStride)
                    {
                        dy = (transformationBytes[nextY + x] - transformationBytes[prevY + x]) / 2;
                    }
                    if (prevY >= 0)
                    {
                        dy = -transformationBytes[prevY + x];
                    }
                    else
                    {
                        dy = transformationBytes[nextY + x];
                    }
                    candidates[row, col] = new CannyEdgeCandidate(new PixelGradient(dx, dy));
                }
            }

            return candidates;
        }

        private Bitmap SmoothImage(Bitmap original)
        {
            Bitmap smoothedBitmap = new Bitmap(original.Width, original.Height);
            var originalData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, original.PixelFormat);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(original.PixelFormat) / 8;

            int height = originalData.Height;
            int width = originalData.Width * bytesPerPixel;
            int stride = Math.Abs(originalData.Stride);

            byte[] transformationBytes = new byte[stride * height];
            Marshal.Copy(originalData.Scan0, transformationBytes, 0, transformationBytes.Length);
            original.UnlockBits(originalData);
            int rowm2 = 0;
            int rowm1 = stride;
            int row = 2*stride;
            int rowp1 = 3 * stride;
            int rowp2 = 4 * stride;
            //skip first 2 rows and cols
            for (int y = 2; y < height-2; y++)
            {
                int colm2 = 0;
                int colm1 = bytesPerPixel;
                int col = 2 * bytesPerPixel;
                int colp1 = 3 * bytesPerPixel;
                int colp2 = 4 * bytesPerPixel;
                for (int x = 2*bytesPerPixel; x < width-2*bytesPerPixel; x += bytesPerPixel)
                {
                    int sum1 = transformationBytes[rowm2 + colm2] + transformationBytes[rowm2 + colp2] + transformationBytes[rowp2 + colm2] + transformationBytes[rowp2 + colp2];
                    int sum4 = transformationBytes[rowm2 + colm1] + transformationBytes[rowm2 + colp1] + transformationBytes[rowp2 + colm1] + transformationBytes[rowp2 + colp1];
                    sum4 += transformationBytes[rowm1 + colm2] + transformationBytes[rowm1 + colp2] + transformationBytes[rowp1 + colm2] + transformationBytes[rowp1 + colp2];
                    int sum7 = transformationBytes[rowm2 + col] + transformationBytes[row + colp2] + transformationBytes[rowp2 + col] + transformationBytes[row + colm2];
                    int sum16 = transformationBytes[rowm1 + colm1] + transformationBytes[rowm1 + colp1] + transformationBytes[rowp1 + colm1] + transformationBytes[rowp1 + colp1];
                    int sum26 = transformationBytes[rowm1 + col] + transformationBytes[row + colp1] + transformationBytes[rowp1 + col] + transformationBytes[row + colm1];

                    int multipliedValue = sum1 + (sum4 <<  2) + sum7*7 + sum16*16 + sum26*26 + transformationBytes[row + col]*41;
                    byte smoothedValue = (byte)((multipliedValue + 136)/273);
                    transformationBytes[row + x] = smoothedValue;
                    transformationBytes[row + x + 1] = smoothedValue;
                    transformationBytes[row + x + 2] = smoothedValue;

                    colm2 += bytesPerPixel;
                    colm1 += bytesPerPixel;
                    col += bytesPerPixel;
                    colp1 += bytesPerPixel;
                    colp2 += bytesPerPixel;
                }
                rowm2 += stride;
                rowm1 += stride;
                row += stride;
                rowp1 += stride;
                rowp2 += stride;
            }
            var smoothedData = smoothedBitmap.LockBits(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, original.PixelFormat);
            Marshal.Copy(transformationBytes, 0, smoothedData.Scan0, transformationBytes.Length);
            smoothedBitmap.UnlockBits(smoothedData);
            return smoothedBitmap;
        }

        private Bitmap ConvertToGrayScale(Bitmap original)
        {
            Bitmap copyBitmap = new Bitmap(original.Width, original.Height);
            var originalData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, original.PixelFormat);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(original.PixelFormat) / 8;

            int height = originalData.Height;
            int width = originalData.Width * bytesPerPixel;
            int stride = Math.Abs(originalData.Stride);

            byte[] transformationBytes = new byte[stride * height];
            Marshal.Copy(originalData.Scan0, transformationBytes, 0, transformationBytes.Length);
            original.UnlockBits(originalData);

            for (int y = 0; y < height; y++)
            {
                int row = y * stride;
                for (int x = 0; x < width; x += bytesPerPixel)
                {
                    int blue = transformationBytes[row + x];
                    int green = transformationBytes[row + x + 1];
                    int red = transformationBytes[row + x + 2];
                    byte luminace = (byte)(red * .299 + green * .587 + blue * .114);

                    transformationBytes[row + x] = luminace;
                    transformationBytes[row + x + 1] = luminace;
                    transformationBytes[row + x + 2] = luminace;
                }
            }
            var alteredData = copyBitmap.LockBits(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, original.PixelFormat);
            Marshal.Copy(transformationBytes, 0, alteredData.Scan0, transformationBytes.Length);
            copyBitmap.UnlockBits(alteredData);
            return copyBitmap;
        }
    }
}
