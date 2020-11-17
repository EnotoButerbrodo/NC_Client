using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
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
        
        public BitmapImage GetBackground(string name)
        {
            return backgrounds[name];
        }
        public Character GetCharacter(string name)
        {
            return characters[name];
        }
        public BitmapImage GetSprite(string char_name, string sprite_name)
        {
            return characters[char_name].sprites[sprite_name];
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
        public void LoadScene(Scene scene, Canvas place)
        {
            LoadCharacters(scene, place);
            LoadBackgrounds(scene);
        }
        void LoadCharacters(Scene scene, Canvas place)
        {
            foreach (var character in scene.used_sprites)
            {
                //Если героя нет в ресурсах игры
                if (!CharacterInList(character.Key))
                {
                    //Создаем его
                    Character new_char = new Character()
                    {
                        name = character.Key,
                        //nameColor = nameColor[character.Key]
                    };
                    CreateImageForCharacter(new_char);

                    place.Children.Add(new_char.image);

                    AddCharacter(character.Key, new_char);
                }
                //Теперь нужный герои есть в ресурсах. Добавляем ему нужные спрайты
                //Добавляем каждый необходимый спрайт нужному герою
                foreach (string sprite in scene.used_sprites[character.Key])
                {
                    //Добавляем новый спрайт, только если ещё нет в ресурсах
                    if (!SpriteInList(character.Key, sprite))
                    {
                        BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                            sprite).toBitmapImage();
                        AddSprite(character.Key, sprite, image);
                    }
                }
            }
        }
        void CreateImageForCharacter(Character character)
        {
            character.image.BeginInit();
            character.image.Width = 400;
            character.image.Height = 400;
            Canvas.SetLeft(character.image, character.position.X + 250);
            Canvas.SetBottom(character.image, character.position.Y);
            character.image.Stretch = Stretch.Fill;
            character.image.EndInit();
        }
        void LoadBackgrounds(Scene scene)
        {
            foreach (string sprite in scene.used_backgrouds)
            {
                if (!BackgroundInList(sprite))
                {
                    BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                            sprite).toBitmapImage();
                    AddBackground(sprite, image);
                }
            }
        }

        void AddCharacter(string name, Character character)
        {
            characters.Add(name, character);
        }
        void AddBackground(string name, BitmapImage image)
        {
            backgrounds.Add(name, image);
        }
        void AddSprite(string char_name, string sprite_name, BitmapImage image)
        {
            characters[char_name].sprites.Add(sprite_name, image);
        }

        bool CharacterInList(string name)
        {
            return characters.ContainsKey(name);
        } 
        bool SpriteInList(string char_name, string sprite_name)
        {
            return characters[char_name].sprites.ContainsKey(sprite_name);
        }
        bool BackgroundInList(string name)
        {
            return backgrounds.ContainsKey(name);
        }
        

        public int CharactersCount
        {
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
        //BitmapImage toBitmapImage(MemoryStream stream)
        //{
        //    BitmapImage src = new BitmapImage();
        //    src.BeginInit();
        //    src.StreamSource = stream;
        //    src.EndInit();
        //    return src;
        //}
    }
}
