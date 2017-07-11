using System.Drawing;
using System.Windows.Forms;

namespace Wolf
{
    public partial class ImageViewer : Form
    {
        public ImageViewer(string Pic)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(Pic);
        }
    }
}