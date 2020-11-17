using System;
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
        Dictionary<string, Character> characters = new Dictionary<string, Character>();
        Scene curr_scene;
        Dictionary<string, SolidColorBrush> nameColor = new Dictionary<string, SolidColorBrush>()
        {
            ["Monika"] = Brushes.Red,
            ["Lilly"] = Brushes.Green
        };
        Resourses resourses = new Resourses();
        int curr_frame=0;

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
            ChangeFrame(curr_scene, curr_frame++%curr_scene.Length);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //Синтетическое создание сцены
            //Синтетический фрейм

            //List<Frame> frames = new List<Frame>();
            //frames.Add(new Frame()
            //{
            //    text = "Привет всем. Это первый самостоятельный фрейм",
            //    character = "Monika",
            //    background = "Class1.png",

            //});
            //frames[0].sprites.Add("Monika", "Default.png");
            //frames[0].chacters_size.Add("Monika", 1.0);
            //frames.Add(new Frame()
            //{
            //    text = "А это уже второй",
            //    character = "Monika",
            //    background = "Class2.png",
            //});
            //frames[1].sprites.Add("Monika", "Teaching_sad.png");
            //frames[1].chacters_size.Add("Monika", 1.0);
         
            //Scene first = new Scene()
            //{
            //    name = "FirstScene",
            //    used_backgrouds = new string[] { "Class1.png", "Class2.png" },
            //    used_sprites = new Dictionary<string, string[]>()
            //    {
            //        ["Monika"] = new string[] {"Default.png", "Teaching_sad.png"},
            //        ["Lilly"] = new string[] { "lilly_basic_cheerful.png" }
            //    }

            //};
            //first.frames = frames.ToArray();
            //SaveSceneFile("script.txt", first);

            curr_scene = LoadSceneFile("script.txt");

            
            CharactersSetup(curr_scene);
            BackgroundsSetup(curr_scene);



        }
        void CharactersSetup(Scene scene)
        {
            //Для каждого героя, участвующего в сцене
            foreach (var character in scene.used_sprites)
            {
                //Если героя нет в ресурсах игры
                if (!resourses.CharacterInList(character.Key))
                {
                    //Создаем его
                    Character new_char = new Character()
                    {
                        name = character.Key,
                        nameColor = nameColor[character.Key]
                    };
                    
                    //Добавляем ему image для отображения
                    CreateImageForCharacter(new_char);
                    //Добавляем героя в ресурсы
                    resourses.AddCharacter(character.Key, new_char);
                }
                //Теперь нужный герои есть в ресурсах. Добавляем ему нужные спрайты
                //Добавляем каждый необходимый спрайт нужному герою
                foreach (string sprite in scene.used_sprites[character.Key])
                {
                    //Добавляем новый спрайт, только если ещё нет в ресурсах
                    if (!resourses.SpriteInList(character.Key, sprite))
                    {
                        BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                            sprite).toBitmapImage();
                        resourses.AddSprite(character.Key, sprite, image);
                    }
                }
            }
        }
        void CreateImageForCharacter(Character character)
        {
            character.image.BeginInit();
            character.image.Width = 400;
            character.image.Height = 400;
            Canvas.SetLeft(character.image, character.position.X+250);
            Canvas.SetBottom(character.image, character.position.Y);
            character.image.Stretch = Stretch.Fill;
            character.image.EndInit();
            Characters_place.Children.Add(character.image);
        }
        void BackgroundsSetup(Scene scene)
        {
            foreach(string sprite in scene.used_backgrouds)
            {
                if (!resourses.BackgroundInList(sprite))
                {
                    BitmapImage image = ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                            sprite).toBitmapImage();
                    resourses.AddBackground(sprite, image);
                }
            }
        }
        void ChangeFrame(Scene scene, int frame)
        {

            string frame_backgroud = scene[frame].background;
            BitmapImage background_image = resourses.GetBackground(frame_backgroud);
            BackgroundImage.Source = background_image;
            foreach(var character in scene[frame].sprites)
            {
                Character.SetImage(resourses.GetCharacter(character.Key),
                    character.Value);
            }
            FrameText.Text = curr_scene[frame].text;
            LoadingSplash.Opacity = 0;
        }
    }
}
