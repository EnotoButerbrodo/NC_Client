﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ionic.Zip;
using System.Drawing;
using Point = System.Drawing.Point;

namespace NC_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Сделать нормальный класс фрейм
        //Записать сценарий
        //Прочитать сценарий в память
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Variables

        SettingsFile settings = new SettingsFile();
        Dictionary<string, BitmapImage> backgrouds = new Dictionary<string, BitmapImage>();
        Scene curr_scene;
        Dictionary<string, SolidColorBrush> nameColor = new Dictionary<string, SolidColorBrush>()
        {
            ["Monika"] = Brushes.Red,
            ["Lilly"] = Brushes.Green
        };

        #endregion


        #region Metods
        void SaveSceneFile(string path, Scene scene)
        {
            using(var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                string ser_scene = JsonSerializer.Serialize(scene);
                using(var writer = new StreamWriter(fs))
                {
                    writer.Write(ser_scene);
                }
            }
        }
        Scene LoadSceneFile(string path)
        {
            
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    string file = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<Scene>(file);
                }
            }
        }

        SettingsFile LoadConfig()
        {
            try
            {
                using (FileStream fs = new FileStream("config.json", FileMode.Open, FileAccess.Read))
                {
                    using (var stream = new StreamReader(fs))
                    {
                        string load_config = stream.ReadToEnd();
                        return JsonSerializer.Deserialize<SettingsFile>(load_config);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new SettingsFile() { Window_Width = Properties.Settings.Default.WindowWidth,
                Window_Height = Properties.Settings.Default.WindowHeight
            };
        }
        void SaveConfig(SettingsFile file)
        {
            string save_config = JsonSerializer.Serialize<SettingsFile>(file);
            try
            {
                using (FileStream fs = new FileStream("config.json", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var stream = new StreamWriter(fs))
                    {
                        stream.Write(save_config);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        MemoryStream ReadFromZip(string zipPath, string fileName)
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

        void SaveList(List<string> image_list)
        {
            string save_config = JsonSerializer.Serialize(image_list);

            try
            {
                using (FileStream fs = new FileStream("config.json", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var stream = new StreamWriter(fs))
                    {
                        stream.Write(save_config);
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        #region Buttons

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> needImage = new List<string>();
            needImage.AddRange(new string[] { "Class1.png", "Class2.png","Default.png" });
            BitmapImage image;
            Dictionary<string, BitmapImage> dic = new Dictionary<string, BitmapImage>();
            Character Monika = new Character();
            foreach (string file in needImage)
            {
                image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                file).toBitmapImage();
                Monika.sprites.Add(file, image);
                BackgroundImage.Source = Monika.sprites[file];
            }
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //Синтетическое создание сцены
            //Синтетический фрейм
            List<Frame> frames = new List<Frame>();
            frames.Add(new Frame()
            {
                text = "Привет всем. Это первый самостоятельный фрейм",
                character = "Monika",
                background = "Class1.png"
            });
            frames[0].sprites.Add("Monika", "Default.png");
            frames.Add(new Frame()
            {
                text = "А это уже второй",
                character = "Lilly",
                background = "Class1.png"
            });
            frames[1].sprites.Add("Lilly", "lilly_basic_cheerful.png");
            //Синтетическая сцена
            Scene first = new Scene()
            {
                used_characters = new string[] { "Monika", "Lilly" },
                used_backgrouds = new string[] { "Class1.png", "Class2.png" },
                used_sprites = new string[2][] { new string[] { "Default.png", "Teaching_sad.png" }, new string[] { "lilly_basic_cheerful.png" } }

            };
            first.frames = frames.ToArray();
            SaveSceneFile("script.txt", first);
     
            curr_scene = LoadSceneFile("script.txt");

            Dictionary<string, Character> characters = new Dictionary<string, Character>();
            CharactersSetup(curr_scene, characters);
            BackgroundsSetup(curr_scene, backgrouds);
            string frame_char = curr_scene.frames[0].character;
            string char_sprite = curr_scene.frames[0].sprites[frame_char];
            BitmapImage char_image = characters[frame_char].sprites[char_sprite];
           // BackgroundImage.Source = char_image;
            Image im = new Image();
            im.BeginInit();
            im.Name = "char";
            im.Width = char_image.Width / 2;
            im.Height = char_image.Height / 2;
            im.Stretch = Stretch.Fill;
            im.Source = char_image;
            im.EndInit();
            Main.Children.Add(im);

        }
        void CharactersSetup(Scene scene, Dictionary<string, Character> characters)
        {
            characters.Clear();
            foreach (string char_name in scene.used_characters)
            {
                characters.Add(char_name, new Character()
                    {
                        name = char_name,
                        nameColor = nameColor[char_name]
                    });
                foreach (var sprite in scene.used_sprites[characters.Count-1])
                {
                    BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                        sprite).toBitmapImage();
                    characters[char_name].sprites.Add(sprite, image);
                }
            }
        }
        void BackgroundsSetup(Scene scene, Dictionary<string, BitmapImage> backgrouds)
        {
            backgrouds.Clear();
            foreach(string sprite in scene.used_backgrouds)
            {
                BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                        sprite).toBitmapImage();
                backgrouds.Add(sprite, image);
            }
        }

    }
}
