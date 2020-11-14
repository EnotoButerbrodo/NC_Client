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
            List<string> list = new List<string>(new string[] { "Hi" });
            String[] myArr = (String[])list.ToArray();
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
        void SaveScriptFile(string path, Scene scene)
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
        Scene LoadScriptFile(string path)
        {
            string file;
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    file = reader.ReadToEnd();
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
            
            SaveScriptFile(images, script,"script1.txt");
        }


    }
}
