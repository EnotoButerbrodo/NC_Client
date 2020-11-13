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

        //List<string> images = new List<string>() { "Default.png", "Class1.png" };
        //static string[] chars = new string[] { "Monika", "Sayori" };
        #region Variables
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        List<string> needImages = new List<string>();
        List<BitmapImage> scene_images = new List<BitmapImage>();
        SettingsFile settings = new SettingsFile();

        List<Frame> test_scene = new List<Frame>();
        
        #endregion


        #region Metods
        void SaveScriptFile(List<string> images_list, List<Frame> script_list, string path)
        {
            string images = JsonSerializer.Serialize(images_list);
            string script = JsonSerializer.Serialize(script_list);
            using(var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                using(var stream = new StreamWriter(fs))
                {
                    stream.Write($"{images}@{script}");
                }
            }
        }
        void LoadScriptFile(out List<string> images, out List<Frame> script, string path)
        {
            string file, im, sc;
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    file = reader.ReadToEnd();
                }
            }
            int buff = file.IndexOf('@');
            im = file.Remove(buff);
            sc = file.Remove(0, buff+1);

            images = JsonSerializer.Deserialize<List<string>>(im);

            script = JsonSerializer.Deserialize<List<Frame>>(sc);
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

        //Сформировать строку из сериализованного листа использованных картинок и листа фреймов
        //Десиализовать обратно и разбить строку обратно на два листа

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
            List<string> images = new List<string>(
                new string[]
                {
                    "Class1.png",
                    "Class2.png",
                    "Closet1.png",
                    "Koridor.png",
                    "Default.png",
                    "Flirty.png"
                }
                );
            List<Frame> script = new List<Frame>();
            script.Add(new Frame()
            {
                background = "Class1.png",
                character = "Monika",
                position = new Point(0, 0),
                sprite = "Default.png",
                text = "Приветик"

            });
            script.Add(new Frame()
            {
                background = "Class1.png",
                character = "Monika",
                position = new Point(0, 0),
                sprite = "Flirty.png",
                text = "Зая"

            });
            script.Add(new Frame()
            {
                background = "Class2.png",
                character = "Monika",
                position = new Point(0, 0),
                sprite = "Flirty.png",
                text = "Ой, где это я?"

            });
            SaveScriptFile(images, script,"script1.txt");
        }


    }
}
