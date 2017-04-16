using System;
using System.Collections.Generic;
using System.IO;
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


namespace Launching_Interface
{
    //enum Language
    //{
    //    French, English, Spanish, Japanese
    //}

    //enum Input
    //{
    //    Controller, Keyboard
    //}

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        List<string> ListeLangueOficielle { get; set; }
        List<int> ListeInfosÀEnvoyer { get; set; }

        int LangueOficielle { get; set; }



        public SettingsPage()
        {
            ListeLangueOficielle = new List<string>();
            ListeInfosÀEnvoyer = new List<int>();

            AssocierListeEnvoyer();
            InitializeComponent();

            GérerFPS();
            GererDonnees.AAAA = true;
            GérerLangues();
            GérerRenderDistance();
            GérerSon();

            ChangerRéglages();
        }

        void AssocierListeEnvoyer()
        {

            if (GererDonnees.PremierFichier == true)
            {
                ListeInfosÀEnvoyer.Add(GererDonnees.Langue);
                ListeInfosÀEnvoyer.Add(GererDonnees.Fps);
                ListeInfosÀEnvoyer.Add(GererDonnees.RenderDistance);
                ListeInfosÀEnvoyer.Add(GererDonnees.VolMusique);
                ListeInfosÀEnvoyer.Add(GererDonnees.VolEffets);
                ListeInfosÀEnvoyer.Add(GererDonnees.FullscreenMode);
                ListeInfosÀEnvoyer.Add(GererDonnees.KeyboardMode);
            }
            else
            {
                ListeInfosÀEnvoyer = GererDonnees.ListeInfosRecus;
                ListeInfosÀEnvoyer[0] = GererDonnees.Langue;
                ListeInfosÀEnvoyer[1] = GererDonnees.Fps;
                ListeInfosÀEnvoyer[2] = GererDonnees.RenderDistance;
                ListeInfosÀEnvoyer[3] = GererDonnees.VolMusique;
                ListeInfosÀEnvoyer[4] = GererDonnees.VolEffets;
                ListeInfosÀEnvoyer[5] = GererDonnees.FullscreenMode;
                ListeInfosÀEnvoyer[6] = GererDonnees.KeyboardMode;
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            this.NavigationService.Navigate(new MainPage());

            ListeInfosÀEnvoyer[0] = GererDonnees.Langue;
            ListeInfosÀEnvoyer[1] = GererDonnees.Fps;
            ListeInfosÀEnvoyer[2] = GererDonnees.RenderDistance;
            ListeInfosÀEnvoyer[3] = GererDonnees.VolMusique;
            ListeInfosÀEnvoyer[4] = GererDonnees.VolEffets;
            ListeInfosÀEnvoyer[5] = GererDonnees.FullscreenMode;
            ListeInfosÀEnvoyer[6] = GererDonnees.KeyboardMode;

            GererDonnees.ÉcrireFichier(ListeInfosÀEnvoyer);
        }

        private void SaveSettings()
        {
            StreamWriter w = new StreamWriter("../../Saves/Settings.txt");

            w.WriteLine("Music: " + GererDonnees.VolMusique.ToString());
            w.WriteLine("Sound: " + GererDonnees.VolEffets.ToString());
            w.WriteLine("Language: " + GererDonnees.Langue.ToString());
            w.WriteLine("Render Distance: " + GererDonnees.RenderDistance.ToString());
            w.WriteLine("Frame Rate: " + GererDonnees.Fps.ToString());
            w.WriteLine("Fullscreen: " + GererDonnees.FullscreenMode.ToString());
            w.WriteLine("Input: " + GererDonnees.KeyboardMode.ToString());
            w.Close();
        }

        private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void SoundVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void RenderDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void RDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) // Render Distance
        {
            double value = 0;
            if (GererDonnees.AAAA == true)
            {
                GererDonnees.AAAA = false;
                switch (GererDonnees.RenderDistance)
                {
                    case 10:
                        value = 0;
                        break;
                    case 50:
                        value = 1.25;
                        break;
                    case 100:
                        value = 2.5;
                        break;
                    case 500:
                        value = 3.75;
                        break;
                    case 1000:
                        value = 5;
                        break;
                    case 5000:
                        value = 6.25;
                        break;
                    case 10000:
                        value = 7.5;
                        break;
                    case 50000:
                        value = 8.75;
                        break;
                    case 100000:
                        value = 10;
                        break;
                }
            }
            else
            {
                var slider = sender as Slider;
                value = slider.Value;
            }

            if (value <= 5.2)
            {
                if (value >= 0 && value <= 0.1)
                {
                    GererDonnees.RenderDistance = 10;
                    RDistanceSlider.Value = 0;
                }
                if (value > 1.2 && value < 1.3)
                {
                    GererDonnees.RenderDistance = 50;
                    RDistanceSlider.Value = 1.25;
                }
                if (value > 2.4 && value < 2.6)
                {
                    GererDonnees.RenderDistance = 100;
                    RDistanceSlider.Value = 2.5;
                }
                if (value > 3.7 && value < 3.8)
                {
                    GererDonnees.RenderDistance = 500;
                    RDistanceSlider.Value = 3.75;
                }
                if (value > 4.9 && value < 5.1)
                {
                    GererDonnees.RenderDistance = 1000;
                    RDistanceSlider.Value = 5;
                }
            }
            else
            {
                if (value > 6.2 && value < 6.3)
                {
                    GererDonnees.RenderDistance = 5000;
                    RDistanceSlider.Value = 6.25;
                }
                if (value > 7.4 && value < 7.6)
                {
                    GererDonnees.RenderDistance = 10000;
                    RDistanceSlider.Value = 7.5;
                }
                if (value > 8.7 && value < 8.8)
                {
                    GererDonnees.RenderDistance = 50000;
                    RDistanceSlider.Value = 8.75;
                }
                if (value > 9.9 && value <= 10)
                {
                    GererDonnees.RenderDistance = 100000;
                    RDistanceSlider.Value = 10;
                }
            }
            rdvalue.Text = GererDonnees.RenderDistance.ToString();

        }
        private void PerformanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;

