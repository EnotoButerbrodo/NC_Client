using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NC_Client
{
    public class Character
    {
        public string name { get; set; }
        public Brushes nameColor { get; set; }
        List<BitmapImage> emotions { get; }
        public Point position { get;}
        bool visible { get; set; }
        
        public delegate void ChangeVisibleHandler();
        ChangeVisibleHandler changeVisible;
        public void ChangeVisible(bool visible)
        {
            this.visible = visible;
            if (changeVisible != null) changeVisible();
        }
        public void AddEmotion(BitmapImage image)
        {
            emotions.Add(image);
        }

    }
}
