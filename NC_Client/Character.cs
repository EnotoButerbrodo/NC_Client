using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NC_Client
{
    [Serializable]
    public class Character
    {
        public string name { get; set; }
        public Brushes nameColor { get; set; }
        public Dictionary<string, BitmapImage> sprites = new Dictionary<string, BitmapImage>();
        public Point position { get; set; }
        public double size { get; set; }
    }
}
