using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

namespace NC_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            settings = LoadConfig();
            Application.Current.MainWindow.Width = settings.Window_Width;
            Application.Current.MainWindow.Height = settings.Window_Height;
            List<string> list = new List<string>() { "hi", "my", "name is", "Igor"};
            List<string> list2 = new List<string>() { "HUI", "HER" };

            String str = JsonSerializer.Serialize(list);
            str +="@" + JsonSerializer.Serialize(list2);
            string str2 = str.Remove(str.IndexOf('@'));
            string str21 = str.Remove(0, str.IndexOf('@')+1);
            MessageBox.Show(str21);
            var str3 = JsonSerializer.Deserialize<List<string>>(str2);
            var str31 = JsonSerializer.Deserialize<List<string>>(str21);
            foreach(var s in str31)
            {
                MessageBox.Show(s);
            }




        }
        #region Variables
        SettingsFile config_file = new SettingsFile();
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        List<string> needImages = new List<string>()
        {
            "Default.png"
        };
        List<BitmapImage> images = new List<BitmapImage>();
        SettingsFile settings = new SettingsFile();
        #endregion



        #region Metods
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
            ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
            {
                try
                {
                    foreach (var entry in archive.Entries)
                    {

                        if (entry.FullName.Contains(Folder) & needingImagesList.Contains(entry.Name))
                        {
                            try
                            {

                                BitmapImage src = new BitmapImage();
                                src.DownloadCompleted += (s, e) =>
                                {
                                    archive.Dispose();
                                };

                                src.BeginInit();
                                src.CacheOption = BitmapCacheOption.OnLoad;
                                src.StreamSource = entry.Open();
                                src.EndInit();
                                image_list.Add(src);
                                needingImagesList.RemoveAt(needingImagesList.FindIndex(item => item == entry.Name));

                                //MessageBox.Show(image_list[0].ToString());
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "Hui");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
    }
}
