using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for MainPage.xaml
   /// </summary>
   public partial class MainPage : Page
   {
      List<string> LangueOficielleMain { get; set; }
      public MainPage()
      {
         LangueOficielleMain = new List<string>();
         InitializeComponent();

         GérerTextes();
         GérerPleinÉcran();
      }

      void GérerTextes()
      {
         switch (GererDonnees.Langue)
         {
            case GererDonnees.Langues.Francais:
               LangueOficielleMain = GererDonnees.ListeFrancais;
               lg.Margin = new Thickness(55, 10, 50, 10);
               ng.Margin = new Thickness(55, 10, 55, 10);
               se.Margin = new Thickness(55, 10, 55, 10);
               break;
            case GererDonnees.Langues.Anglais:
               LangueOficielleMain = GererDonnees.ListeAnglais;
               lg.Margin = new Thickness(57, 10, 46, 10);
               ng.Margin = new Thickness(57, 10, 51, 10);
               se.Margin = new Thickness(55, 10, 55, 10);
               break;
            case GererDonnees.Langues.Espagnol:
               LangueOficielleMain = GererDonnees.ListeEspagnol;
               lg.Margin = new Thickness(55, 10, 50, 10);
               ng.Margin = new Thickness(55, 10, 55, 10);
               se.Margin = new Thickness(55, 10, 55, 10);

               break;
            case GererDonnees.Langues.Japonais:
               LangueOficielleMain = GererDonnees.ListeJaponais;
               lg.Margin = new Thickness(55, 10, 50, 10);
               ng.Margin = new Thickness(57, 10, 51, 10);
               se.Margin = new Thickness(57, 10, 53, 10);
               break;


         }


         ng.Text = LangueOficielleMain[1];
         lg.Text = LangueOficielleMain[32];
         se.Text = LangueOficielleMain[11];
         cr.Text = LangueOficielleMain[24];
         exit.Text = LangueOficielleMain[34];
      }

      void GérerPleinÉcran()
      {
         if (GererDonnees.FullscreenMode == GererDonnees.Fullscreen.oui)
         {
            AppliquerFondÉcran();
         }
         else
         {
            RetirerDondÉcran();
         }
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

      void AppliquerFondÉcran()
      {
         Application.Current.MainWindow.WindowState = WindowState.Maximized;
         Application.Current.MainWindow.WindowStyle = WindowStyle.None;
         Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;

      }

      private void LoadGameButton_Click(object sender, RoutedEventArgs e)
      {
         NavigationService.Navigate(new LoadGamePage());
      }
      private void SettingsButton_Click(object sender, RoutedEventArgs e)
      {
         NavigationService.Navigate(new SettingsPage());
      }
      private void NewGameButton_Click(object sender, RoutedEventArgs e)
      {
         NavigationService.Navigate(new NewGamePage());
      }

      private void CreditsButton_Click(object sender, RoutedEventArgs e)
      {
         NavigationService.Navigate(new CreditsPage());
      }


      private void Quit_Click(object sender, RoutedEventArgs e)
      {
         Application.Current.Shutdown();
      }
   }
}
