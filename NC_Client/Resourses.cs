using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace NC_Client
{
    class Resourses
    {
        public Resourses()
        {
            characters = new Dictionary<string, Character>();
            backgrounds = new Dictionary<string, BitmapImage>();
        }
        public Dictionary<string, Character> characters;
        public Dictionary<string, BitmapImage> backgrounds;
    }
}
