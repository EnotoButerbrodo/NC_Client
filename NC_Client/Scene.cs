using System;
using System.Collections.Generic;
using System.Text;

namespace NC_Client
{   
    [Serializable]
    public class Scene
    {
        public string[] used_sprites;
        public string[] used_backgrouds;
        public List<Frame> frames;


    }
}
