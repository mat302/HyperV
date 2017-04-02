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
using System.Windows.Shapes;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Launching_Interface
{

   /// <summary>
   /// Logique d'interaction pour MenuDansJeu.xaml
   /// </summary>
   public partial class MenuDansJeu : Page
   {
      bool EstPremiereFois { get; set; }
      List<string> ListeLangueOficielle { get; set; }
      List<int> ListeInfosÀEnvoyer { get; set; }

      int LangueOficielle { get; set; }
      public MenuDansJeu()
      {
            RefreshData();

         EstPremiereFois = true;
         ListeLangueOficielle = new List<string>();
         ListeInfosÀEnvoyer = new List<int>();

            AjouterListeEnvoyer();//AssocierListeEnvoyer();

         InitializeComponent();

         BonScreenshot();

         GérerFPS();
         GererDonnees.AAAA = true;
         GérerLangues();
         GérerRenderDistance();
         GérerSon();

         ChangerRéglages();

      }

        private void RefreshData()
        {
            //StreamReader reader = new StreamReader("F:/programmation clg/quatrième session/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
            //StreamReader reader = new StreamReader("C:/Users/Mathieu/Source/Repos/WPFINTERFACE/Launching Interface/Saves/Settings.txt");
            StreamReader reader = new StreamReader("../../Saves/Settings.txt");
            string line = reader.ReadLine();
            string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.VolMusique = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.VolEffets = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.Langue = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.RenderDistance = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.Fps = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.FullscreenMode = int.Parse(parts[1]);
            line = reader.ReadLine();
            parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
            GererDonnees.KeyboardMode = int.Parse(parts[1]);
            reader.Close();
        }

      void AjouterListeEnvoyer()//AssocierListeEnvoyer()
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
                AssocierListeEnvoyer();
            }
      }

      private void BackButton_Click(object sender, RoutedEventArgs e)
      {
            SaveSettings();
            // this.NavigationService.Navigate(new MainPage());

            AssocierListeEnvoyer();

            GererDonnees.ÉcrireFichier(ListeInfosÀEnvoyer);

            PlaceMouseInTheCenter();
         Application.Current.Shutdown();
      }

        void AssocierListeEnvoyer()
        {
            int cpt = 0;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.Langue; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.Fps; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.RenderDistance; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.VolMusique; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.VolEffets; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.FullscreenMode; ++cpt;
            ListeInfosÀEnvoyer[cpt] = GererDonnees.KeyboardMode;
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

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        void PlaceMouseInTheCenter()
        {
            SetCursorPos((int)(((Panel)Application.Current.MainWindow.Content).ActualWidth / 2), (int)(((Panel)Application.Current.MainWindow.Content).ActualHeight / 2));
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

         if(EstPremiereFois == true)
         {
            EstPremiereFois = false;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
         }
         else
         {
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
            Application.Current.MainWindow.Left = 0;
            Application.Current.MainWindow.Top = 0;
            Application.Current.MainWindow.Width = SystemParameters.VirtualScreenWidth;
            Application.Current.MainWindow.Height = SystemParameters.VirtualScreenHeight;
            Application.Current.MainWindow.Topmost = true;
         }
      }
      private void ButCont_Unchecked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = 1;

            //ChangerRéglages();
            Instructions();
        }
      private void ButCont_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = 0;
         GererDonnees.PremierFichier = false;
            //ChangerRéglages();
            Instructions();
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

         TitreSett.Text = ListeLangueOficielle[35];
         Backtext.Text = ListeLangueOficielle[0];
         Resettext2.Text = ListeLangueOficielle[33];

         RenD.Text = ListeLangueOficielle[18];
         Perfo.Text = ListeLangueOficielle[19];
         Inp.Text = ListeLangueOficielle[21];
         Full.Text = ListeLangueOficielle[20];

         saveText.Text = ListeLangueOficielle[37];
         if (GererDonnees.Langue == 0) { saveText.Margin = new Thickness(29, 60, 118, 48); }
         else { saveText.Margin = new Thickness(40,64,118,48); }

         menuText.Text = ListeLangueOficielle[36];

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

            //if (GererDonnees.KeyboardMode == 1)
            //{
            //   ButCont.Content = ListeLangueOficielle[23];
            //   ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/keyboard.png", UriKind.Relative));
            //   ButCont.IsChecked = false;
            //}
            //else if (GererDonnees.KeyboardMode == 0)
            //{
            //   ButCont.Content = ListeLangueOficielle[22];
            //   ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/Controller2Sides.png", UriKind.Relative));
            //   ButCont.IsChecked = true;
            //}
            textFleches.Text = ListeLangueOficielle[41];
            textWASD.Text = ListeLangueOficielle[40];
            textShift.Text = ListeLangueOficielle[38];
            textE.Text = ListeLangueOficielle[42];
            textP.Text = ListeLangueOficielle[35];
            textSpace.Text = ListeLangueOficielle[39];

            Instructions();
        }

      void GérerLangues()
      {
            //if (GererDonnees.Langue == 0)
            //{
            //   ListeLangueOficielle = GererDonnees.ListeFrancais;
            //   RBfr.IsChecked = true;
            //   RBan.IsChecked = false;
            //   RBes.IsChecked = false;
            //   RBjp.IsChecked = false;
            //}
            //if (GererDonnees.Langue == 1)
            //{
            //   ListeLangueOficielle = GererDonnees.ListeAnglais;
            //   RBfr.IsChecked = false;
            //   RBan.IsChecked = true;
            //   RBes.IsChecked = false;
            //   RBjp.IsChecked = false;
            //}
            //if (GererDonnees.Langue == 2)
            //{
            //   ListeLangueOficielle = GererDonnees.ListeEspagnol;
            //   RBfr.IsChecked = false;
            //   RBan.IsChecked = false;
            //   RBes.IsChecked = true;
            //   RBjp.IsChecked = false;
            //}
            //if (GererDonnees.Langue == 3)
            //{
            //   ListeLangueOficielle = GererDonnees.ListeJaponais;
            //   RBfr.IsChecked = false;
            //   RBan.IsChecked = false;
            //   RBes.IsChecked = false;
            //   RBjp.IsChecked = true;
            //}
            if (GererDonnees.Langue == 0)
            {
                ListeLangueOficielle = GererDonnees.ListeFrancais;
                CocherLangues(true, false, false, false);
            }
            if (GererDonnees.Langue == 1)
            {
                ListeLangueOficielle = GererDonnees.ListeAnglais;
                CocherLangues(false, true, false, false);
            }
            if (GererDonnees.Langue == 2)
            {
                ListeLangueOficielle = GererDonnees.ListeEspagnol;
                CocherLangues(false, false, true, false);
            }
            if (GererDonnees.Langue == 3)
            {
                ListeLangueOficielle = GererDonnees.ListeJaponais;
                CocherLangues(false, false, false, true);
            }
        }

        void CocherLangues(bool fr, bool an, bool es, bool jp)
        {
            RBfr.IsChecked = fr;
            RBan.IsChecked = an;
            RBes.IsChecked = es;
            RBjp.IsChecked = jp;
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

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
                Application.Current.MainWindow.ShowInTaskbar = true;
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
      {
            SaveSettings();
         StreamWriter writer = new StreamWriter("../../Saves/save.txt");
         writer.WriteLine("0");
         writer.WriteLine("false");
            writer.Close();
            KillHyperV();
         this.NavigationService.Navigate(new MainPage());
      }

        void KillHyperV()
        {
            Process[] procs = Process.GetProcessesByName("HyperV");
            Process hypervProc = procs[0];

            hypervProc.Kill();

            //try
            //{
            //    procs = Process.GetProcessesByName("HyperV");

            //    Process hypervProc = procs[0];

            //    if (!hypervProc.HasExited)
            //    {
            //        hypervProc.Kill();
            //    }
            //}
            //finally
            //{
            //    if (procs != null)
            //    {
            //        foreach (Process p in procs)
            //        {
            //            p.Dispose();
            //        }
            //    }
            //}
        }

        void BonScreenshot()
      {
            //ImageFond.Source = new BitmapImage(new Uri(@"Pictures/SavesScreenshots/save0.bmp", UriKind.Relative));
            //ImageFond.Source = new BitmapImage(new Uri(@"../../Saves/" + nomImage, UriKind.Relative));

            //ImageFond = new Image();

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(@"../../Saves/pendingscreenshot.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            ImageFond.Source = src;

            //BitmapImage src = new BitmapImage(new Uri(@"Saves/" + nomImage, UriKind.Relative));
            //src.CacheOption = BitmapCacheOption.OnLoad;
            //ImageFond.Source = src;
        }

        void Instructions()
        {
            if (GererDonnees.KeyboardMode == 1)
            {
                ButCont.Content = ListeLangueOficielle[23];
                ButCont.IsChecked = false;
                ChangerImagesClavier();
                ChangerMargesClavier();

                textL.Text = " ";
                textR.Text = " ";
            }
            else
            {
                ButCont.Content = ListeLangueOficielle[22];
                ButCont.IsChecked = true;
                ChangerImagesManette();
                ChangerMargesManette();

                textL.Text = ListeLangueOficielle[43];
                textR.Text = ListeLangueOficielle[44];
            }


        }

        void ChangerImagesClavier()
        {
            ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/keyboard.png", UriKind.Relative));
            wasd.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/WASD.png", UriKind.Relative));
            e.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/E.png", UriKind.Relative));
            SpaceBar.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/SpaceBar.png", UriKind.Relative));
            Shift.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/Shift.png", UriKind.Relative));
            p.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/P.png", UriKind.Relative));
            FlèchesClavier.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesClavier/FlèchesClavier.png", UriKind.Relative));
        }
        void ChangerMargesClavier()
        {
            textWASD.Margin = new Thickness(0);
            textSpace.Margin = new Thickness(0);
            textFleches.Margin = new Thickness(0);

            wasd.Margin = new Thickness(50, -100, 500, -375);
            e.Margin = new Thickness(206, -280, 250, -212);
            p.Margin = new Thickness(165, -200, 275, -230);
            SpaceBar.Margin = new Thickness(-40, -210, 300, -184);
            Shift.Margin = new Thickness(166, -235, 213, -260);
            FlèchesClavier.Margin = new Thickness(10, -28, 400, -278);

        }
        void ChangerImagesManette()
        {
            ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/Controller2Sides.png", UriKind.Relative));
            wasd.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/Stick_Xbox.png", UriKind.Relative));
            e.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/X_Xbox.png", UriKind.Relative));
            p.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/Start_Xbox.png", UriKind.Relative));
            SpaceBar.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/A_Xbox.png", UriKind.Relative));
            Shift.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/LT_Xbox.png", UriKind.Relative));
            FlèchesClavier.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/TouchesManette/Stick_Xbox.png", UriKind.Relative));
        }
        void ChangerMargesManette()
        {
            textWASD.Margin = new Thickness(-10, 0, 0, 0);
            textSpace.Margin = new Thickness(-22.5, 0, 20, 0);

            textFleches.Margin = new Thickness(-17, 0, 15, 0);

            wasd.Margin = new Thickness(4200, 950, 8600, -1000);
            e.Margin = new Thickness(5800, 180, 9900, 700);
            p.Margin = new Thickness(170, -47, 770, -100);
            SpaceBar.Margin = new Thickness(2260, 740, 3820, -20);
            Shift.Margin = new Thickness(175, 20, 370, -9);
            FlèchesClavier.Margin = new Thickness(4550, 950, 8170, -1000);

            if (GererDonnees.Langue == 3) { textFleches.Margin = new Thickness(-10, 0, 5, 0); }
            if (GererDonnees.Langue == 2) { textFleches.Margin = new Thickness(-20, 0, 5, 0); }

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
      {
            StreamReader r = new StreamReader("../../Saves/save.txt");
            int n = int.Parse(r.ReadLine());
            r.Close();
            File.Copy("../../Saves/pendingsave.txt", "../../Saves/save" + n + ".txt", true);
            File.Copy("../../Saves/pendingscreenshot.png", "../../Saves/screenshot" + n + ".png", true);
        }
   }
}
