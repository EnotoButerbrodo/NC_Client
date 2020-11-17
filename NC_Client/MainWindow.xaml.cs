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
            ChangeFrame(curr_scene, curr_frame++%curr_scene.Length);
            Effects.HideLoadingSplash(LoadingSplash,1000);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //Синтетическое создание сцены
            //Синтетический фрейм

            List<Frame> frames = new List<Frame>();
            frames.Add(new Frame()
            {
                text = "Привет всем. Это первый самостоятельный фрейм. И на самом деле это ограмная честь" +
                "Иметь возможность поговрить с вами сегодня",
                character = "Monika",
                background = "Class1.png",

            });
            frames[0].sprites.Add("Monika", "Default.png");
            frames[0].chacters_size.Add("Monika", 1.0);
            frames.Add(new Frame()
            {
                text = "А это уже второй, но я волнуюсь все так же сильно, как и в первый. Безусловно, это невероятно" +
                "Наконец то мои слова были услышаны!",
                character = "Monika",
                background = "Class1.png",
            });
            frames[1].sprites.Add("Monika", "Teaching_sad.png");
            frames[1].chacters_size.Add("Monika", 1.0);

            Scene first = new Scene()
            {
                name = "FirstScene",
                used_backgrouds = new string[] { "Class1.png", "Class2.png" },
                used_sprites = new Dictionary<string, string[]>()
                {
                    ["Monika"] = new string[] { "Default.png", "Teaching_sad.png" },
                    ["Lilly"] = new string[] { "lilly_basic_cheerful.png" }
                }

            };
            first.frames = frames.ToArray();
            SaveSceneFile("script.txt", first);
            Effects.ShowLoadingSplash(LoadingSplash);
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
