using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {

        public Frame_type frame_type { get; set; }
        public string text { get; set; }
        public string speaker { get; set; }
        public Background_info background_config { get; set; }
        public Dictionary<string, Character_info> characters_config { get; set; }

        public void AddCharacterInfo(string char_name, Character_info info)
        {
            characters_config.Add(char_name, info);
        }

        public void AddBackgroundInfo(Background_info back_info)
        {
            background_config = back_info;
        }

        
       
    }
    [Serializable]
    public class Background_info
    {
        public string background { get; set; }
        public double? size { get; set; }
        public Point? position { get; set; }
        public string script { get; set; }
    }
    [Serializable]
    public class Character_info
    {
        public string sprite { get; set; }
        public double? size { get; set; }
        public Effect effect { get; set; }
    }
    [Serializable]
    public class Effect
    {
        public Effect_type? type { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public int? speed { get; set; }
    }
    
    public enum Effect_type
    {
        ENTER,
        LEAVE,
        MOVE
    }

    public enum Frame_type
    {
        TEXT,
        EMPTY
    }
}
