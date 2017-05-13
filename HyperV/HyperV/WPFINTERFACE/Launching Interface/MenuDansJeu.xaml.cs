using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Launching_Interface
{
   /// <summary>
   /// Logique d'interaction pour MenuDansJeu.xaml
   /// </summary>
   public partial class MenuDansJeu : Page
   {
      bool EstPremiereFoisFondÉcran { get; set; }
      List<string> ListeLangueOficielle { get; set; }

      public MenuDansJeu()
      {
         RefreshData();
         EstPremiereFoisFondÉcran = true;
         ListeLangueOficielle = new List<string>();

         InitializeComponent();

         BonScreenshot();
         GérerFPS();
         GererDonnees.RD = true;
         GérerLangues();
         GérerRenderDistance();
         GérerSon();
       //  GérerBoutons();
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


      private void BackButton_Click(object sender, RoutedEventArgs e)
      {
         SaveSettings();
         // this.NavigationService.Navigate(new MainPage());

         PlaceMouseInTheCenter();
         Application.Current.Shutdown();
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

      private void RDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
         double value = 0;
         if (GererDonnees.RD == true)
         {
            GererDonnees.RD = false;
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
            else if (value > 1.2 && value < 1.3)
            {
               GererDonnees.RenderDistance = 50;
               RDistanceSlider.Value = 1.25;
            }
            else if (value > 2.4 && value < 2.6)
            {
               GererDonnees.RenderDistance = 100;
               RDistanceSlider.Value = 2.5;
            }
            else if (value > 3.7 && value < 3.8)
            {
               GererDonnees.RenderDistance = 500;
               RDistanceSlider.Value = 3.75;
            }
            else if (value > 4.9 && value < 5.1)
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
            else if (value > 7.4 && value < 7.6)
            {
               GererDonnees.RenderDistance = 10000;
               RDistanceSlider.Value = 7.5;
            }
            else if (value > 8.7 && value < 8.8)
            {
               GererDonnees.RenderDistance = 50000;
               RDistanceSlider.Value = 8.75;
            }
            else if (value > 9.9 && value <= 10)
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
       //  GérerBoutons();
         Instructions();

      }

      private void ButFull_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.FullscreenMode = 1;
      //   GérerBoutons();
         Instructions();
      }

      private void ButCont_Unchecked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = 1;
        // GérerBoutons();
         Instructions();
      }

      private void ButCont_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = 0;
      //   GérerBoutons();
         Instructions();
      }

      // Langues
      #region 

      private void RBes_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = 2;
         ListeLangueOficielle = GererDonnees.ListeEspagnol;

         ChangerRéglages();
      }
      private void RBjp_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = 3;
         ListeLangueOficielle = GererDonnees.ListeJaponais;

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

         ChangerRéglages();
      }

      void ChangerRéglages()
      {
         Backtext.Text = ListeLangueOficielle[0];
         GMus.Text = ListeLangueOficielle[12];
         SEff.Text = ListeLangueOficielle[13];
         RBfr.Content = ListeLangueOficielle[14];
         RBan.Content = ListeLangueOficielle[15];
         RBes.Content = ListeLangueOficielle[16];
         RBjp.Content = ListeLangueOficielle[17];
         RenD.Text = ListeLangueOficielle[18];
         Perfo.Text = ListeLangueOficielle[19];
         Full.Text = ListeLangueOficielle[20];
         Inp.Text = ListeLangueOficielle[21];

         Lang.Text = ListeLangueOficielle[31];
         Resettext2.Text = ListeLangueOficielle[33];
         TitreSett.Text = ListeLangueOficielle[35];
         menuText.Text = ListeLangueOficielle[36];
         saveText.Text = ListeLangueOficielle[37];


         switch (GererDonnees.Langue)
         {
            case 0:
               Backtext.Margin = new Thickness(38, 19, 110, 88);
               Resettext2.Margin = new Thickness(113, 19, 28, 88);
               saveText.Margin = new Thickness(29, 60, 118, 48);
               break;
            case 1:
               Backtext.Margin = new Thickness(38, 19, 113, 88);
               Resettext2.Margin = new Thickness(107, 19, 33, 88);
               saveText.Margin = new Thickness(40, 64, 118, 48);
               break;
            case 2:
               Backtext.Margin = new Thickness(34, 19, 110, 88);
               Resettext2.Margin = new Thickness(111, 19, 30, 88);
               saveText.Margin = new Thickness(40, 64, 118, 48);
               break;
            case 3:
               Backtext.Margin = new Thickness(38, 19, 113, 88);
               Resettext2.Margin = new Thickness(107, 19, 33, 88);
               saveText.Margin = new Thickness(40, 64, 118, 48);

               break;
         }

         textP.Text = ListeLangueOficielle[35];
         textShift.Text = ListeLangueOficielle[38];
         textSpace.Text = ListeLangueOficielle[39];
         textWASD.Text = ListeLangueOficielle[40];
         textFleches.Text = ListeLangueOficielle[41];
         textE.Text = ListeLangueOficielle[42];

         Instructions();
      }

      void GérerLangues()
      {
         RBfr.IsChecked = false;
         RBan.IsChecked = false;
         RBes.IsChecked = false;
         RBjp.IsChecked = false;

         switch (GererDonnees.Langue)
         {
            case 0:
               ListeLangueOficielle = GererDonnees.ListeFrancais;
               RBfr.IsChecked = true;
               break;
            case 1:
               ListeLangueOficielle = GererDonnees.ListeAnglais;
               RBan.IsChecked = true;
               break;
            case 2:
               ListeLangueOficielle = GererDonnees.ListeEspagnol;
               RBes.IsChecked = true;
               break;
            case 3:
               ListeLangueOficielle = GererDonnees.ListeJaponais;
               RBjp.IsChecked = true;
               break;
         }
      }

      #endregion

      void GérerFPS()
      {
         float valeurPerformance = 0;
         switch (GererDonnees.Fps)
         {
            case 30:
               valeurPerformance = 0;
               break;
            case 60:
               valeurPerformance = 3.3333f;
               break;
            case 90:
               valeurPerformance = 6.66666f;
               break;
            case 120:
               valeurPerformance = 10f;
               break;
         }
         PerformanceSlider.Value = valeurPerformance;

         if (PerformanceSlider.Value < 0.2) { valeurPerfo.Text = "30 FPS"; }
      }

      void GérerRenderDistance()
      {
         RDistanceSlider.Value = GererDonnees.RenderDistance;
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
         GererDonnees.PremierFichier = true; //nothing for commit
         GererDonnees.RD = true;
         GererDonnees.RéglagesBase();
         ChangerRéglages();
         GérerFPS();
         GérerRenderDistance();
         GérerLangues();
         GérerSon();
         GérerBoutons();
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
         BitmapImage src = new BitmapImage();
         src.BeginInit();
         src.UriSource = new Uri(@"../../Saves/pendingscreenshot.png", UriKind.Relative);
         src.CacheOption = BitmapCacheOption.OnLoad;
         src.EndInit();
         ImageFond.Source = src;
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
         GérerBoutons();
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
         textL.Margin = new Thickness(0);
         textR.Margin = new Thickness(0);

         wasd.Margin = new Thickness(50, -140, 500, -360);
         e.Margin = new Thickness(206, -292, 252, -233);
         p.Margin = new Thickness(165, -130, 275, -230);
         SpaceBar.Margin = new Thickness(-110, -130, 176,-361);
         Shift.Margin = new Thickness(167, -230, 213, -280);
         FlèchesClavier.Margin = new Thickness(22, -51, 420, -271);

         switch (GererDonnees.Langue)
         {
            case 0:
               textWASD.Margin = new Thickness(110, 8, -32, -5);
               textP.Margin = new Thickness(80, 0, 44, 4);
               textShift.Margin = new Thickness(80, 10, 44, -3);
               textFleches.Margin = new Thickness(68, 7, -20, -5);
               textSpace.Margin = new Thickness(72, 5, -11, -2);
               textE.Margin = new Thickness(54, 0, 5.5, 4);
               break;
            case 1:
               textWASD.Margin = new Thickness(90, 8, -18, -5);
               textP.Margin = new Thickness(80, 0, 44, 4);
               textShift.Margin = new Thickness(72, 10, 46, -3);
               textFleches.Margin = new Thickness(77, 6.5, -24, -5);
               textSpace.Margin = new Thickness(69, 5, -7, -2);
               textE.Margin = new Thickness(48, 0, 10, 4);
               break;
            case 2:
               textWASD.Margin = new Thickness(90, 8, -18, -5);
               textP.Margin = new Thickness(80, 0, 44, 4);
               textShift.Margin = new Thickness(80, 10, 44, -3);
               textFleches.Margin = new Thickness(68, 7, -20, -5);
               textSpace.Margin = new Thickness(72, 5, -7, -2);
               textE.Margin = new Thickness(54, 0, 15, 4);
               break;
            case 3:
               textWASD.Margin = new Thickness(90, 8, -18, -5);
               textP.Margin = new Thickness(80, 2, 44, 4);
               textShift.Margin = new Thickness(72, 10, 46, -3);
               textFleches.Margin = new Thickness(55, 7, -12, -5);
               textSpace.Margin = new Thickness(77, 5, -11, -2);
               textE.Margin = new Thickness(38, 0, 21, 4);
               break;
         }

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
         wasd.Margin = new Thickness(1900, 500, 3900, -400);
         e.Margin = new Thickness(2860, 100, 5120, 420);
         p.Margin = new Thickness(130, 72, 580,-74);
         SpaceBar.Margin = new Thickness(2255, 1720, 4050, -1300);
         Shift.Margin = new Thickness(175, 35, 370, -37);
         FlèchesClavier.Margin = new Thickness(2100, 500, 3900, -380);

         switch (GererDonnees.Langue)
         {
            case 0:
               textFleches.Margin = new Thickness(59, 8, 0, -5);
               textSpace.Margin = new Thickness(53, 9, 29, -2);
               textWASD.Margin = new Thickness(67, 8, -33, -5);
               textL.Margin = new Thickness(40, 19, 76, -4);
               textP.Margin = new Thickness(67, -2, 50, 5);
               textShift.Margin = new Thickness(67, 8, 45, -3);
               textE.Margin = new Thickness(52, -1, 12, 5);
               textR.Margin = new Thickness(44, 15, 74, -4);
               break;
            case 1:
               textFleches.Margin = new Thickness(63, 8, -3, -5);
               textSpace.Margin = new Thickness(50, 9, 29, -2);
               textWASD.Margin = new Thickness(70, 8, 0, -5);
               textL.Margin = new Thickness(37, 19, 71, -4);
               textP.Margin = new Thickness(67, -2, 50, 5);
               textShift.Margin = new Thickness(61, 8, 54, -3);
               textE.Margin = new Thickness(50, -1, 20, 6);
               textR.Margin = new Thickness(52, -1, 12, 6);
               break;
            case 2:
               textFleches.Margin = new Thickness(78, 8, -15, -5);
               textSpace.Margin = new Thickness(50, 9, 29, -2);
               textWASD.Margin = new Thickness(75, 8, 0, -5);
               textL.Margin = new Thickness(38, 19, 71, -4);
               textP.Margin = new Thickness(67, -2, 50, 5);
               textShift.Margin = new Thickness(67, 8, 45, -3);
               textE.Margin = new Thickness(51.5, -1, 18, 6);
               textR.Margin = new Thickness(52, -1, 12, 6);
               break;
            case 3:
               textFleches.Margin = new Thickness(46, 8, 6, -5);
               textSpace.Margin = new Thickness(53, 9, 29, -2);
               textWASD.Margin = new Thickness(70, 8, 0, -5);
               textL.Margin = new Thickness(51, 22, 98, -5);
               textP.Margin = new Thickness(67, 0, 56, 6);
               textShift.Margin = new Thickness(58, 8, 57, -3);
               textE.Margin = new Thickness(38, -1, 27, 6);
               textR.Margin = new Thickness(52, -1, 12, 6);
               break;
         }

      }

      void saveButton_Click(object sender, RoutedEventArgs e)
      {
         StreamReader r = new StreamReader("../../Saves/save.txt");
         int n = int.Parse(r.ReadLine());
         r.Close();
         File.Copy("../../Saves/pendingsave.txt", "../../Saves/save" + n + ".txt", true);
         File.Copy("../../Saves/pendingscreenshot.png", "../../Saves/screenshot" + n + ".png", true);
            GererDonnees.RefreshSaves();
      }

      void GérerBoutons()
      {
         if (GererDonnees.FullscreenMode == 1)
         {
            ButFull.IsChecked = true;
            AppliquerFondÉcran();
            ButFull.Content = ListeLangueOficielle[29];
         }
         else
         {
            if (EstPremiereFoisFondÉcran == true) { EstPremiereFoisFondÉcran = false; }
            RetirerDondÉcran();
            ButFull.Content = ListeLangueOficielle[30];
         }

         if (GererDonnees.KeyboardMode == 1)
         {
            ButCont.Content = ListeLangueOficielle[23];
            ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/keyboard.png", UriKind.Relative));
         }
         else
         {
            ButCont.Content = ListeLangueOficielle[22];
            ImageInstructions.Source = new BitmapImage(new Uri(@"/Pictures/Instructions/Controller2Sides.png", UriKind.Relative));
         }
      }

      void AppliquerFondÉcran()
      {
         if (EstPremiereFoisFondÉcran == true)
         {
            EstPremiereFoisFondÉcran = false;
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

      void RetirerDondÉcran()
      {
         Application.Current.MainWindow.Height = 750;
         Application.Current.MainWindow.Width = 1400;

         Application.Current.MainWindow.WindowState = WindowState.Normal;
         Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
         Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
      }

      // Non-utilisés
      #region

      private void rdvalue_TextChanged(object sender, TextChangedEventArgs e)
      {

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
   }
}
