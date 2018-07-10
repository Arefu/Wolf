using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Eyes_White_Dragon
{
    public class ArtworkModel
    {
        public int Index { get; set; }
        public Image GameImage { get; set; } 
        public string GameImageFormat { get; set; } 
        public int GameImageHeight { get; set; } 
        public int GameImageWidth { get; set; } 

        public Image ReplacementImagePath { get; set; }
        public string ReplacementImageFormat { get; set; }
        public int ReplacementImageHeight { get; set; }
        public int ReplacementImageWidth { get; set; }


    }
}
