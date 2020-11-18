using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
namespace NC_Client
{
    public static class SceneReader
    {
        public static string GetText(Scene scene, int frame)
        {
            return scene[frame].text;
        }
        public static string GetSpeaker(Scene scene, int frame)
        {
            return scene[frame].speaker;
        }
        public static string GetBackground(Scene scene, int frame)
        {
            return scene[frame].background;
        }
        public static string GetCharacterSprite(Scene scene, int frame, string char_name)
        {
            return scene[frame].characters_configuration[char_name].sprite;
        }
        public static Dictionary<string, Character_info> GetAllSprites(Scene scene, int frame)
        {
            return scene[frame].characters_configuration;
        }
        public static double GetCharacterSize(Scene scene, int frame, string char_name)
        {
            return scene[frame].characters_configuration[char_name].character_size;
        }
        public static Presense GetCharacterPresense(Scene scene, int frame, string char_name)
        {
            return scene[frame].characters_configuration[char_name].presense;
        }
        public static Point GetCharacterPosition(Scene scene, int frame, string char_name)
        {
            return scene[frame].characters_configuration[char_name].position;
        }

    }
}