            if (value >= 0 && value <= 0.5)
            {
                GererDonnees.Fps = 30;
                PerformanceSlider.Value = 0;
            }
            else if (value > 3.2 && value < 3.4)
            {
                GererDonnees.Fps = 60;
                PerformanceSlider.Value = 3.333333;
            }
            else if (value > 6.5 && value < 6.7)
            {
                GererDonnees.Fps = 90;
                PerformanceSlider.Value = 6.66666;
            }
            else if (value < 10 && value > 9.9)
            {
                GererDonnees.Fps = 120;
                PerformanceSlider.Value = 10;
            }
            valeurPerfo.Text = GererDonnees.Fps.ToString() + " FPS";
        }
        private void ButFull_Unchecked(object sender, RoutedEventArgs e)
        {
            GererDonnees.FullscreenMode = 0;
            if (GererDonnees.FullscreenMode == 0)
            {
                ButFull.Content = ListeLangueOficielle[30];
            }
            else if (GererDonnees.FullscreenMode == 1)
            {
                ButFull.Content = ListeLangueOficielle[29];
            }

            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
        }
        private void ButFull_Checked(object sender, RoutedEventArgs e)
        {

            GererDonnees.FullscreenMode = 1;
            GererDonnees.PremierFichier = false;

            if (GererDonnees.FullscreenMode == 0)
            {
                ButFull.Content = ListeLangueOficielle[30];
            }
            else if (GererDonnees.FullscreenMode == 1)
            {
                ButFull.Content = ListeLangueOficielle[29];
            }


            //Application.Current.MainWindow.WindowState = WindowState.Maximized;
            //Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            //Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;

            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
            Application.Current.MainWindow.Left = 0;
            Application.Current.MainWindow.Top = 0;
            Application.Current.MainWindow.Width = SystemParameters.VirtualScreenWidth;
            Application.Current.MainWindow.Height = SystemParameters.VirtualScreenHeight;
            Application.Current.MainWindow.Topmost = true;




        }
        private void ButCont_Unchecked(object sender, RoutedEventArgs e)
        {
            GererDonnees.KeyboardMode = 0;
            ChangerRéglages();
        }
        private void ButCont_Checked(object sender, RoutedEventArgs e)
        {
            GererDonnees.KeyboardMode = 1;
            GererDonnees.PremierFichier = false;
            ChangerRéglages();
        }

        // Langues
        #region 

        private void RBes_Checked(object sender, RoutedEventArgs e)
        {
            GererDonnees.Langue = 2;
            ListeLangueOficielle = GererDonnees.ListeEspagnol;
            GererDonnees.PremierFichier = false;
            ChangerRéglages();
        }
        private void RBjp_Checked(object sender, RoutedEventArgs e)
        {
            GererDonnees.Langue = 3;
            ListeLangueOficielle = GererDonnees.ListeJaponais;
            GererDonnees.PremierFichier = false;
            ChangerRéglages();
        }
        private void RBfr_Checked(object sender, RoutedEventArgs e)
        {
            GererDonnees.Langue = 0;
            ListeLangueOficielle = GererDonnees.ListeFrancais;
            ChangerRéglages();
        }
        private void RBan_Checked(object sender, RoutedEventArgs e)
        {
            GererDonnees.Langue = 1;
            ListeLangueOficielle = GererDonnees.ListeAnglais;
            GererDonnees.PremierFichier = false;
            ChangerRéglages();
        }

        void ChangerRéglages()
        {

            Lang.Text = ListeLangueOficielle[31];

            RBan.Content = ListeLangueOficielle[15];
            RBfr.Content = ListeLangueOficielle[14];
            RBes.Content = ListeLangueOficielle[16];
            RBjp.Content = ListeLangueOficielle[17];

            SEff.Text = ListeLangueOficielle[13];
            GMus.Text = ListeLangueOficielle[12];

            TitreSett.Text = ListeLangueOficielle[11];
            Backtext.Text = ListeLangueOficielle[0];
            Resettext2.Text = ListeLangueOficielle[33];

            RenD.Text = ListeLangueOficielle[18];
            Perfo.Text = ListeLangueOficielle[19];
            Inp.Text = ListeLangueOficielle[21];
            Full.Text = ListeLangueOficielle[20];

            if (GererDonnees.FullscreenMode == 1)
            {
                ButFull.Content = ListeLangueOficielle[29];
                ButFull.IsChecked = true;
            }
            else if (GererDonnees.FullscreenMode == 0)
            {
                ButFull.Content = ListeLangueOficielle[30];
                ButFull.IsChecked = false;
            }

            if (GererDonnees.KeyboardMode == 1)
            {
                ButCont.Content = ListeLangueOficielle[22];
                ButCont.IsChecked = true;
            }
            else if (GererDonnees.KeyboardMode == 0)
            {
                ButCont.Content = ListeLangueOficielle[23];
                ButCont.IsChecked = false;
            }



        }

        void GérerLangues()
        {
            if (GererDonnees.Langue == 0)
            {
                ListeLangueOficielle = GererDonnees.ListeFrancais;
                RBfr.IsChecked = true;
                RBan.IsChecked = false;
                RBes.IsChecked = false;
                RBjp.IsChecked = false;
            }
            if (GererDonnees.Langue == 1)
            {
                ListeLangueOficielle = GererDonnees.ListeAnglais;
                RBfr.IsChecked = false;
                RBan.IsChecked = true;
                RBes.IsChecked = false;
                RBjp.IsChecked = false;
            }
            if (GererDonnees.Langue == 2)
            {
                ListeLangueOficielle = GererDonnees.ListeEspagnol;
                RBfr.IsChecked = false;
                RBan.IsChecked = false;
                RBes.IsChecked = true;
                RBjp.IsChecked = false;
            }
            if (GererDonnees.Langue == 3)
            {
                ListeLangueOficielle = GererDonnees.ListeJaponais;
                RBfr.IsChecked = false;
                RBan.IsChecked = false;
                RBes.IsChecked = false;
                RBjp.IsChecked = true;
            }
        }
        //on s'en fou de ces trois la
        private void musicvalue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TitreSett_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void perfovalue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion

        void GérerFPS()
        {
            if (GererDonnees.Fps == 30)
            {
                PerformanceSlider.Value = 0;
            }
            if (GererDonnees.Fps == 60)
            {
                PerformanceSlider.Value = 3.333333;
            }
            if (GererDonnees.Fps == 90)
            {
                PerformanceSlider.Value = 6.666666;
            }
            if (GererDonnees.Fps == 120)
            {
                PerformanceSlider.Value = 10;
            }
            if (PerformanceSlider.Value < 0.2) { valeurPerfo.Text = "30 FPS"; }  // temporaire
        }
        void GérerRenderDistance()
        {
            int a = 500;
            if (GererDonnees.RenderDistance == 10) { a = 10; }
            if (GererDonnees.RenderDistance == 50) { a = 50; }
            if (GererDonnees.RenderDistance == 100) { a = 100; }
            if (GererDonnees.RenderDistance == 500) { a = 500; }
            if (GererDonnees.RenderDistance == 1000) { a = 1000; }
            if (GererDonnees.RenderDistance == 5000) { a = 5000; }
            if (GererDonnees.RenderDistance == 10000) { a = 10000; }
            if (GererDonnees.RenderDistance == 50000) { a = 50000; }
            if (GererDonnees.RenderDistance == 100000) { a = 100000; }
            RDistanceSlider.Value = a;
            GererDonnees.RenderDistance = a;
        }

        private void GameMusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            GererDonnees.VolMusique = (int)Math.Round(value, 0);
            musicvalue.Text = GererDonnees.VolMusique.ToString();
        }

        private void SoundEffectsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            GererDonnees.VolEffets = (int)Math.Round(value, 0);
            soundvalue.Text = GererDonnees.VolEffets.ToString();

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GererDonnees.PremierFichier = true;
            GererDonnees.AAAA = true;
            GererDonnees.RéglagesBase();
            ChangerRéglages();
            GérerFPS();
            GérerRenderDistance();
            GérerLangues();
            GérerSon();

        }

        private void rdvalue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void GérerSon()
        {
            GameMusicSlider.Value = GererDonnees.VolMusique;
            SoundEffectsSlider.Value = GererDonnees.VolEffets;
        }
    }
}

