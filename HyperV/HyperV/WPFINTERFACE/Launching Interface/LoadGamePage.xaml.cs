using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for LoadGamePage.xaml
   /// </summary>
   public partial class LoadGamePage : Page
    {
      List<string> LangueOficielleLoadPage { get; set; }
      List<string> ListeÉlémentsAAfficher { get; set; }


      public LoadGamePage()
        {
            Refresh();
      }

        void Refresh()
        {
            LangueOficielleLoadPage = new List<string>();
            ListeÉlémentsAAfficher = new List<string>();

            InitializeComponent();
            switch (GererDonnees.Langue)
            {
                case 0:
                    LangueOficielleLoadPage = GererDonnees.ListeFrancais;
                    tbtitre.Margin = new Thickness(-40, 13, 42, 5);
                    BackButton.Margin = new Thickness(28, 17, 113, 52);
                    break;
                case 1:
                    LangueOficielleLoadPage = GererDonnees.ListeAnglais;
                    tbtitre.Margin = new Thickness(-30, 13, 49, 5);
                    BackButton.Margin = new Thickness(28, 17, 113, 52);
                    break;
                case 2:
                    LangueOficielleLoadPage = GererDonnees.ListeEspagnol;
                    tbtitre.Margin = new Thickness(-40, 13, 42, 5);
                    BackButton.Margin = new Thickness(24, 17, 118, 52);
                    break;
                case 3:
                    LangueOficielleLoadPage = GererDonnees.ListeJaponais;
                    tbtitre.Margin = new Thickness(-30, 13, 49, 5);
                    BackButton.Margin = new Thickness(28, 17, 113, 52);
                    break;

            }
            tbtitre.Text = LangueOficielleLoadPage[32];
            BackButton.Text = LangueOficielleLoadPage[0];

            //CheckForExistingGames();
            PlaceContent();
        }

      private void PlaceContent()
      {
         for (int i = 0; i < 3; ++i)
         {
            if (GererDonnees.GameExists[i])
            {
               PlaceRows(i);
            }
            else
            {
               PlaceCreateImage(i);
            }
         }

      }

      private void PlaceRows(int i)
      {
         switch (i)
         {
            case 0:
               CreateRows(i);
               break;
            case 1:
               CreateRows(i);
               break;
            case 2:
               CreateRows(i);
               break;
         }
      }

      private void CreateRows(int i)
      {
         BitmapImage src = new BitmapImage();
         src.BeginInit();
         src.UriSource = new Uri(@"../../Saves/screenshot" + i + ".png", UriKind.Relative);
         src.CacheOption = BitmapCacheOption.OnLoad;
         src.EndInit();

         LireInformationsNouvellePartie(i);

         switch (i)
         {
            case 0:
               image0.Source = src;
               image0.Margin = new Thickness(30);
               slotA.Text = ÉlémentFichiersLangues(2);
               Level0.Text = ÉlémentFichiersLangues(4) + " " + GererDonnees.CountComplete(i) + "/" + GererDonnees.CountLevels(i).ToString();
               Time0.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               break;
            case 1:
               image1.Source = src;
               image1.Margin = new Thickness(30);
               slotB.Text = ÉlémentFichiersLangues(5);
               Level1.Text = ÉlémentFichiersLangues(4) + " " + GererDonnees.CountComplete(i) + "/" + GererDonnees.CountLevels(i).ToString();
               Time1.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               break;
            case 2:
               image2.Source = src;
               image2.Margin = new Thickness(30);
               slotC.Text = ÉlémentFichiersLangues(8);
               Level2.Text = ÉlémentFichiersLangues(4) + " " + GererDonnees.CountComplete(i) + "/" + GererDonnees.CountLevels(i).ToString();
               Time2.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               break;
               //case 0:
               //   image0.Source = src;
               //   image0.Margin = new Thickness(30);
               //   slotA.Text = ÉlémentFichiersLangues(7) + " A";
               //   Level0.Text = ÉlémentFichiersLangues(4) + " " + ListeÉlémentsAAfficher[0] + "/" + GererDonnees.NBRE_NIVEAUX.ToString();
               //   Time0.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               //   break;
               //case 1:
               //   image1.Source = src;
               //   image1.Margin = new Thickness(30);
               //   slotB.Text = ÉlémentFichiersLangues(7) + " B";
               //   Level1.Text = ÉlémentFichiersLangues(4) + " " + ListeÉlémentsAAfficher[0] + "/" + GererDonnees.NBRE_NIVEAUX.ToString();
               //   Time1.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               //   break;
               //case 2:
               //   image2.Source = src;
               //   image2.Margin = new Thickness(30);
               //   slotC.Text = ÉlémentFichiersLangues(7) + " C";
               //   Level2.Text = ÉlémentFichiersLangues(4) + " " + ListeÉlémentsAAfficher[0] + "/" + GererDonnees.NBRE_NIVEAUX.ToString();
               //   Time2.Text = ÉlémentFichiersLangues(3) + " " + ListeÉlémentsAAfficher[3];
               //   break;
         }
         //ListeÉlémentsAAfficher.Clear();

         OrganiserMargesDesCaractéristiques();
         RendreTexteRouge(i);
      }

      void RendreTexteRouge(int i)
      {
         switch (i)
         {
            case 0:
               slotA.Foreground = Brushes.Red;
               Load0Button.BorderBrush = Brushes.Red;
               break;
            case 1:
               slotB.Foreground = Brushes.Red;
               Load1Button.BorderBrush = Brushes.Red;
               break;
            case 2:
               slotC.Foreground = Brushes.Red;
               Load2Button.BorderBrush = Brushes.Red;
               break;
         }
      }

        void ChangeBorderBrushColor(int i)
        {
            switch (i)
            {
                case 0:
                    Load0Button.BorderBrush = Brushes.Black;//DarkBlue;
                    break;
                case 1:
                    Load1Button.BorderBrush = Brushes.Black;//DarkBlue;
                    break;
                case 2:
                    Load2Button.BorderBrush = Brushes.Black;//DarkBlue;
                    break;
            }
        }

        string ÉlémentFichiersLangues(int i)
      {
         return LangueOficielleLoadPage[i].Replace("\n", string.Empty);
      }

      void OrganiserMargesDesCaractéristiques()
      {
         Thickness margesSave = new Thickness(10,0,10,0);
         Thickness margesLevel = new Thickness(0);

         if (GererDonnees.Langue != 0)
         {
            margesSave = new Thickness(20, 0, 20, 0);
         }
         slotA.Margin = margesSave;
         slotB.Margin = margesSave;
         slotC.Margin = margesSave;


         if (GererDonnees.Langue == 3)
         {
            margesLevel = new Thickness(10, 0, 10, 0);
         }
         Level0.Margin = margesLevel;
         Level1.Margin = margesLevel;
         Level2.Margin = margesLevel;


      }

      void LireInformationsNouvellePartie(int i)
      {
         switch (i)
         {
            case 0:
               ListeÉlémentsAAfficher = GererDonnees.ListeCaractéristiquesAAfficher0;
               break;
            case 1:
               ListeÉlémentsAAfficher = GererDonnees.ListeCaractéristiquesAAfficher1;
               break;
            case 2:
               ListeÉlémentsAAfficher = GererDonnees.ListeCaractéristiquesAAfficher2;
               break;
         }
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
            ChangeBorderBrushColor(i);
         RéinitialiserBoutons(i);
      }

      private void CreateImage(Grid l)
      {
         Create e = new Create();
         switch (GererDonnees.Langue)
         {
            case 0:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/EmptyFR.png", UriKind.Relative));
               break;
            case 1:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/Empty.png", UriKind.Relative));
               break;
            case 2:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/EmptyES.png", UriKind.Relative));
               break;
            case 3:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/EmptyJA.png", UriKind.Relative));
               break;
         }
         e.Image.Margin = new Thickness(0, -90, 0, -350);
         l.Children.Add(e);
      }

      void RéinitialiserBoutons(int i)
      {
         switch (i)
         {
            case 0:
               slotA.Text = "";
               Time0.Text = "";
               Level0.Text = "";
               break;
            case 1:
               slotB.Text = "";
               Time1.Text = "";
               Level1.Text = "";
               break;
            case 2:
               slotC.Text = "";
               Time2.Text = "";
               Level2.Text = "";
               break;
         }
      }

      //----------------------------------------------------------------------------------------------------------

      private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }

        void DeleteSave(string saveNumber)
        {
            StreamWriter writer = new StreamWriter("../../Saves/save" + saveNumber + ".txt");
            writer.WriteLine();
            writer.Close();
            GererDonnees.RefreshSaves();
            image0.Source = null;
            image1.Source = null;
            image2.Source = null;
            Refresh();
        }

        private void Load1Button_Click(object sender, RoutedEventArgs e)
      {
            DeleteSave("0");
      }

      private void Load2Button_Click(object sender, RoutedEventArgs e)
      {
            DeleteSave("1");
        }

      private void Load3Button_Click(object sender, RoutedEventArgs e)
      {
            DeleteSave("2");
        }
   }
}
