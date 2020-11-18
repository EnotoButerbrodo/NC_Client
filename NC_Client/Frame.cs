using System;
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
            //chacters_size = new Dictionary<string, double>();
            //sprites = new Dictionary<string, string>();
            characters_configuration = new Dictionary<string, Character_info>();
        }
        public string text { get; set; }
        public string speaker { get; set; }
        public string background { get; set; }
        public Dictionary<string, Character_info> characters_configuration { get; set; }
       
       
    }
    [Serializable]
    public class Character_info
    {
        public string sprite { get; set; }
        public double character_size { get; set; }
        public Presense presense { get; set; }
        public Point position { get; set; }
    }
    public enum Presense
    {
        IN,
        STAY,
        OUT
    }
}
