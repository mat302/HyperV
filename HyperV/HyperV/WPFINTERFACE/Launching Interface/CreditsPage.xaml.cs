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
         if (GererDonnees.Langue == 0) { LangueOficielleCredits = GererDonnees.ListeFrancais; }
         if (GererDonnees.Langue == 1) { LangueOficielleCredits = GererDonnees.ListeAnglais; }
         if (GererDonnees.Langue == 2) { LangueOficielleCredits = GererDonnees.ListeEspagnol; }
         if (GererDonnees.Langue == 3) { LangueOficielleCredits = GererDonnees.ListeJaponais; }

         sim.Text = LangueOficielleCredits[25];
         clg.Text = LangueOficielleCredits[26];
         TitreSett.Text = LangueOficielleCredits[24];
         annee.Text = LangueOficielleCredits[27];
         BackButton.Text = LangueOficielleCredits[0];
      }
   }
}
