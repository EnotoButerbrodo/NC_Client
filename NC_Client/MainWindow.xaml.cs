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

        #region Variables

        SettingsFile settings = new SettingsFile();
        Dictionary<string, BitmapImage> backgrouds = new Dictionary<string, BitmapImage>();
        Dictionary<string, Character> characters = new Dictionary<string, Character>();
        Scene curr_scene;
        Dictionary<string, SolidColorBrush> nameColor = new Dictionary<string, SolidColorBrush>()
        {
            ["Monika"] = Brushes.Red,
            ["Lilly"] = Brushes.Green
        };
        Resourses resourses = new Resourses();
        int curr_frame=0;
        bool skip = false;

        #endregion


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
        //void CharactersSetup(Scene scene)
        //{
        //    //Для каждого героя, участвующего в сцене
        //    foreach (var character in scene.used_sprites)
        //    {
        //        //Если героя нет в ресурсах игры
        //        if (!resourses.CharacterInList(character.Key))
        //        {
        //            //Создаем его
        //            Character new_char = new Character()
        //            {
        //                name = character.Key,
        //                nameColor = nameColor[character.Key]
        //            };
                    
        //            //Добавляем ему image для отображения
        //            CreateImageForCharacter(new_char);
        //            //Добавляем героя в ресурсы
        //            resourses.AddCharacter(character.Key, new_char);
        //        }
        //        //Теперь нужный герои есть в ресурсах. Добавляем ему нужные спрайты
        //        //Добавляем каждый необходимый спрайт нужному герою
        //        foreach (string sprite in scene.used_sprites[character.Key])
        //        {
        //            //Добавляем новый спрайт, только если ещё нет в ресурсах
        //            if (!resourses.SpriteInList(character.Key, sprite))
        //            {
        //                BitmapImage image = Resourses.ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
        //                    sprite).toBitmapImage();
        //                resourses.AddSprite(character.Key, sprite, image);
        //            }
        //        }
        //    }
        //}
        //void CreateImageForCharacter(Character character)
        //{
        //    character.image.BeginInit();
        //    character.image.Width = 400;
        //    character.image.Height = 400;
        //    Canvas.SetLeft(character.image, character.position.X+250);
        //    Canvas.SetBottom(character.image, character.position.Y);
        //    character.image.Stretch = Stretch.Fill;
        //    character.image.EndInit();
        //    Characters_place.Children.Add(character.image);
        //}
        //void BackgroundsSetup(Scene scene)
        //{
        //    foreach(string sprite in scene.used_backgrouds)
        //    {
        //        if (!resourses.BackgroundInList(sprite))
        //        {
        //            BitmapImage image = Resourses.ReadFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
        //                    sprite).toBitmapImage();
        //            resourses.AddBackground(sprite, image);
        //        }
        //    }
        //}
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
