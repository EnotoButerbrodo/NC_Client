using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace NC_Client
{   
    [Serializable]
    public class Scene
    {
        public string name { get; set; }
        [JsonPropertyName("u_sp")]
        public Dictionary<string, string[]> used_sprites { get; set; }
        [JsonPropertyName("u_bg")]
        public string[] used_backgrouds { get; set; }
        [JsonPropertyName("frs")]
        public Frame[] frames { get; set; }
        [JsonPropertyName("l")]
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
