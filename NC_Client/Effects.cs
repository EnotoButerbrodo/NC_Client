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
        async public static void HideLoadingSplash(Rectangle screen)
        {
            while (screen.Opacity > 0.0)
            {
                screen.Opacity -= 0.05;
                await Task.Delay(1);
            }
        }
        async public static void ShowCharacter(Character character)
        {
            while (character.image.Opacity < 1D)
            {
                character.image.Opacity += 0.05;
                await Task.Delay(1);
            }
        }
        async public static void HideCharacter(Character character)
        {
            while (character.image.Opacity > 0)
            {
                character.image.Opacity -= 0.05;
                await Task.Delay(1);
            }
        }
        
        
    }
}
