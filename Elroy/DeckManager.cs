using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elroy
{
    public partial class DeckManager : Form
    {
        public DeckManager(string DeckName)
        {
            InitializeComponent();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!Directory.Exists("YGO_DATA")) throw new Exception("Game Data Not Extracted!",new DirectoryNotFoundException("Can't Find YGO_DATA, Run Onomatoparia? Make Sure I'm In The Same Directory As YGO_DATA?"));
            if(!Directory.Exists("cardcropHD400.jpg.zib Unpacked") || !Directory.Exists("cardcropHD401.jpg.zib Unpacked")) throw new Exception("Card Images Not Extracted!", new DirectoryNotFoundException("Can't Find cardcropHD*** Folder, Run Relinquished? Make Sure I'm In The Same Directory As cardcropHD***?"));
            //Start Previewer.
        }
    }
}
