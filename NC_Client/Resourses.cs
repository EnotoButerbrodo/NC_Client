using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ionic.Zip;
namespace NC_Client
{
    public class Resourses
    {
        public Resourses(Canvas char_place)
        {
            characters = new Dictionary<string, Character>();
            backgrounds = new Dictionary<string, BitmapImage>();
            scenes = new Scene[3];
            scene_count = 0;
       
        }
        Dictionary<string, Character> characters;
        Dictionary<string, BitmapImage> backgrounds;
        Dictionary<string, SolidColorBrush> nameColor = new Dictionary<string, SolidColorBrush>()
        {
            ["Monika"] = Brushes.Red,
            ["Lilly"] = Brushes.Green
        };
        Scene[] scenes;
        byte scene_count;

        public Character GetCharacter(string name) => characters[name];
        public Dictionary<string, BitmapImage> GetFrameSprites(int frame)
        {
            Dictionary<string, BitmapImage> frame_sprites = new Dictionary<string, BitmapImage>();
            foreach (var char_info in scenes[scene_count][frame].characters_config)
            {
                frame_sprites.Add(char_info.Key, characters[char_info.Key].sprites[char_info.Value.sprite]);
            }
            return frame_sprites;
        }
        public BitmapImage GetBitmapImage(string name) => backgrounds[name];


        public BitmapImage GetFrameBackground(int frame)
        {
            return backgrounds[scenes[scene_count][frame].background_config.background];
        }
        public Character_info GetCharacterConfig(int frame, string name)
        {
            return scenes[scene_count][frame].characters_config[name];
        }
        public BitmapImage GetCharacterSprite(int frame, string name)
        {
            return characters
        }
        public SolidColorBrush GetFrameSpeakerColor(int frame)
        {
            return nameColor[scenes[scene_count][frame].speaker] ?? Brushes.Transparent;
        }
        public BitmapImage GetSprite(int frame, string char_name, string sprite)
        {
            return characters[char_name].sprites[sprite];
        }
        public void SetupCharactersSprites(int frame)
        {
            foreach(var character in scenes[scene_count][frame].characters_config)
            {
                characters[character.Key].SetSprite(character.Value.sprite);
            }
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
                        return stream;
                    }
                }
            }
            throw new Exception("Файл не найден");
        }
        public void LoadSceneResourses(Scene scene)
        {
            scenes[scene_count] = scene;
            LoadCharactersResourses(scene);
            LoadBackgroundsResourses(scene);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LoadCharactersResourses(Scene scene)
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
                        nameColor = nameColor[character.Key]
                    };
                    CreateImageForCharacter(new_char);

                    AddCharacter(character.Key, new_char);
                }
                //Теперь нужный герои есть в ресурсах. Добавляем ему нужные спрайты
                //Добавляем каждый необходимый спрайт нужному герою
                foreach (string sprite in scene.used_sprites[character.Key])
                {
                    //Добавляем новый спрайт, только если ещё нет в ресурсах
                    if (!SpriteInList(character.Key, sprite))
                    {
                        BitmapImage image = ReadFromZip(@"Resourses\images.zip",
                            sprite).toBitmapImage();
                        AddSprite(character.Key, sprite, image);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CreateImageForCharacter(Character character)
        {
            character.image.BeginInit();
            character.image.Width = 400;
            character.image.Height = 400;
            Canvas.SetLeft(character.image, character.position.X + 250);
            Canvas.SetBottom(character.image, character.position.Y);
            character.image.Stretch = Stretch.Uniform;
            character.image.EndInit();
            char_place.Children.Add(character.image);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LoadBackgroundsResourses(Scene scene)
        {
            foreach (string sprite in scene.used_backgrouds)
            {
                if (!BackgroundInList(sprite))
                {
                    BitmapImage image = ReadFromZip(@"Resourses\images.zip",
                            sprite).toBitmapImage();
                    AddBackground(sprite, image);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AddCharacter(string name, Character character)
        {
            characters.Add(name, character);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AddBackground(string name, BitmapImage image)
        {
            backgrounds.Add(name, image);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AddSprite(string char_name, string sprite_name, BitmapImage image)
        {
            characters[char_name].sprites.Add(sprite_name, image);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool CharacterInList(string name)
        {
            return characters.ContainsKey(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool SpriteInList(string char_name, string sprite_name)
        {
            return characters[char_name].sprites.ContainsKey(sprite_name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    }
}
