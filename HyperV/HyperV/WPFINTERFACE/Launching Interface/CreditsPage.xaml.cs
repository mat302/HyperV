using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Launching_Interface
{
   /// <summary>
   /// Interaction logic for CreditsPage.xaml
   /// </summary>
   public partial class CreditsPage : Page
   {
      List<string> LangueOficielleCredits { get; set; }
      public CreditsPage()
      {
         LangueOficielleCredits = new List<string>();
         InitializeComponent();
         GérerLangues();
      }
      private void BackButton_Click(object sender, RoutedEventArgs e)
      {
         this.NavigationService.Navigate(new MainPage());
      }

      void GérerLangues()
      {
         switch (GererDonnees.Langue)
         {
            case GererDonnees.Langues.Francais:
               LangueOficielleCredits = GererDonnees.ListeFrancais;
               BackButton.Margin = new Thickness(35, 19, 101, 88);
               break;
            case GererDonnees.Langues.Anglais:
               LangueOficielleCredits = GererDonnees.ListeAnglais;
               BackButton.Margin = new Thickness(36, 19, 104, 88);
               break;
            case GererDonnees.Langues.Espagnol:
               LangueOficielleCredits = GererDonnees.ListeEspagnol;
               BackButton.Margin = new Thickness(31, 19, 109, 88);
               break;
            case GererDonnees.Langues.Japonais:
               LangueOficielleCredits = GererDonnees.ListeJaponais;
               BackButton.Margin = new Thickness(35, 19, 102, 88);
               break;


         }
         sim.Text = LangueOficielleCredits[25];
         clg.Text = LangueOficielleCredits[26];
         TitreSett.Text = LangueOficielleCredits[24];
         annee.Text = LangueOficielleCredits[27];
         BackButton.Text = LangueOficielleCredits[0];
      }
   }
}
