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
        void SaveScriptFile(List<string> images_list, List<Frame> script_list)
        {
            string images = JsonSerializer.Serialize(images_list);
            string script = JsonSerializer.Serialize(script_list);
            using(var fs = File.Open("script.txt", FileMode.OpenOrCreate))
            {
                using(var stream = new StreamWriter(fs))
                {
                    stream.Write($"{images}@{script}");
                }
            }
        }
        void LoadScriptFile(out List<string> images, out List<Frame> script)
        {
            string file, im, sc;
            using (FileStream fs = File.Open("script.txt", FileMode.Open))
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
        void ReadImageFromZip(string zipPath, string Folder, List<string> needingImagesList, List<BitmapImage> image_list)
        {
           
            
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

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> im = new List<string>() { "Default.png", "Class1.png", "Class2.png" };
            List<BitmapImage> t = new List<BitmapImage>();
            foreach (var file in im)
            {
                MemoryStream zipMs = new MemoryStream();
                using (ZipFile zip = ZipFile.Read(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip"))
                {
                    foreach (ZipEntry zipEntry in zip)
                    {
                        if (zipEntry.FileName.Contains(file))
                        {
                            zipEntry.Extract(zipMs);
                        }
                    }
                }
                zipMs.Seek(0, SeekOrigin.End);
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.StreamSource = zipMs;
                src.EndInit();
                t.Add(src);
                
            }
            MessageBox.Show(t.Count.ToString());
            foreach (BitmapImage image in t)
            {
                BackgroundImage.Source = image;
                await Task.Delay(1000);
            }
        }
    }
}
