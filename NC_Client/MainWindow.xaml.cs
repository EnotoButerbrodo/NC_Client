using System;
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
            ReadImageFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
                "Monika", needImages, images);
            BackgroundImage.Source = images[0];
        }
        #region Variables
        SettingsFile config_file = new SettingsFile();
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        List<string> needImages = new List<string>()
        {
            "Default.png"
        };
        List<BitmapImage> images = new List<BitmapImage>();
            #endregion

        #region Metods
        void LoadConfig()
        {
            string load_config;
            try
            {
                using (FileStream fs = new FileStream("config.json", FileMode.Open, FileAccess.Read))
                {
                    using (var stream = new StreamReader(fs))
                    {
                        load_config = stream.ReadToEnd();
                        config_file = JsonSerializer.Deserialize<SettingsFile>(load_config);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void SaveConfig()
        {
            string save_config = JsonSerializer.Serialize<SettingsFile>(config_file);

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

            #endregion
        #region Buttons

            #endregion
    }
}
