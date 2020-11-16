using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace NC_Client
{   
    [Serializable]
    public class Scene
    {
        public string name { get; set; }
        public string[] used_characters { get; set; }
        public string[][] used_sprites { get; set; }
        public string[] used_backgrouds { get; set; }
        public Frame[] frames { get; set; }
        
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
