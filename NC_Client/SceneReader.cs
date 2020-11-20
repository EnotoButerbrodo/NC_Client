using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
namespace NC_Client
{
    public static class SceneReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetText(Scene scene, int frame)
        {
            return scene[frame].text;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
