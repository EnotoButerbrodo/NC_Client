using System;
using System.Collections.Generic;
using System.Text;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {
        public Frame() { }
        public string text { get; set; }
        public Character[] characters { get; set; }
        public string background { get; set; }

    }
}
