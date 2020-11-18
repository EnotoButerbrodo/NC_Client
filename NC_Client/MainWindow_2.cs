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
    public partial class MainWindow : Window
    {
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
        int curr_frame = 0;
        bool skip = false;

        #endregion

        void SaveSceneFile(string path, Scene scene)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                string ser_scene = JsonSerializer.Serialize(scene);
                using (var writer = new StreamWriter(fs))
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

        void ChangeFrame(Scene scene, int frame)
        {

            SetupBackgroundImage(scene, frame);
            SetupCharactersSprites(scene, frame);
            ShowText(scene, frame, 25);
        }
        void SetupBackgroundImage(Scene scene, int frame)
        {
            BitmapImage background_image = resourses.GetBackground(scene[frame].background);
            BackgroundImage.Source = background_image;
        }
        void SetupCharactersSprites(Scene scene, int frame)
        {
            foreach (var character in scene[frame].sprites)
            {
                Character.SetImage(resourses.GetCharacter(character.Key),
                    character.Value);
            }
        }
        async void ShowText(Scene scene, int frame, int time_del)
        {
            FrameText.Text = "";
            skip = false;
            foreach (char sign in scene[frame].text)
            {
                if (skip)
                {
                    FrameText.Text = curr_scene[frame].text;
                    break;
                }

                FrameText.Text += sign;
                if (sign == ' ') continue;
                await Task.Delay(time_del);
            }
            skip = true;
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
            return new SettingsFile()
            {
                Window_Width = Properties.Settings.Default.WindowWidth,
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
        void CreateTestScene()
        {
            //Синтетическое создание сцены
            //Синтетический фрейм

            List<Frame> frames = new List<Frame>();
            frames.Add(new Frame()
            {
                text = "Привет всем. Это первый самостоятельный фрейм. И на самом деле это ограмная честь " +
                "иметь возможность поговрить с вами сегодня",
                character = "Monika",
                background = "Class1.png",

            });
            frames[0].sprites.Add("Monika", "Default.png");
            frames[0].chacters_size.Add("Monika", 1.0);

            frames.Add(new Frame()
            {
                text = "А это уже второй, но я волнуюсь все так же сильно, как и в первый. Безусловно, это невероятно" +
                " наконец то мои слова были услышаны!",
                character = "Monika",
                background = "Class1.png",
            });
            frames[1].sprites.Add("Monika", "Teaching_sad.png");
            frames[1].chacters_size.Add("Monika", 1.0);

            frames.Add(new Frame()
            {
                text = "Третий фрейм делает безумные вещи!3333333333333333333333 ",
                character = "Monika",
                background = "Class2.png",
            });
            frames[2].sprites.Add("Monika", "Teaching_sad.png");
            frames[2].chacters_size.Add("Monika", 1.0);

            frames.Add(new Frame()
            {
                text = "Третий фрейм делает безумные вещи!3333333333333333333333 ",
                character = "Lilly",
                background = "Class2.png",
            });
            frames[3].sprites.Add("Monika", "Teaching_sad.png");
            frames[3].sprites.Add("Lilly", "lilly_basic_ara.png");
            frames[3].chacters_size.Add("Monika", 1.0);

            Scene scene = new Scene()
            {
                name = "FirstScene",
                used_backgrouds = new string[] { "Class1.png", "Class2.png" },
                used_sprites = new Dictionary<string, string[]>()
                {
                    ["Monika"] = new string[] { "Default.png", "Teaching_sad.png" },
                    ["Lilly"] = new string[] { "lilly_basic_cheerful.png", "lilly_basic_ara.png" }
                }

            };
            scene.frames = frames.ToArray();
            SaveSceneFile("script.txt", scene);
        }
    }
}
