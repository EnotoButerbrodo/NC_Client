using System;
using System.Collections.Generic;
using System.Text;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {
        public Frame() { }
        public Frame(string text, string[] characters)
        {
            this.text = text;
            this.characters = characters;
        }
        public string text { get; set; }
        public string[] characters { get; set; }
    }
}
