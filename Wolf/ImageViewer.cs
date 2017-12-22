using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Wolf
{
    public partial class ImageViewer : Form
    {
        public ImageViewer(string Pic)
        {
            InitializeComponent();
            Image Img;
            using (var Temp = new Bitmap(Pic))
            {
                Img = new Bitmap(Temp);
            }

            pictureBox1.Image = Img;
            File.Delete(Pic);
            Closed += ImageViewer_Closed;
        }

        private void ImageViewer_Closed(object Sender, EventArgs Args)
        {
            pictureBox1.Image = null;
            pictureBox1.Dispose();
        }
    }
}