using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;
using System.IO;
using System.Windows.Markup;

namespace Launching_Interface
{
    /// <summary>
    /// Interaction logic for NewGamePage.xaml
    /// </summary>
    public partial class NewGamePage : Page
    {
        List<string> LangueOficielleLoadPage { get; set; }
        bool[] GameExists { get; set; }
        public NewGamePage()
        {
            LangueOficielleLoadPage = new List<string>();
            InitializeComponent();
            if (GererDonnees.Langue == 0) { LangueOficielleLoadPage = GererDonnees.ListeFrancais; }
            if (GererDonnees.Langue == 1) { LangueOficielleLoadPage = GererDonnees.ListeAnglais; }
            if (GererDonnees.Langue == 2) { LangueOficielleLoadPage = GererDonnees.ListeEspagnol; }
            if (GererDonnees.Langue == 3) { LangueOficielleLoadPage = GererDonnees.ListeJaponais; }
            tbtitre.Text = LangueOficielleLoadPage[1];
            BackButton.Text = LangueOficielleLoadPage[0];
            CheckForExistingGames();
            PlaceContent();
        }

        private void PlaceContent()
        {
            for (int i = 0; i < 3; ++i)
            {
                PlaceButtonsContent(i);
            }
        }

        private void PlaceButtonsContent(int i)
        {
            if (GameExists[i])
            {
                PlaceRows(i);
            }
            else
            {
                PlaceCreateImage(i);
            }
        }

        private void PlaceRows(int i)
        {
            switch (i)
            {
                case 0:
                    CreateRows(Load0, i);
                    break;
                case 1:
                    CreateRows(Load1, i);
                    break;
                case 2:
                    CreateRows(Load2, i);
                    break;
            }
        }

        private void CreateRows(Grid l, int i)
        {
            Rows e = new Rows();
            BitmapImage src;

            switch (GererDonnees.Langue)
            {
                case 0:
                    src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"../../Saves/screenshot" + i + ".png", UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    e.Image.Source = src;
                    e.Text.Text = File.ReadAllText("../../Saves/save" + i + ".txt");
                    break;
                case 1:
                    src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"../../Saves/screenshot" + i + ".png", UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    e.Image.Source = src;
                    e.Text.Text = File.ReadAllText("../../Saves/save" + i + ".txt");
                    break;
                case 2:
                    src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"../../Saves/screenshot" + i + ".png", UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    e.Image.Source = src;
                    e.Text.Text = File.ReadAllText("../../Saves/save" + i + ".txt");
                    break;
                case 3:
                    src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"../../Saves/screenshot" + i + ".png", UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    e.Image.Source = src;
                    e.Text.Text = File.ReadAllText("../../Saves/save" + i + ".txt");
                    break;
            }
            l.Children.Add(e);
            l.VerticalAlignment = VerticalAlignment.Top;
        }

        private void PlaceCreateImage(int i)
        {
            switch (i)
            {
                case 0:
                    CreateImage(Load0);
                    break;
                case 1:
                    CreateImage(Load1);
                    break;
                case 2:
                    CreateImage(Load2);
                    break;
            }
        }

        private void CreateImage(Grid l)
        {
            Create e = new Create();
            switch (GererDonnees.Langue)
            {
                case 0:
                    e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateFR.png", UriKind.Relative));
                    break;
                case 1:
                    e.Image.Source = new BitmapImage(new Uri(@"/Pictures/Create.png", UriKind.Relative));
                    break;
                case 2:
                    e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateES.png", UriKind.Relative));
                    break;
                case 3:
                    e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateJA.png", UriKind.Relative));
                    break;
            }
            l.Children.Add(e);
        }

        private void CheckForExistingGames()
        {
            StreamReader r;

            GameExists = new bool[3];
            for (int i = 0; i < 3; ++i)
            {
                r = new StreamReader("../../Saves/save" + i + ".txt");
                GameExists[i] = r.ReadLine() != "";
                r.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }

        private void Save0Button_Click(object sender, RoutedEventArgs e)
        {
            if (!GameExists[0])
            {
                CreateSave("0");
            }
            LoadSave("0");
        }

        private void LoadSave(string saveNumber)
        {
            ManagePause(saveNumber);
            //string path = "F:/programmation clg/quatrième session/HyperV/HyperV/HyperV/bin/x86/Debug/HyperV.exe";
            //string path = "C:/Users/Mathieu/Source/Repos/HyperV/HyperV/HyperV/bin/x86/Debug/HyperV.exe";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"../../../../bin/x86/Debug/HyperV.exe");
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = path;
            p.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            Process.Start(p);
            Application.Current.Shutdown();
            //Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            //Application.Current.MainWindow.ShowInTaskbar = false;
            //this.NavigationService.Navigate(new MenuDansJeu());
        }

        void CreateSave(string saveNumber)
        {
            StreamWriter writer = new StreamWriter("../../Saves/save" + saveNumber + ".txt");

            writer.WriteLine("Level: 0");
            writer.WriteLine("Position: {X:5 Y:5 Z:5}");
            writer.WriteLine("Direction: {X:5 Y:5 Z:5}");
            //writer.WriteLine("Language: " + GererDonnees.Langue);
            //writer.WriteLine("World: Lobby");
            //writer.WriteLine("Percentage: 0%");
            writer.WriteLine("Time Played: " + (new TimeSpan(0, 0, 0)).ToString());
            writer.Close();
            File.Copy("../../Saves/startscreenshot.png", "../../Saves/screenshot" + saveNumber + ".png", true);
        }

        private void ManagePause(string saveNumber)
        {
            StreamWriter writer = new StreamWriter("../../Saves/save.txt");
            writer.WriteLine(saveNumber);
            writer.WriteLine("true");
            writer.Close();
        }

        private void Save1Button_Click(object sender, RoutedEventArgs e)
        {
            if (!GameExists[1])
            {
                CreateSave("1");
            }
            LoadSave("1");
        }

        private void Save2Button_Click(object sender, RoutedEventArgs e)
        {
            if (!GameExists[2])
            {
                CreateSave("2");
            }
            LoadSave("2");
        }
    }
}
