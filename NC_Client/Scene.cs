using System;
using System.Collections.Generic;
using System.Text;

namespace NC_Client
{   
    [Serializable]
    public class Scene
    {
        public string[] used_characters { get; set; }
        public string[][] used_sprites { get; set; }
        public string[] used_backgrouds { get; set; }
        public Frame[] frames { get; set; }
    }
}
