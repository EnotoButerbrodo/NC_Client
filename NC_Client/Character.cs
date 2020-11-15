using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace NC_Client
{
    [Serializable]
    public class Character
    {
        public Character()
        {
            name = "Unnamed";
            nameColor = Brushes.Black;
            sprites = new Dictionary<string, BitmapImage>();
            position = new Point(0, 0);
            size = 1.0;
            image = new Image();
        }
        public string name { get; set; }
        public SolidColorBrush nameColor { get; set; }
        public Dictionary<string, BitmapImage> sprites { get; set; }
        public Point position { get; set; }
        public double size { get; set; }
        public Image image { get; set; }
        public void SetImage(string sprite)
        {
            image.Source = sprites[sprite];
        }

    }
}
