﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace NC_Client
{
    class Resourses
    {
        public Resourses()
        {
            characters = new Dictionary<string, Character>();
            backgrounds = new Dictionary<string, BitmapImage>();
        }
        Dictionary<string, Character> characters;
        Dictionary<string, BitmapImage> backgrounds;

        public void AddCharacter(string name, Character character)
        {
            if (characters.ContainsKey(name) | characters.ContainsValue(character)) return;
            characters.Add(name, character);
        }
        public void AddBackground(string name, BitmapImage image)
        {
            if (backgrounds.ContainsKey(name) | backgrounds.ContainsValue(image)) return;
            backgrounds.Add(name, image);
        }
        public BitmapImage GetBackground(string name)
        {
            if (backgrounds.ContainsKey(name))
                return backgrounds[name];
            throw new Exception("Задний фон отсуствует в ресурсах");
        }
        public Character GetCharacter(string name)
        {
            if (characters.ContainsKey(name))
                return characters[name];
            throw new Exception("Персонаж отсуствует в ресурсах");
        }
    }
}