using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for SettingsPage.xaml
   /// </summary>
   public partial class SettingsPage : Page
   {
      List<string> ListeLangueOficielle { get; set; }

      public SettingsPage()
      {
         ListeLangueOficielle = new List<string>();

         InitializeComponent();
         GérerFPS();
         GererDonnees.RenderDistenceModifiée = true;
         InitialiserListesLangues();
         GérerRenderDistance();
         GérerSon();
         GérerBoutons();
         ChangerRéglages();

      }

      public void BackButton_Click(object sender, RoutedEventArgs e)
      {
         SaveSettings();
         NavigationService.Navigate(new MainPage());
      }

      private void SaveSettings()
      {
         StreamWriter w = new StreamWriter("../../Saves/Settings.txt");

         w.WriteLine("Music: " + GererDonnees.VolMusique.ToString());
         w.WriteLine("Sound: " + GererDonnees.VolEffets.ToString());
         w.WriteLine("Language: " + ((int)GererDonnees.Langue).ToString());
         w.WriteLine("Render Distance: " + GererDonnees.RenderDistance.ToString());
         w.WriteLine("Frame Rate: " + GererDonnees.Fps.ToString());
         w.WriteLine("Fullscreen: " + ((int)GererDonnees.FullscreenMode).ToString());
         w.WriteLine("Input: " + ((int)GererDonnees.KeyboardMode).ToString());
         w.Close();
      }
  
      private void RDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
      {
         double value = 0;
         if (GererDonnees.RenderDistenceModifiée == true)
         {
            GererDonnees.RenderDistenceModifiée = false;
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
           else  if (value > 3.7 && value < 3.8)
            {
               GererDonnees.RenderDistance = 500;
               RDistanceSlider.Value = 3.75;
            }
           else  if (value > 4.9 && value < 5.1)
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
         GererDonnees.FullscreenMode = GererDonnees.Fullscreen.non;
         GérerBoutons();
      }

      private void ButFull_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.FullscreenMode = GererDonnees.Fullscreen.oui;
         GérerBoutons();
      }

      private void ButCont_Unchecked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = GererDonnees.Controller.Manette;
         GérerBoutons();
      }

      private void ButCont_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.KeyboardMode = GererDonnees.Controller.Clavier;
         GérerBoutons();
      }

      // Langues
      #region 

      private void RBes_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = GererDonnees.Langues.Espagnol;
         ListeLangueOficielle = GererDonnees.ListeEspagnol;
         ChangerRéglages();
         GérerBoutons();
      }

      private void RBjp_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = GererDonnees.Langues.Japonais;
         ListeLangueOficielle = GererDonnees.ListeJaponais;
         ChangerRéglages();
         GérerBoutons();
      }

      private void RBfr_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = GererDonnees.Langues.Francais;
         ListeLangueOficielle = GererDonnees.ListeFrancais;
         ChangerRéglages();
         GérerBoutons();
      }

      private void RBan_Checked(object sender, RoutedEventArgs e)
      {
         GererDonnees.Langue = GererDonnees.Langues.Anglais;
         ListeLangueOficielle = GererDonnees.ListeAnglais;
         ChangerRéglages();
         GérerBoutons();
      }

      void ChangerRéglages()
      {
         Backtext.Text = ListeLangueOficielle[0];
         TitreSett.Text = ListeLangueOficielle[11];
         GMus.Text = ListeLangueOficielle[12];
         SEff.Text = ListeLangueOficielle[13];
         RBfr.Content = ListeLangueOficielle[14];
         RBan.Content = ListeLangueOficielle[15];
         RBes.Content = ListeLangueOficielle[16];
         RBjp.Content = ListeLangueOficielle[17];
         RenD.Text = ListeLangueOficielle[18];
         Perfo.Text = ListeLangueOficielle[19];
         Inp.Text = ListeLangueOficielle[21];
         Full.Text = ListeLangueOficielle[20];
         Lang.Text = ListeLangueOficielle[31];       
         Resettext2.Text = ListeLangueOficielle[33];

         GererCaractéristiques();
      }

      void InitialiserListesLangues()
      {
         RBfr.IsChecked = false;
         RBan.IsChecked = false;
         RBes.IsChecked = false;
         RBjp.IsChecked = false;

         switch (GererDonnees.Langue)
         {
            case GererDonnees.Langues.Francais:
               ListeLangueOficielle = GererDonnees.ListeFrancais;
               RBfr.IsChecked = true;
               break;
            case GererDonnees.Langues.Anglais:
               ListeLangueOficielle = GererDonnees.ListeAnglais;
               RBan.IsChecked = true;
               break;
            case GererDonnees.Langues.Espagnol:
               ListeLangueOficielle = GererDonnees.ListeEspagnol;
               RBes.IsChecked = true;
               break;
            case GererDonnees.Langues.Japonais:
               ListeLangueOficielle = GererDonnees.ListeJaponais;
               RBjp.IsChecked = true;
               break;
         }
      }

      #endregion

      void GérerFPS()
      {
         float valeurPerformance = 0;
         switch(GererDonnees.Fps)
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

      void GameMusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
         var slider = sender as Slider;
         double value = slider.Value;
         GererDonnees.VolMusique = (int)Math.Round(value, 0);
         musicvalue.Text = GererDonnees.VolMusique.ToString();
      }

      void SoundEffectsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
         var slider = sender as Slider;
         double value = slider.Value;
         GererDonnees.VolEffets = (int)Math.Round(value, 0);
         soundvalue.Text = GererDonnees.VolEffets.ToString();

      }

      void ResetButton_Click(object sender, RoutedEventArgs e)
      {
         GererDonnees.RenderDistenceModifiée = true;
         GererDonnees.RéglagesBase();
         ChangerRéglages();
         GérerFPS();
         GérerRenderDistance();
         InitialiserListesLangues();
         GérerSon();
         GérerBoutons();
      } 

      void GérerSon()
      {
         GameMusicSlider.Value = GererDonnees.VolMusique;
         SoundEffectsSlider.Value = GererDonnees.VolEffets;
      }

      void GérerBoutons()
      {
         if (GererDonnees.FullscreenMode == GererDonnees.Fullscreen.oui)
         {
            ButFull.Content = ListeLangueOficielle[29];
            ButFull.IsChecked = true;
            AppliquerFondÉcran();
         }
         else
         {
            ButFull.Content = ListeLangueOficielle[30];
            ButFull.IsChecked = false;
            RetirerDondÉcran();

         }

         if (GererDonnees.KeyboardMode == GererDonnees.Controller.Manette)
         {
            ButCont.Content = ListeLangueOficielle[23];
            ButCont.IsChecked = false;
         }
         else
         {

            ButCont.Content = ListeLangueOficielle[22];
            ButCont.IsChecked = true;
         }

      }

      void GererCaractéristiques()
      {
         switch (GererDonnees.Langue)
         {
            case GererDonnees.Langues.Francais:
               Resettext2.Margin = new Thickness(33, 64, 126, 48);
               Backtext.Margin = new Thickness(28, 19, 113, 88);
               TitreSett.Margin = new Thickness(-25, 11, 40, 11);
               break;
            case GererDonnees.Langues.Anglais:
               Resettext2.Margin = new Thickness(43, 64, 126, 48);
               Backtext.Margin = new Thickness(28, 19, 113, 88);
               TitreSett.Margin = new Thickness(-24, 11, 38, 11);
               break;
            case GererDonnees.Langues.Espagnol:
               Resettext2.Margin = new Thickness(37, 64, 122, 48);
               Backtext.Margin = new Thickness(22, 19, 114, 88);
               TitreSett.Margin = new Thickness(-21, 11, 35, 11);
               break;
            case GererDonnees.Langues.Japonais:
               Resettext2.Margin = new Thickness(39, 64, 123, 48);
               Backtext.Margin = new Thickness(28, 19, 113, 88);
               TitreSett.Margin = new Thickness(-14, 11, 44, 11);
               break;
         }
      }


      void AppliquerFondÉcran()
      {
         Application.Current.MainWindow.WindowStyle = WindowStyle.None;
         Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
         Application.Current.MainWindow.Left = 0;
         Application.Current.MainWindow.Top = 0;
         Application.Current.MainWindow.Width = SystemParameters.VirtualScreenWidth;
         Application.Current.MainWindow.Height = SystemParameters.VirtualScreenHeight;
         Application.Current.MainWindow.Topmost = true;
      }

      void RetirerDondÉcran()
      {
         int largeurÉcran = 1500;
         int hauteurÉcran = 800;

         Application.Current.MainWindow.Height = hauteurÉcran;
         Application.Current.MainWindow.Width = largeurÉcran;

         Application.Current.MainWindow.WindowState = WindowState.Normal;
         Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
         Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;

      }


      //Fonctions nécessaires mais non-utilisés
      #region   

      private void rdvalue_TextChanged(object sender, TextChangedEventArgs e)
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
      private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {

      }

      private void SoundVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {

      }

      private void RenderDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {

      }


      #endregion
   }
}

