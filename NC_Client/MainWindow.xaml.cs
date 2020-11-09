using System;
using System.Collections.Generic;
using System.IO;
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
        }0
        #endregion
    }
}
