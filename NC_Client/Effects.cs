using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NC_Client
{
    public static class Effects
    {
        public static void ShowLoadingSplash(Rectangle screen)
        {
            screen.Opacity = 1;
        }
        public static void HideLoadingSplash(Rectangle screen)
        {
            screen.Opacity = 0;
        }
        async public static void HideLoadingSplash(Rectangle screen, int time_mc)
        {
            while (screen.Opacity > 0.0)
            {
                screen.Opacity -= 0.01;
                await Task.Delay(time_mc / 1000);
            }

        }
        public static void ShowCharacter(Character character)
        {
            character.image.Opacity = 1;
        }
        public static void HideCharacter(Character character)
        {
            character.image.Opacity = 0;
        }
        async public static void HideCharacter(Character character, int time_mc)
        {
            while (character.image.Opacity > 0.0)
            {
                character.image.Opacity -= 0.01;
                await Task.Delay(time_mc / 1000);
            }
        }
    }
}
