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
        //public static string GetBackground(Scene scene, int frame)
        //{
        //    return scene[frame].background;
        //}


    }
}
