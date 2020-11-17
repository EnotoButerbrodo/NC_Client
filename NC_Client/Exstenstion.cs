using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace NC_Client
{
    public static class Exstenstion
    {
        public static BitmapImage toBitmapImage(this MemoryStream stream)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = stream;
            src.EndInit();
            return src;
        }

    }
}
