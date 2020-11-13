using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {
        public Frame() { }
        public string text { get; set; }
        public string character { get; set; }
        public string sprite { get; set; }
        public string background { get; set; }
        public Point position { get; set; }

    }
}
