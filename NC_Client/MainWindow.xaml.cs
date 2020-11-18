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
            Effects.ShowLoadingSplash(LoadingSplash);
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeFrame(curr_scene, curr_frame++ % curr_scene.Length);
            Effects.HideLoadingSplash(LoadingSplash,1000);

        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            
            Effects.ShowLoadingSplash(LoadingSplash);
            CreateTestScene();
            curr_scene = LoadSceneFile("script.txt");


            //CharactersSetup(curr_scene);
            //BackgroundsSetup(curr_scene);
            resourses.LoadScene(curr_scene, Characters_place);


        }
        private void ClickHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(skip == false)
            {
                skip = true;
                curr_frame = curr_frame++ % curr_scene.Length;
                return;
            }
            ChangeFrame(curr_scene, curr_frame++ % curr_scene.Length);

        }
    }
}
