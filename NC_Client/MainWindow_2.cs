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
        Resourses resourses;
        int curr_frame = 0;
        bool skip = false;

        #endregion

        void SaveSceneFile(string path, Scene scene)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                
                string ser_scene = JsonSerializer.Serialize(scene, new JsonSerializerOptions { IgnoreNullValues  = true});
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
                    return JsonSerializer.Deserialize<Scene>(file, new JsonSerializerOptions { IgnoreNullValues = true });
                }
            }
        }

        void ChangeFrame(Scene scene, int frame)
        {

            SetupBackgroundImage(scene, frame);
            resourses.SetupCharactersSprites(frame);
            ShowText(scene, frame, 25);
        }
        void SetupBackgroundImage(Scene scene, int frame)
        {
            BackgroundImage.Source = resourses.GetFrameBackground(frame);
        }
        async void ShowText(Scene scene, int frame, int time_del)
        {

            FrameText.Text = "";

            if(scene[frame].speaker != null)
            {
                Namebox.Text = scene[frame].speaker;
                Namebox.Foreground = resourses.GetFrameSpeakerColor(frame);
            }
            if (scene[frame].text != null)
            {

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
            Background_info back_info = new Background_info();
            Character_info char_info = new Character_info();
            Effect effect = new Effect();

            //Frame1

            back_info.background = "Class1.png";

            char_info.sprite = "Default.png";

            frames.Add(new Frame());
            frames[0].background_config = back_info;
            frames[0].characters_config = new Dictionary<string, Character_info>();
            frames[0].characters_config.Add("Monika", char_info);
            frames[0].text = "Первый фрейм!";
            frames[0].speaker = "Monika";
            frames[0].frame_type = Frame_type.TEXT;

            back_info = new Background_info();
            char_info = new Character_info();
            effect = new Effect();

            //Frame2

            back_info.background = "Class1.png";

            char_info.sprite = "Default.png";

            frames.Add(new Frame());
            frames[1].background_config = back_info;
            frames[1].characters_config = new Dictionary<string, Character_info>();
            frames[1].characters_config.Add("Monika", char_info);
            frames[1].text = "Второй!";
            frames[1].frame_type = Frame_type.TEXT;

            curr_scene = new Scene();
            curr_scene.frames = frames.ToArray();
            curr_scene.name = "First Scene";
            curr_scene.used_backgrouds = new string[] { "Class1.png" };
            curr_scene.used_sprites = new Dictionary<string, string[]>
            {
                ["Monika"] = new string[] { "Default.png" }
            };


            SaveSceneFile("script.json", curr_scene);
        }
        
    }
}
