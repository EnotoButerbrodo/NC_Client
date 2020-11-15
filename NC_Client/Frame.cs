﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {
        public Frame()
        {
            chacters_size = new Dictionary<string, double>();
        }
        public string text { get; set; }
        public string character { get; set; }
        public Dictionary<string, double> chacters_size { get; set; }
        public Dictionary<string, string> sprites { get; set; } = new Dictionary<string, string>();
        public string background { get; set; }
        

    }
}
