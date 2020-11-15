using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace NC_Client
{   
    [Serializable]
    public class Scene
    {
        public Scene()
        {
            backgrouds = new Dictionary<string, BitmapImage>();
            characters = new Dictionary<string, Character>();
        }
        public string name { get; set; }
        public string[] used_characters { get; set; }
        public string[][] used_sprites { get; set; }
        public string[] used_backgrouds { get; set; }
        public Frame[] frames { get; set; }
        public Dictionary<string, BitmapImage> backgrouds { get; set;}
        public Dictionary<string, Character> characters { get; set; }
        public int Length
        {
            get
            {
                return frames.Length;
            }
        }
        public Frame this[int number]
        {
            get
            {
                return frames[number];
            }
        }
    }
}
