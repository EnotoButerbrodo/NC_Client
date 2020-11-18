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
            BitmapImage background_image = resourses.GetBackground(SceneReader.GetBackground(scene, frame));
            BackgroundImage.Source = background_image;
        }
        void SetupCharactersSprites(Scene scene, int frame)
        {
            foreach (var character in scene[frame].characters_configuration)
            {
                Character.SetImage(resourses.GetCharacter(character.Key),
                   SceneReader.GetCharacterSprite(scene, frame, character.Key));
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
                text = "Первый фрейм!!! Первый фрейм!!! Первый фрейм!!! Первый фрейм!!!",
                speaker = "Monika",
                background = "Class1.png",
            });
            Character_info charinfo1 = new Character_info()
            {
                sprite = "Default.png",
                character_size = 1.0,
                presense = Presense.IN,
                position = new Point(0, 0)
            };
            frames[0].characters_configuration.Add("Monika", charinfo1);

            frames.Add(new Frame()
            {
                text = "Второй фрейм!!! Второй фрейм!!! Второй фрейм!!! Второй фрейм!!!",
                speaker = "Monika",
                background = "Class1.png",
            });
            charinfo1 = new Character_info()
            {
                sprite = "Teaching.png",
                character_size = 1.0,
                presense = Presense.STAY,
                position = new Point(0, 0)
            };
            frames[1].characters_configuration.Add("Monika", charinfo1);

            frames.Add(new Frame()
            {
                text = "Третий фрейм!!! Третий фрейм!!! Третий фрейм!!! Третий фрейм!!!",
                speaker = "Monika",
                background = "Class1.png",
            });
            charinfo1 = new Character_info()
            {
                sprite = "Teaching_sad.png",
                character_size = 1.0,
                presense = Presense.OUT,
                position = new Point(0, 0)
            };
            frames[2].characters_configuration.Add("Monika", charinfo1);


            Scene scene = new Scene()
            {
                name = "FirstScene",
                used_backgrouds = new string[] { "Class1.png", "Class2.png" },
                used_sprites = new Dictionary<string, string[]>()
                {
                    ["Monika"] = new string[] { "Default.png", "Teaching.png", "Teaching_sad.png" }
                }

            };
            scene.frames = frames.ToArray();
            SaveSceneFile("script.txt", scene);
        }
    }
}
