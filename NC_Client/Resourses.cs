using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using Ionic.Zip;

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

        public int CharactersCount {
            get
            {
                return characters.Count;
            }
        }
        public int BackgroundsCount
        {
            get
            {
                return backgrounds.Count;
            }
        }

        public void AddCharacter(string name, Character character)
        {
            characters.Add(name, character);
          
        }
        public bool CharacterInList(string name)
        {
            if (characters.ContainsKey(name)) return true;
            return false;

        }
        public void AddSprite(string char_name, string sprite_name, BitmapImage image)
        {
                characters[char_name].sprites.Add(sprite_name, image);
        }
        public bool SpriteInList(string char_name, string sprite_name)
        {
            if (characters[char_name].sprites.ContainsKey(sprite_name)) return true;
            return false;
        }
        public void AddBackground(string name, BitmapImage image)
        {
            if (!BackgroundInList(name))
                backgrounds.Add(name, image);
            else return;
        }
        public bool BackgroundInList(string name)
        {
            if (backgrounds.ContainsKey(name)) return true;
            return false;
        }
        public BitmapImage GetBackground(string name)
        {
               return backgrounds[name];
        }
        public Character GetCharacter(string name)
        {
            return characters[name];
        }
        public static MemoryStream ReadFromZip(string zipPath, string fileName)
        {
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                foreach (ZipEntry zipEntry in zip)
                {
                    if (zipEntry.FileName.Contains(fileName))
                    {
                        MemoryStream stream = new MemoryStream();
                        zipEntry.Extract(stream);
                        stream.Seek(0, SeekOrigin.End);
                        return stream;
                    }
                }
            }
            throw new Exception("Файл не найден");
        }

    }
}
