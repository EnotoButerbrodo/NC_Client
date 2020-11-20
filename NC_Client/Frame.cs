using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NC_Client
{
    [Serializable]
    public class Frame
    {
        [JsonPropertyName("tp")]
        public Frame_type frame_type { get; set; }
        [JsonPropertyName("t")]
        public string text { get; set; }
        [JsonPropertyName("sp")]
        public string speaker { get; set; }
        [JsonPropertyName("bg_c")]
        public Background_info background_config { get; set; }
        [JsonPropertyName("ch_c")]
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
        [JsonPropertyName("bg")]
        public string background { get; set; }
        [JsonPropertyName("sz")]
        public double? size { get; set; }
        [JsonPropertyName("pos")]
        public Point? position { get; set; }
        [JsonPropertyName("scr")]
        public string script { get; set; }
    }
    [Serializable]
    public class Character_info
    {
        [JsonPropertyName("sp")]
        public string sprite { get; set; }
        [JsonPropertyName("sz")]
        public double? size { get; set; }
        [JsonPropertyName("eff")]
        public Effect effect { get; set; }
    }
    [Serializable]
    public class Effect
    {
        [JsonPropertyName("t")]
        public Effect_type? type { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        [JsonPropertyName("spd")]
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
