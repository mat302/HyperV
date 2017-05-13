using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.IO;

namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for NewGamePage.xaml
   /// </summary>
   public partial class NewGamePage : Page
   {
      const int NBRE_FICHIERS_SAVES = 3;

      List<string> LangueOficielleNewPage { get; set; }
      List<string> ListeÉlémentsAAfficher { get; set; }

      public NewGamePage()
      {
         LangueOficielleNewPage = new List<string>();
         ListeÉlémentsAAfficher  = new List<string>();

         InitializeComponent();
         OrganiserLangueEtMarges();
         PlaceContent();
         OrganiserMargesCaractéristiques();
      }

      void OrganiserLangueEtMarges()
      {
         switch (GererDonnees.Langue)
         {
            case GererDonnees.Langues.Francais:
               LangueOficielleNewPage = GererDonnees.ListeFrancais;
               tbtitre.Margin = new Thickness(-40, 13, 42, 5);
               BackButton.Margin = new Thickness(28, 17, 113, 52);
               break;
            case GererDonnees.Langues.Anglais:
               LangueOficielleNewPage = GererDonnees.ListeAnglais;
               tbtitre.Margin = new Thickness(-30, 13, 49, 5);
               BackButton.Margin = new Thickness(28, 17, 113, 52);
               break;
            case GererDonnees.Langues.Espagnol:
               LangueOficielleNewPage = GererDonnees.ListeEspagnol;
               tbtitre.Margin = new Thickness(-40, 13, 42, 5);
               BackButton.Margin = new Thickness(24, 17, 118, 52);
               break;
            case GererDonnees.Langues.Japonais:
               LangueOficielleNewPage = GererDonnees.ListeJaponais;
               tbtitre.Margin = new Thickness(-30, 13, 49, 5);
               BackButton.Margin = new Thickness(28, 17, 113, 52);
               break;

         }
         tbtitre.Text = LangueOficielleNewPage[1];
         BackButton.Text = LangueOficielleNewPage[0];
      }

      private void PlaceContent()
      {
         for (int i = 0; i < NBRE_FICHIERS_SAVES; ++i)
         {
            PlaceButtonsContent(i);
         }
         
      }

      private void PlaceButtonsContent(int i)
      {
         if (GererDonnees.JeuEstExistant[i])
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
               slotA.Text = LangueOficielleNewPage[2];
               Level0.Text= LangueOficielleNewPage[4] + " " + GererDonnees.NbreNiveauxComplétés(i) + "/" + GererDonnees.NombreNiveaux(i).ToString();
               Time0.Text = LangueOficielleNewPage[3] + " " + ListeÉlémentsAAfficher[3];     
               break;
            case 1:
               image1.Source = src;
               image1.Margin = new Thickness(30);
               slotB.Text = LangueOficielleNewPage[5];
               Level1.Text= LangueOficielleNewPage[4] + " " + GererDonnees.NbreNiveauxComplétés(i) + "/" + GererDonnees.NombreNiveaux(i).ToString();
               Time1.Text = LangueOficielleNewPage[3] + " " + ListeÉlémentsAAfficher[3];
               break;
            case 2:
               image2.Source = src;
               image2.Margin = new Thickness(30);
               slotC.Text = LangueOficielleNewPage[8];
               Level2.Text= LangueOficielleNewPage[4] + " " + GererDonnees.NbreNiveauxComplétés(i) + "/" + GererDonnees.NombreNiveaux(i).ToString();
               Time2.Text = LangueOficielleNewPage[3] + " " + ListeÉlémentsAAfficher[3];
               break;
         }

         OrganiserMargesCaractéristiques();
         ChangeBorderBrushColor(i);
      }

      void OrganiserMargesCaractéristiques()
        {
            switch (GererDonnees.Langue)
            {
                case GererDonnees.Langues.Francais:
                    LangueOficielleNewPage = GererDonnees.ListeFrancais;
                    tbtitre.Margin = new Thickness(-38, 13, 43, 5);
                    BackButton.Margin = new Thickness(36, 17, 105, 50);
                    slotA.Margin = slotB.Margin = slotC.Margin = new Thickness(15, -11, 15, 10);
                    Level0.Margin = Level1.Margin = Level2.Margin = new Thickness(5, -5, 5, 5);
                    Time0.Margin = Time1.Margin = Time2.Margin = new Thickness(20, 0, 20, 0);

                    break;
                case GererDonnees.Langues.Anglais:
                    LangueOficielleNewPage = GererDonnees.ListeAnglais;
                    tbtitre.Margin = new Thickness(-35, 13, 49, 5);
                    BackButton.Margin = new Thickness(36, 17, 105, 50);
                    slotA.Margin = slotB.Margin = slotC.Margin = new Thickness(33, -11, 33, 10);
                    Level0.Margin = Level1.Margin = Level2.Margin = new Thickness(5, -5, 5, 5);
                    Time0.Margin = Time1.Margin = Time2.Margin = new Thickness(20, 0, 20, 0);
                    break;
                case GererDonnees.Langues.Espagnol:
                     LangueOficielleNewPage = GererDonnees.ListeEspagnol;
                    tbtitre.Margin = new Thickness(-39, 13, 42, 5);
                    BackButton.Margin = new Thickness(33, 17, 107, 52);
                    slotA.Margin = slotB.Margin = slotC.Margin = new Thickness(27, -11, 27, 10);
                    Level0.Margin = Level1.Margin = Level2.Margin = new Thickness(5, -5, 5, 5);
                    Time0.Margin = Time1.Margin = Time2.Margin = new Thickness(20, 0, 20, 0);
                    break;
                case GererDonnees.Langues.Japonais:
                    LangueOficielleNewPage = GererDonnees.ListeJaponais;
                    tbtitre.Margin = new Thickness(-41, 13, 53, 5);
                    BackButton.Margin = new Thickness(36, 17, 105, 52);
                    slotA.Margin = slotB.Margin = slotC.Margin = new Thickness(26, -11, 26, 10);
                    Level0.Margin = Level1.Margin = Level2.Margin = new Thickness(14, -5, 14, 5);
                    Time0.Margin = Time1.Margin = Time2.Margin = new Thickness(20, 0, 20, 0);
                    break;

            }
        }

      void ChangeBorderBrushColor(int i)
      {
         switch (i)
         {
            case 0:
                    Save0Button.BorderBrush = Brushes.Black;
               break;
            case 1:
                    Save1Button.BorderBrush = Brushes.Black;
               break;
            case 2:
                    Save2Button.BorderBrush = Brushes.Black;
               break;
         }
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
               CreateImage(Save0);
               break;
            case 1:
               CreateImage(Save1);
               break;
            case 2:
               CreateImage(Save2);
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
            case GererDonnees.Langues.Francais:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateFR.png", UriKind.Relative));
               break;
            case GererDonnees.Langues.Anglais:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/Create.png", UriKind.Relative));
               break;
            case GererDonnees.Langues.Espagnol:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateES.png", UriKind.Relative));
               break;
            case GererDonnees.Langues.Japonais:
               e.Image.Source = new BitmapImage(new Uri(@"/Pictures/CreateJA.png", UriKind.Relative));
               break;
         }
         e.Image.Margin = new Thickness(0, -90, 0, -350);
         l.Children.Add(e);
      }

      void BackButton_Click(object sender, RoutedEventArgs e)
      {
         NavigationService.Navigate(new MainPage());
      }

      void Save0Button_Click(object sender, RoutedEventArgs e)
      {
         if (!GererDonnees.JeuEstExistant[0])
         {
            CreateSave("0");
         }
         LoadSave("0");
      }

      void LoadSave(string saveNumber)
      {
         ManagePause(saveNumber);
         string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"../../../../bin/x86/Debug/HyperV.exe");
         ProcessStartInfo p = new ProcessStartInfo();
         p.FileName = path;
         p.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
         Process.Start(p);
         Application.Current.Shutdown();
      }

      void CreateSave(string saveNumber)
      {
         StreamWriter writer = new StreamWriter("../../Saves/save" + saveNumber + ".txt");

         writer.WriteLine("Level: 0");
         writer.WriteLine("Position: {X:5 Y:5 Z:5}");
         writer.WriteLine("Direction: {X:5 Y:5 Z:5}");
         writer.WriteLine("Time Played: " + (new TimeSpan(0, 0, 0)).ToString());
            writer.WriteLine("Max Life: 300");
            writer.WriteLine("Attack: 0");
            writer.WriteLine("false;false;false;false;false;false;false;false");
            writer.Close();
         File.Copy("../../Saves/startscreenshot.png", "../../Saves/screenshot" + saveNumber + ".png", true);
      }

      void ManagePause(string saveNumber)
      {
         StreamWriter writer = new StreamWriter("../../Saves/save.txt");
         writer.WriteLine(saveNumber);
         writer.WriteLine("true");
         writer.Close();
      }

      void Save1Button_Click(object sender, RoutedEventArgs e)
      {
         if (!GererDonnees.JeuEstExistant[1])
         {
            CreateSave("1");
         }
         LoadSave("1");
      }

      void Save2Button_Click(object sender, RoutedEventArgs e)
      {
         if (!GererDonnees.JeuEstExistant[2])
         {
            CreateSave("2");
         }
         LoadSave("2");
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

   }
}
